public class FClubster : TroopFormation
{
    public override string GetName()
    {
        return "Clubster";
    }

    public override int GetCost()
    {
        return 4;
    }

    protected override void SpawnFormation(Team team)
    {
        SpawnTroop(TroopType.CLUBSTER, team);
    }
}
