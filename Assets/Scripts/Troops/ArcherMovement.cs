
public class ArcherMovement : Troop
{
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
        return 2;
    }

    public override float GetKnockback()
    {
        return 30;
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
}