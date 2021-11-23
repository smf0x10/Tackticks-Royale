using UnityEngine;

/// <summary>
/// A centralized registry where troop types are matched to the actual troop that spawns
/// </summary>
public class TroopRegistry : MonoBehaviour
{
    public static TroopRegistry instance;

    [SerializeField] private GameObject[] troops;

    [SerializeField] private Transform blueSpawn;
    [SerializeField] private Transform redSpawn;
    [SerializeField] private Material blueClothes;
    [SerializeField] private Material redClothes;
    [SerializeField] private GameObject selectionIndicatorPrefab;
    [SerializeField] private GameObject rallyPointPrefab;
    [SerializeField] private GameObject deathParticle;

    /// <summary>
    /// Returns the list of troop GameObjects
    /// </summary>
    /// <returns>The aforementioned list. Use the index (int)TroopType.INTERNAL_NAME to access that troop</returns>
    public GameObject[] GetTroopList()
    {
        return troops;
    }

    public GameObject GetTroopPrefab(TroopType type)
    {
        return troops[(int)type];
    }

    public GameObject GetSelectionIndicatorPrefab()
    {
        return selectionIndicatorPrefab;
    }

    public GameObject GetRallyPointPrefab()
    {
        return rallyPointPrefab;
    }

    /// <summary>
    /// Returns the blue team spawn point
    /// </summary>
    /// <returns>Use the position and rotation properties to determine where to spawn troops</returns>
    public Transform GetBlueSpawn()
    {
        return blueSpawn;
    }

    /// <summary>
    /// Returns the red team spawn point
    /// </summary>
    /// <returns>Use the position and rotation properties to determine where to spawn troops</returns>
    public Transform GetRedSpawn()
    {
        return redSpawn;
    }

    public Material GetBlueClothes()
    {
        return blueClothes;
    }

    public Material GetRedClothes()
    {
        return redClothes;
    }

    /// <summary>
    /// Returns the prefab for the particle effect used when troops die
    /// </summary>
    /// <returns></returns>
    public GameObject GetDeathParticle()
    {
        return deathParticle;
    }

    private void Awake()
    {
        instance = this;
    }
}

/// <summary>
/// Used to easily access the troop prefabs from anywhere. When a troop is added to the troops array, add its internal name here
/// </summary>
public enum TroopType
{
    SOLDIER,
    ARCHER,
    CLUBSTER,
    KING
}
