
public class SoldierMovement : Troop {
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
        return 1.5f;
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
        return 3;
    }

    public override float GetMoveSpeed()
    {
        return 2.5f;
    }

    public override float GetKnockbackThreshold()
    {
        return 350;
    }
}
