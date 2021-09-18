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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Troop>())
        {
            selected.Remove(other.GetComponent<Troop>());
        }
    }

    public List<Troop> GetSelection()
    {
        return selected;
    }
}
