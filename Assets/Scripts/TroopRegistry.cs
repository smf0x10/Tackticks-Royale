using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A centralized registry where troop types are matched to the actual troop that spawns
/// </summary>
public class TroopRegistry : MonoBehaviour
{
    public static TroopRegistry instance;

    public GameObject[] troops;

    private void Awake()
    {
        instance = this;
    }
}

public enum TroopType
{
    SOLDIER,
    ARCHER,
    CLUBSTER
}
