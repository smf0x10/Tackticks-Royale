using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTroop : Troop
{

    protected override void Attack(Troop atkTarget)
    {
        base.Attack(atkTarget);
        Vector3 shotVec = atkTarget.transform.position - transform.position;
        Vector3 shotPos = (transform.position + atkTarget.transform.position) / 2;
        GameObject shot = Instantiate(GetProjectile(), shotPos, Quaternion.FromToRotation(Vector3.forward, shotVec));
        float shotDist = Vector3.Magnitude(shotVec);
        shot.transform.localScale = new Vector3(1, 1, shotDist / 2);
    }

    /// <summary>
    /// Returns the prefab for this troop to use as the projectile.
    /// The projectile is purely visual; stats are all determined by the other stat functions
    /// </summary>
    /// <returns></returns>
    protected GameObject GetProjectile()
    {
        Debug.Log(ProjectileRegistry.GetProjectilePrefab(ProjectileType.ARROW));
        return ProjectileRegistry.GetProjectilePrefab(ProjectileType.ARROW);
    }

    protected override bool IsInRange(Transform other)
    {
        if (other.position.y > transform.position.y)
        {
            return base.IsInRange(other);
        }
        float heightAdvantage = transform.position.y - other.transform.position.y;
        return Vector3.Distance(transform.position, other.position) <= Mathf.Sqrt(Mathf.Pow(GetAtkRange(), 2) + Mathf.Pow(heightAdvantage, 2));
    }

}
