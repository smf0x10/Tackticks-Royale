using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Troop
{
    private List<Troop> inRange = new List<Troop>();
    public override int GetMaxHp()
    {
        return 30;
    }

    public override int GetAtk()
    {
        return 2;
    }

    public override float GetAtkDelay()
    {
        return 0.5f;
    }

    public override float GetKnockback()
    {
        return 250;
    }

    public override float GetAtkRange()
    {
        return 2.5f;
    }

    public override float GetGetUpDelay()
    {
        return 0.8f;
    }

    public override float GetMoveSpeed()
    {
        return 2;
    }

    public override float GetKnockbackThreshold()
    {
        return 1000;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<Troop>() || other.GetComponent<Troop>().GetTeam() != GetTeam())
        {
            return;
        }
        inRange.Add(other.GetComponent<Troop>());
    }

    protected override void Update()
    {
        for  (int i = 0; i < inRange.Count; i++)
        {
            if (inRange[i] == null)
            {
                inRange.RemoveAt(i); // Flush out defeated troops in the list. Might wanna find a better way to do this
            }
        }
        foreach (Troop t in inRange)
        {
            t.UpdateWhileKingBuffed();
        }
        base.Update();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<Troop>() || other.GetComponent<Troop>().GetTeam() != GetTeam())
        {
            return;
        }
        Debug.Log("Exunt");
        inRange.Remove(other.GetComponent<Troop>());
    }
}
