using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;

/// <summary>
/// Base class for all troops
/// </summary>
public class Troop : NetworkBehaviour
{
    private Transform target;
    private float lastDist;
    private float atkTime;
    protected int hp;
    [SerializeField] private NetworkVariable<Team> team;
    [SerializeField] private MeshRenderer[] teamClothes; // Mesh renderers to change color depending on the team
    private float knockbackToNextTopple;
    private Animator animator;
    private NavMeshAgent agent;
    private Rigidbody rb;
    private float timeToGetUp;
    protected NetworkVariable<bool> IsRallied { get; set; }
    private RallyPoint rallyTarget;
    private NetworkVariable<Vector3> rallyPos; // Stores the position of the rally point, for pathfinding. Can't just use the position property of the rallyTarget because that variable can't be synced
    private GameObject selectedIndicator;

    protected static List<Troop> activeTroops = new List<Troop>();
    /// <summary>
    /// Troops die when they fall below this y coordinate
    /// </summary>
    public const float DEATH_HEIGHT = -50f;

    /// <summary>
    /// Run every frame; causes this troop to die if its hp reaches 0 or it falls out of the map
    /// </summary>
    protected virtual void HandleDeath()
    {
        if (hp <= 0 || transform.position.y < DEATH_HEIGHT)
        {
            Die();
        }
    }

    private void Awake()
    {
        activeTroops.Add(this);
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        atkTime = GetAtkDelay();
        hp = GetMaxHp();
        agent.speed = GetMoveSpeed();
        knockbackToNextTopple = GetKnockbackThreshold();
        Init();
    }

    /// <summary>
    /// Kills this troop, removes it from the list of active troops, and lets the rally point know what happened
    /// </summary>
    protected virtual void Die()
    {
        activeTroops.Remove(this);
        if (IsRallied.Value)
        {
            rallyTarget.RemoveTroopTarget();
        }
        Instantiate(TroopRegistry.instance.GetDeathParticle(), transform.position, transform.rotation);
        if (IsOwner)
        {
            DestroyServerRpc();
        }
    }

    /// <summary>
    /// Returns a list of all the troops in play, regardless of team
    /// </summary>
    /// <returns></returns>
    public static List<Troop> GetActiveTroops()
    {
        return activeTroops;
    }

    /// <summary>
    /// Returns a list of all the troops on the specified team
    /// </summary>
    /// <param name="t">The team to search for</param>
    /// <returns></returns>
    public static List<Troop> GetActiveTroops(Team t)
    {
        List<Troop> ret = new List<Troop>();
        for (int i = 0; i < activeTroops.Count; i++)
        {
            if (activeTroops[i].GetTeam() == t)
            {
                ret.Add(activeTroops[i]);
            }
        }
        return ret;
    }

    /// <summary>
    /// Additional code to run when this troop is spawned, after initializing the NavMeshAgent, Animator, and Rigidbody
    /// </summary>
    protected virtual void Init()
    {
        
    }

    /// <summary>
    /// Number of hit points this troop has
    /// </summary>
    public virtual int GetMaxHp()
    {
        return 1;
    }

    /// <summary>
    /// The range at which this troop can attack. 
    /// There is currently no projectile logic in place, so ranged units just have this set higher than normal
    /// </summary>
    public virtual float GetAtkRange()
    {
        return 2;
    }

    /// <summary>
    /// The number of hit points an attack from this troop deals
    /// </summary>
    public virtual int GetAtk()
    {
        return 0;
    }

    /// <summary>
    /// The knockback of this troop's attack
    /// </summary>
    public virtual float GetKnockback()
    {
        return 0;
    }

    /// <summary>
    /// The delay between attacks, in seconds
    /// </summary>
    public virtual float GetAtkDelay()
    {
        return 1;
    }

    /// <summary>
    /// The amount of time this troop must be lying on the ground before it can get up
    /// </summary>
    public virtual float GetGetUpDelay()
    {
        return 1;
    }

    /// <summary>
    /// The speed for this troop to move at
    /// </summary>
    /// <returns></returns>
    public virtual float GetMoveSpeed()
    {
        return 1;
    }

    /// <summary>
    /// The total amount of knockback that must be received before this troop starts being affected by physics.
    /// If an attack deals less knockback than this threshold, the amount of attempted knockback taken will be stored and used to reduce the threshold for the next attack.
    /// If 0, the troop will always fall over when recieving knockback
    /// </summary>
    /// <returns></returns>
    public virtual float GetKnockbackThreshold()
    {
        return 0;
    }

    /// <summary>
    /// The height for the indicator that this troop is selected to appear at
    /// </summary>
    /// <returns></returns>
    public virtual float GetSelectionHeight()
    {
        return 2;
    }

    protected virtual void Update()
    {
        ChooseTarget();
        agent.speed = GetMoveSpeed();
        if (atkTime > 0)
        {
            atkTime -= Time.deltaTime;
        }
        if (timeToGetUp > 0 && rb.velocity.magnitude < 0.5f)
        {
            timeToGetUp -= Time.deltaTime;
            if (timeToGetUp <= 0)
                GetUp();
        }
        HandleDeath();
    }

    private void ChooseTarget()
    {
        lastDist = int.MaxValue;
        for (int i = 0; i < activeTroops.Count; i++)
        {
            if (activeTroops[i] == null || activeTroops[i].GetTeam() == team.Value)
                continue;
            if (Vector3.Distance(activeTroops[i].transform.position, transform.position) < lastDist)
            {
                target = activeTroops[i].transform;
                lastDist = Vector3.Distance(activeTroops[i].transform.position, transform.position);
            }
        }
        if (target != null && CanTarget(target))
        {
            if (agent.enabled)
            {
                agent.destination = target.position;
                if (IsInRange(target))
                {
                    agent.isStopped = true;
                    // Random delay added for attacks to stop troops that execute their script first from always having priority
                    if (atkTime <= 0 && Random.Range(0, 2) == 1)
                        Attack(target.GetComponent<Troop>());
                }
                else
                {
                    agent.isStopped = false;
                }
            }
        }
        else if (agent.enabled)
        {
            agent.isStopped = false;
            if (IsRallied.Value)
            {
                //agent.destination = rallyTarget.transform.position;
                agent.SetDestination(rallyPos.Value);
            }
            else
            {
                if (team.Value == Team.Blue)
                {
                    agent.destination = TroopRegistry.instance.GetRedSpawn().position;
                }
                else
                {
                    agent.destination = TroopRegistry.instance.GetBlueSpawn().position;
                }
            }
        }
    }

    /// <summary>
    /// Checks if another troop is close enough for this troop to attack
    /// </summary>
    /// <param name="other">The troop to check</param>
    /// <returns></returns>
    protected virtual bool IsInRange(Transform other)
    {
        return Vector3.Distance(transform.position, other.position) <= GetAtkRange();
    }

    /// <summary>
    /// Checks if this troop can target something, given its transform information
    /// </summary>
    /// <param name="other">The transform belonging to the potential target</param>
    /// <returns></returns>
    protected virtual bool CanTarget(Transform other)
    {
        return Vector3.Distance(transform.position, other.position) < GetTargetDistance(IsRallied.Value) || 
            (IsRallied.Value && Vector3.Distance(rallyPos.Value, other.position) < GetRallyDefendDistance());
    }

    /// <summary>
    /// Attacks and knocks back a target using this troop's atk and kb stats
    /// </summary>
    /// <param name="atkTarget"></param>
    protected virtual void Attack(Troop atkTarget)
    {
        animator.Play("attack");
        atkTarget.TakeDamage(this, GetAtk(), true, GetKnockback());
        atkTime = GetAtkDelay();
    }

    /// <summary>
    /// Deals damage to this troop and applies knockback
    /// </summary>
    /// <param name="pwr">The damage done</param>
    /// <param name="takeKB">Should the attack cause knockback?</param>
    /// <param name="kbPwr">The force of the knockback. No effect if takeKB is false</param>
    public void TakeDamage(Troop attacker, int pwr, bool takeKB, float kbPwr = 0)
    {
        hp -= pwr;
        if (takeKB)
        {
            knockbackToNextTopple -= kbPwr;
            if (knockbackToNextTopple <= 0)
            {
                rb.isKinematic = false;
                agent.enabled = false;
                timeToGetUp = GetGetUpDelay();
                rb.AddExplosionForce(kbPwr, attacker.transform.position, GetAtkRange() * 2, 4);
                knockbackToNextTopple = GetKnockbackThreshold();
            }
        }
    }

    /// <summary>
    /// Makes this troop get up off the ground
    /// </summary>
    void GetUp()
    {
        rb.isKinematic = true;
        agent.enabled = true;
    }

    /// <summary>
    /// Returns the team this troop is on
    /// </summary>
    /// <returns>Team.BLUE or Team.RED</returns>
    public Team GetTeam()
    {
        return team.Value;
    }

    /// <summary>
    /// Updates the team this troop is on
    /// </summary>
    /// <param name="t">the new team</param>
    public void SetTeam(Team t)
    {
        // TODO: Set the troop's clothes
        team.Value = t;
        foreach (MeshRenderer m in teamClothes)
        {
            if (t == Team.Red)
            {
                m.material = TroopRegistry.instance.GetRedClothes();
            }
            else
            {
                m.material = TroopRegistry.instance.GetBlueClothes();
            }
        }
    }

    public virtual float GetTargetDistance(bool rallied)
    {
        if (rallied)
        {
            return 2;
        }
        else
        {
            return 8;
        }
    }

    public virtual float GetRallyDefendDistance()
    {
        return 8;
    }



    /// <summary>
    /// Sets this troop to target the specified point. Only works on the client that owns this troop
    /// </summary>
    /// <param name="newTarget">The rally point to target (must be instantiated before this function is run</param>
    public void SetRallyTarget(RallyPoint newTarget)
    {
        Deselect();
        if (IsRallied.Value)
        {
            if (newTarget == rallyTarget)
            {
                return; // Already targeting this point
            }
            rallyTarget.RemoveTroopTarget();
        }
        rallyTarget = newTarget;
        SetRallyTargetServerRpc(newTarget.transform.position);
        rallyTarget.AddTroopTarget();
        //Debug.Log(newTarget);
    }

    /// <summary>
    /// Sets the rally target on the server side. Only takes a position value, since physical rally points don't exist on the server side
    /// </summary>
    /// <param name="newPos">The new position to target</param>
    [ServerRpc]
    private void SetRallyTargetServerRpc(Vector3 newPos)
    {
        rallyPos.Value = newPos;
        IsRallied.Value = true;
    }

    /// <summary>
    /// Removes the rally target on the server side. This only changes the IsRallied variable; all physical rally point manipulation happens client-side
    /// </summary>
    [ServerRpc]
    private void RemoveRallyTargetServerRpc()
    {
        IsRallied.Value = false;
    }

    /// <summary>
    /// To be used when this troop dies to properly remove it
    /// </summary>
    [ServerRpc]
    private void DestroyServerRpc()
    {
        if (activeTroops.Contains(this))
        {
            activeTroops.Remove(this);
        }
        GetComponent<NetworkObject>().Despawn(true);
    }

    /// <summary>
    /// If this troop is targeting a rally point, makes it stop targeting the point and advance toward the portal
    /// </summary>
    public void RemoveRallyTarget()
    {
        Deselect();
        if (IsRallied.Value)
        {
            rallyTarget.RemoveTroopTarget();
        }
        RemoveRallyTargetServerRpc();
    }

    public void Select()
    {
        selectedIndicator = Instantiate(TroopRegistry.instance.GetSelectionIndicatorPrefab(), transform.position + new Vector3(0, GetSelectionHeight(), 0), transform.rotation, transform);
    }

    public void Deselect()
    {
        if (selectedIndicator != null)
        {
            Destroy(selectedIndicator);
        }
    }

    /// <summary>
    /// Run every frame while this troop is in its king's area of effect
    /// </summary>
    public virtual void UpdateWhileKingBuffed()
    {

    }

    /// <summary>
    /// Run when entering the king's area of effect
    /// </summary>
    public virtual void GainKingBuff()
    {

    }

    /// <summary>
    /// Run when exiting the king's area of effect. Any changes made when gaining a king buff should be undone here
    /// </summary>
    public virtual void LoseKingBuff()
    {

    }
}
