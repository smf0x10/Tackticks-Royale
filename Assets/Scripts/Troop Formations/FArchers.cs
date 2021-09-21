public class FArchers : TroopFormation
{
    public override string GetName()
    {
        return "Archers";
    }

    public override int GetCost()
    {
        return 3;
    }

    protected override void SpawnFormation(Team team)
    {
        for (int i = 0; i < 3; i++)
        {
            SpawnTroop(TroopType.ARCHER, team);
        }
    }
}
