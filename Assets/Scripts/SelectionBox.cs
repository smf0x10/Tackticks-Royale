using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBox : MonoBehaviour
{

    [SerializeField] private List<Troop> selected = new List<Troop>();
    private Team team;

    /// <summary>
    /// Sets the team of this box. It will only select troops from its team
    /// </summary>
    /// <param name="t"></param>
    public void SetTeam(Team t)
    {
        team = t;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Troop>() && other.GetComponent<Troop>().GetTeam() == team)
        {
            selected.Add(other.GetComponent<Troop>());
            other.GetComponent<Troop>().Select();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Troop>() && other.GetComponent<Troop>().GetTeam() == team)
        {
            selected.Remove(other.GetComponent<Troop>());
            other.GetComponent<Troop>().Deselect();
        }
    }

    /// <summary>
    /// Returns the troops selected by this box
    /// </summary>
    /// <returns>A list containing the troop components currently in this selection box</returns>
    public List<Troop> GetSelection()
    {
        Debug.Log(selected.Count);
        return selected;
    }
}
