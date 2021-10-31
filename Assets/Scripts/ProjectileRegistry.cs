using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides an easy access point for projectile prefabs
/// </summary>
public class ProjectileRegistry : MonoBehaviour
{
    [SerializeField] private GameObject[] projectileList;

    public static ProjectileRegistry instance;

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Returns the prefab for a projectile
    /// </summary>
    /// <param name="type">The type of projectile to get</param>
    public static GameObject GetProjectilePrefab(ProjectileType type)
    {
        return instance.projectileList[(int)type];
    }
}

public enum ProjectileType
{
    ARROW
}
