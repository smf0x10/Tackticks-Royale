using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// The player script. One of these gets instantiated for each player.
/// </summary>
public class SpawnManager: MonoBehaviour {
    private float summonEnergy;
    [SerializeField] private CameraMovement cameraMovement;
    private Vector2 mouseDelta = new Vector2();
    private float mouseSensitivity = 0.5f;
    private float scroll = 0;
    private float scrollSensitivity = 1f;
    [SerializeField] private Team team;

    [SerializeField] private GameObject spawnButton;
    [SerializeField] private Transform canvas;
    [SerializeField] private string[] formations;

    [SerializeField] private GameObject selectionCommands;
    [SerializeField] private GameObject generalCommands;
    [SerializeField] private UnityEngine.EventSystems.EventSystem eventSystem;

    // Rally troop selection
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject selectionPrefab;
    private GameObject currentSelection;
    private bool draggingSelection;
    private bool mouseDown = false;
    private bool rMouseDown = false;
    private Vector3 draggingOrigin;
    private RaycastHit currentDrag;

    private List<Troop> selectedTroops;
    public static float SE_FILL_SPEED = 1f;

    private King king;

    private const int NO_TROOP_COLLISIONS = ~(1 << 9); // Pass into the layermask parameter of a raycast to ignore troops


    private void Awake()
    {
        cameraMovement.SetTeam(team);

        FKing kingForm = ScriptableObject.CreateInstance<FKing>();
        kingForm.TrySpawnFormation(this);
        king = kingForm.GetLastTroopSpawned();
        Debug.Log(king);

        summonEnergy = 5;
        for (int i = 0; i < formations.Length; i++)
        {
            Button sp = Instantiate(spawnButton, canvas.GetChild(i), false).GetComponent<Button>();
            TroopFormation tf = (TroopFormation)ScriptableObject.CreateInstance(formations[i]);

            sp.GetComponentInChildren<Text>().text = tf.GetName() + "\n" + tf.GetCost();

            sp.onClick.AddListener(() => tf.TrySpawnFormation(this));
        }
    }

    /// <summary>
    /// Returns the current summon energy level
    /// </summary>
    /// <returns>The current summon energy as a raw floating point value without rounding</returns>
    public float GetSummonEnergy()
    {
        return summonEnergy;
    }

    /// <summary>
    /// Returns this spawn manager's team
    /// </summary>
    /// <returns>The team</returns>
    public Team GetTeam()
    {
        return team;
    }

    /// <summary>
    /// Uses up some summon energy, provided there is enough summon energy available
    /// </summary>
    /// <param name="amount">The amount to use</param>
    /// <returns>True if there was enough summon energy available, false otherwise</returns>
    public bool UseSummonEnergy(int amount)
    {
        if (summonEnergy >= amount)
        {
            summonEnergy -= amount;
            return true;
        }
        return false;
    }

	
	void Update ()
    {
        if (summonEnergy < 20f)
        {
            summonEnergy += SE_FILL_SPEED * Time.deltaTime;
        }
        if (draggingSelection)
        {
            if (Physics.Raycast(cam.ScreenPointToRay(Mouse.current.position.ReadValue()), out currentDrag, 500, NO_TROOP_COLLISIONS, QueryTriggerInteraction.Ignore))
            {
                currentSelection.transform.localScale = new Vector3(Mathf.Abs(draggingOrigin.x - currentDrag.point.x), Mathf.Abs(draggingOrigin.y - currentDrag.point.y) + 1, Mathf.Abs(draggingOrigin.z - currentDrag.point.z));
                currentSelection.transform.position = (draggingOrigin + currentDrag.point) / 2;
            }
        }
        if (rMouseDown)
        {
            cameraMovement.Rotate(mouseDelta * mouseSensitivity);
        }
        if (scroll != 0)
        {
            cameraMovement.MoveForward(scroll * scrollSensitivity);
        }
	}

    /// <summary>
    /// Run when the right mouse button is pressed or released
    /// </summary>
    /// <param name="ctx">Use ReadValueAsButton() to get a boolean saying if the button is pressed</param>
    public void OnRClick(InputAction.CallbackContext ctx)
    {
        rMouseDown = ctx.ReadValueAsButton();
    }

    /// <summary>
    /// Run when the mouse speed changes
    /// </summary>
    /// <param name="ctx">Use ReadValueAsObject and cast to Vector2 to get the mouse delta</param>
    public void OnMouse(InputAction.CallbackContext ctx)
    {
        mouseDelta = (Vector2)ctx.ReadValueAsObject();
    }

    /// <summary>
    /// Run when the mouse wheel is scrolled. On mice with multiple scroll axes, only the y component will be detected
    /// </summary>
    /// <param name="ctx">Use ReadValueAsObject() and cast to float to get the scroll value</param>
    public void OnScroll(InputAction.CallbackContext ctx)
    {
        scroll = (float)ctx.ReadValueAsObject();
    }

    /// <summary>
    /// Called when the left mouse button is clicked or released
    /// </summary>
    public void OnClick(InputAction.CallbackContext ctx)
    {
        bool mouseWasDown = mouseDown;
        mouseDown = ctx.ReadValueAsButton();
        if (mouseDown && (mouseWasDown || eventSystem.IsPointerOverGameObject()))
        {
            return; // To stop the event from triggering twice upon pressing the mouse or from clicking buttons
        }
        if (!mouseDown && draggingSelection) // stop selecting troops
        {
            draggingSelection = false;
            selectedTroops = currentSelection.GetComponent<SelectionBox>().GetSelection();
            if (selectedTroops.Count > 0)
            {
                EnterSelectedMode();
            }
            Destroy(currentSelection);
            return;
        }
        if (mouseDown && selectedTroops != null && selectedTroops.Count > 0) // set a rally point
        {
            Physics.Raycast(cam.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hit, 500, NO_TROOP_COLLISIONS, QueryTriggerInteraction.Collide);
            if (hit.collider.GetComponentInParent<RallyPoint>())
            {
                RallyPoint r = hit.collider.GetComponentInParent<RallyPoint>();
                SetSelectionTarget(r);
            }
            else if (hit.collider != null && hit.normal.y > Mathf.Sqrt(3) / 3)
            {
                RallyPoint r = Instantiate(TroopRegistry.instance.GetRallyPointPrefab(), hit.point, Quaternion.Euler(0, 0, 0)).GetComponent<RallyPoint>();
                SetSelectionTarget(r);
            }
            return;
        }
        if (mouseDown && !draggingSelection) // Start selecting troops
        {
            Physics.Raycast(cam.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hit, 500, NO_TROOP_COLLISIONS, QueryTriggerInteraction.Ignore);
            currentSelection = Instantiate(selectionPrefab, hit.point, new Quaternion(0, 0, 0, 0));
            currentSelection.GetComponent<SelectionBox>().SetTeam(team);
            draggingOrigin = hit.point;
            draggingSelection = true;
        }
    }

    /// <summary>
    /// Shows the selection commands in the HUD
    /// </summary>
    private void EnterSelectedMode()
    {
        selectionCommands.SetActive(true);
        generalCommands.SetActive(false);
    }

    /// <summary>
    /// Sets the selected troop array to null and hides the selection command menu
    /// </summary>
    private void ExitSelectedMode()
    {
        selectedTroops = null;
        selectionCommands.SetActive(false);
        generalCommands.SetActive(true);
    }

    /// <summary>
    /// Sets the selected troops to target a position
    /// </summary>
    /// <param name="pos">The position to target</param>
    private void SetSelectionTarget(RallyPoint target)
    {
        foreach(Troop t in selectedTroops)
        {
            if (t == null)
            {
                continue;
            }
            t.SetRallyTarget(target);
        }
        ExitSelectedMode();
    }

    /// <summary>
    /// Makes all selected troops abandon their rally point and advance
    /// </summary>
    public void RemoveSelectionTarget()
    {
        if (selectedTroops == null)
        {
            return;
        }
        foreach (Troop t in selectedTroops)
        {
            if (t == null)
            {
                continue;
            }
            t.RemoveRallyTarget();
        }
        ExitSelectedMode();
    }

    /// <summary>
    /// Deselects all the selected troops
    /// </summary>
    public void CancelSelection()
    {
        if (selectedTroops == null)
        {
            return;
        }
        foreach (Troop t in selectedTroops)
        {
            if (t == null)
            {
                continue;
            }
            t.Deselect();
        }
        ExitSelectedMode();
    }

    public void FullAdvance()
    {
        foreach(Troop t in Troop.GetActiveTroops(team))
        {
            t.RemoveRallyTarget();
        }
    }
}

public enum Team
{
    Blue,
    Red
}