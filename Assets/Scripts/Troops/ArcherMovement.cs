
public class ArcherMovement : ProjectileTroop
{
    private float knockbackBonus;
    private float knockbackThresholdBonus;
    private float atkDelayBonus;
    public override int GetMaxHp()
    {
        return 3;
    }

    public override int GetAtk()
    {
        return 1;
    }

    public override float GetAtkDelay()
    {
        return 1 - atkDelayBonus;
    }

    public override float GetKnockback()
    {
        return 30 + knockbackBonus;
    }

    public override float GetAtkRange()
    {
        return 6;
    }

    public override float GetGetUpDelay()
    {
        return 2;
    }

    public override float GetMoveSpeed()
    {
        return 2;
    }

    public override float GetKnockbackThreshold()
    {
        return 10 + knockbackThresholdBonus;
    }

    public override float GetTargetDistance(bool rallied)
    {
        return 7;
    }

    public override void GainKingBuff()
    {
        knockbackBonus = 30;
        knockbackThresholdBonus = 30;
        atkDelayBonus = 0.1f;
    }

    public override void LoseKingBuff()
    {
        knockbackThresholdBonus = 0;
        knockbackBonus = 0;
        atkDelayBonus = 0;
    }
}