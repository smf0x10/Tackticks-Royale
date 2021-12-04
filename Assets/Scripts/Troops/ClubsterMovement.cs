using UnityEngine;

public class ClubsterMovement : Troop
{
    private float sweepRangeBonus = 0;
    private float atkDelayBonus = 0;
    private float moveSpeedBonus = 0;
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
        return 2 - atkDelayBonus;
    }

    public override float GetKnockback()
    {
        return 2000;
    }

    public override float GetAtkRange()
    {
        return 2.6f;
    }

    public override float GetGetUpDelay()
    {
        return 0.5f;
    }

    public override float GetMoveSpeed()
    {
        return 1.5f + moveSpeedBonus;
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
            if (t.GetTeam() != GetTeam() && t != atkTarget && Vector3.Distance(transform.position, t.transform.position) <= GetAtkRange() + 0.7f + sweepRangeBonus)
            {
                t.TakeDamage(this, GetAtk() - 1, true, GetKnockback());
            }
        }
    }

    public override void GainKingBuff()
    {
        sweepRangeBonus = 0.5f;
        atkDelayBonus = 0.5f;
        moveSpeedBonus = 0.5f;
    }

    public override void LoseKingBuff()
    {
        sweepRangeBonus = 0;
        atkDelayBonus = 0;
        moveSpeedBonus = 0;
    }

}

