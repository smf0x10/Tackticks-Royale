
public class SoldierMovement : Troop {
    private float atkDelayBonus = 0;
    private float getUpDelayBonus = 0;
    public override int GetMaxHp()
    {
        return 5;
    }

    public override int GetAtk()
    {
        return 2;
    }

    public override float GetAtkDelay()
    {
        return 1.5f - atkDelayBonus;
    }

    public override float GetKnockback()
    {
        return 250;
    }

    public override float GetAtkRange()
    {
        return 2.4f;
    }

    public override float GetGetUpDelay()
    {
        return 3 - getUpDelayBonus;
    }

    public override float GetMoveSpeed()
    {
        return 2.5f;
    }

    public override float GetKnockbackThreshold()
    {
        return 350;
    }

    public override void GainKingBuff()
    {
        atkDelayBonus = 0.5f;
        getUpDelayBonus = 1;
    }

    public override void LoseKingBuff()
    {
        atkDelayBonus = 0;
        getUpDelayBonus = 0;
    }
}
