public class FSoldiers : TroopFormation
{
    public override string GetName()
    {
        return "Soldiers";
    }

    public override int GetCost()
    {
        return 3;
    }

    protected override void SpawnFormation(Team team)
    {
        for (int i = 0; i < 3; i++)
        {
            SpawnTroop(TroopType.SOLDIER, team);
        }
    }
}
