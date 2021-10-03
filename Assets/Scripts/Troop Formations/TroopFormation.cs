using System;
using UnityEngine;

/// <summary>
/// Class representing a troop formation. One subclass of this is instantiated for each spawn button on the HUD. Subclasses of this class are named starting with an F
/// example: FSoldiers
/// TODO: Actually make it do this
/// </summary>
public class TroopFormation : ScriptableObject
{
    private const int DEFAULT_COST = 1;

    /// <summary>
    /// Returns the summon energy cost of this formation. Override this method to change it
    /// </summary>
    /// <returns>The summon energy cost to spawn this formation once</returns>
    public virtual int GetCost()
    {
        return DEFAULT_COST;
    }

    /// <summary>
    /// Returns the name to show on the spawn button
    /// </summary>
    /// <returns>The name to show on the spawn button; the energy cost is added later</returns>
    public virtual string GetName()
    {
        return "none";
    }

    /// <summary>
    /// Spawns a single troop of the given type on the given team
    /// </summary>
    /// <param name="troop">The troop to spawn</param>
    /// <param name="team">The side the troop is on</param>
    protected static void SpawnTroop(TroopType troopType, Team team)
    {
        Vector3 pos;
        Quaternion rot;
        if (team == Team.Blue)
        {
            pos = TroopRegistry.instance.GetBlueSpawn().position;
            rot = TroopRegistry.instance.GetBlueSpawn().rotation;
        }
        else
        {
            pos = TroopRegistry.instance.GetRedSpawn().position;
            rot = TroopRegistry.instance.GetRedSpawn().rotation;
        }

        GameObject newTroop = Instantiate(TroopRegistry.instance.GetTroopPrefab(troopType), pos, rot);

        newTroop.GetComponent<Troop>().SetTeam(team);
        // TODO: Set clothes color properly as well
        //throw new NotImplementedException();
    }

    /// <summary>
    /// Spawns this formation. Does not check or use up summon energy reserves; these operations are done when deciding whether to call the method in the first place
    /// </summary>
    protected virtual void SpawnFormation(Team team)
    {
        Debug.LogWarning("The SpawnFormation() function has not been overridden for this class. No troops will be spawned.");
    }

    /// <summary>
    /// Spawns the troops in this formation, if the spawn manager has enough summon energy available
    /// </summary>
    /// <param name="manager"></param>
    public void TrySpawnFormation(SpawnManager manager)
    {
        if (manager.UseSummonEnergy(GetCost()))
        {
            SpawnFormation(manager.GetTeam());
        }
    }
}
