using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class SoldierMovement : Troop {
    public override int GetMaxHp()
    {
        return 5;
    }

    public override int GetAtk()
    {
        return 2;
    }

    public override float GetAtkDelay()
    {
        return 2;
    }

    public override float GetKnockback()
    {
        return 300;
    }

    public override float GetAtkRange()
    {
        return 2.5f;
    }

    public override float GetGetUpDelay()
    {
        return 3;
    }

    public override float GetMoveSpeed()
    {
        return 2.5f;
    }
}
