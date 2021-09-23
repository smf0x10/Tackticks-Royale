using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine;

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

    // Rally troop selection
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject selectionPrefab;
    private GameObject currentSelection;
    private bool draggingSelection;
    private bool mouseDown = false;
    private bool rMouseDown = false;
    private Vector3 draggingOrigin;
    private RaycastHit currentDrag;


    private void Awake()
    {
        cameraMovement.SetTeam(team);

        for (int i = 0; i < formations.Length; i++)
        {
            Button sp = Instantiate(spawnButton, canvas.GetChild(i), false).GetComponent<Button>();
            TroopFormation tf = (TroopFormation)ScriptableObject.CreateInstance(formations[i]);

            sp.GetComponentInChildren<Text>().text = tf.GetName() + "\n" + tf.GetCost();

            sp.onClick.AddListener(() => tf.TrySpawnFormation(this));
        }
        // Debugging
        //summonEnergy = 3;
        //TroopFormation t = ScriptableObject.CreateInstance<FSoldiers>();
        //t.TrySpawnFormation(this);
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
            summonEnergy += 0.5f * Time.deltaTime;
        }
        if (draggingSelection)
        {

            if (Physics.Raycast(cam.ScreenPointToRay(Mouse.current.position.ReadValue()), out currentDrag, 500, 1, QueryTriggerInteraction.Ignore))
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

    ///// <summary>
    ///// Spawns three soldiers on this SpawnManager's team
    ///// </summary>
    //public void Soldiers()
    //{
    //    // TODO: Create a scaleable interface for adding new troop formations
    //    for (int i = 0; i < 3; i++)
    //    {
    //        Troop t = Instantiate(soldier, p1Spawn, p1SpawnRotation).GetComponent<Troop>();
    //        t.SetTeam(team);
    //    }
    //    summonEnergy -= 3; // Just using a constant for now
    //}

    ///// <summary>
    ///// Spawns two archers on this SpawnManager's team
    ///// </summary>
    //public void Archers()
    //{
    //    for (int i = 0; i < 2; i++)
    //    {
    //        Troop t = Instantiate(archer, p1Spawn, p1SpawnRotation).GetComponent<Troop>();
    //        t.SetTeam(team);
    //    }
    //    summonEnergy -= 3; // Just using a constant for now
    //}

    ///// <summary>
    ///// Spawns one clubster on this SpawnManager's team
    ///// </summary>
    //public void Clubster()
    //{
    //    for (int i = 0; i < 1; i++)
    //    {
    //        Troop t = Instantiate(clubster, p1Spawn, p1SpawnRotation).GetComponent<Troop>();
    //        t.SetTeam(team);
    //    }
    //    summonEnergy -= 4; // Just using a constant for now
    //}

    /// <summary>
    /// Called when the left mouse button is clicked or released
    /// </summary>
    public void OnClick()
    {
        mouseDown = !mouseDown;
        if (!mouseDown && draggingSelection)
        {
            draggingSelection = false;
            return;
        }
        RaycastHit[] thingsClicked = Physics.RaycastAll(cam.ScreenPointToRay(Mouse.current.position.ReadValue()), 500, 1, QueryTriggerInteraction.Ignore);
        for (int i = 0; i < thingsClicked.Length; i++)
        {
            if (thingsClicked[i].collider.GetComponent<Troop>())
                continue;
            currentSelection = Instantiate(selectionPrefab, thingsClicked[i].point, new Quaternion(0, 0, 0, 0));
            draggingOrigin = thingsClicked[i].point;
            draggingSelection = true;
            break;
        }
    }
}

public enum Team
{
    Blue,
    Red
}