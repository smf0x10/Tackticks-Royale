using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBox : MonoBehaviour
{

    [SerializeField] private List<Troop> selected = new List<Troop>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Troop>())
        {
            selected.Add(other.GetComponent<Troop>());
            other.GetComponent<Troop>().Select();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Troop>())
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
        return selected;
    }
}
