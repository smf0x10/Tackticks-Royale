using UnityEngine;

public class ClubsterMovement : Troop
{
    protected override void Attack(Troop atkTarget)
    {
        base.Attack(atkTarget);

        foreach (Troop t in activeTroops)
        {
            if (t.GetTeam() != team && t != atkTarget && Vector3.Distance(transform.position, t.transform.position) <= range + 0.5f)
            {
                t.TakeDamage(this, atk - 1, true, kb);
            }
        }
    }
}

