using UnityEngine;

public class ClubsterMovement : Troop
{
    public override int GetMaxHp()
    {
        return 10;
    }

    public override int GetAtk()
    {
        return 2;
    }

    public override float GetAtkDelay()
    {
        return 2;
    }

    public override float GetKnockback()
    {
        return 2000;
    }

    public override float GetAtkRange()
    {
        return 2.7f;
    }

    public override float GetGetUpDelay()
    {
        return 0.5f;
    }

    public override float GetMoveSpeed()
    {
        return 1.5f;
    }

    public override float GetKnockbackThreshold()
    {
        return 1000;
    }

    protected override void Attack(Troop atkTarget)
    {
        base.Attack(atkTarget);

        foreach (Troop t in activeTroops)
        {
            if (t.GetTeam() != team && t != atkTarget && Vector3.Distance(transform.position, t.transform.position) <= GetAtkRange() + 0.5f)
            {
                t.TakeDamage(this, GetAtk() - 1, true, GetKnockback());
            }
        }
    }

    
}

