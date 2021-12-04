
public class ArcherMovement : ProjectileTroop
{
    private float knockbackBonus;
    private float knockbackThresholdBonus;
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
        return 1;
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

    public override void GainKingBuff()
    {
        knockbackBonus = 30;
        knockbackThresholdBonus = 30;
    }

    public override void LoseKingBuff()
    {
        knockbackThresholdBonus = 0;
        knockbackBonus = 0;
    }
}