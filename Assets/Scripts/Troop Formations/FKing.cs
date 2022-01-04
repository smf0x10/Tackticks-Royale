
/// <summary>
/// Only used to help spawn each player's king at the start of the game
/// </summary>
public class FKing : TroopFormation
{
    private King lastSpawned;

    protected override void SpawnFormation(Team team)
    {
        SpawnTroop(TroopType.KING, team);
    }

    /// <summary>
    /// Returns the last king that this spawned. If all goes well, this is the only king this formation spawned
    /// </summary>
    public King GetLastTroopSpawned()
    {
        return lastSpawned;
    }

    public override int GetCost()
    {
        return 0;
    }
}
