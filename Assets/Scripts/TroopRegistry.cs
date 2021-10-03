using System.Collections.Generic;
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
    CLUBSTER
}
