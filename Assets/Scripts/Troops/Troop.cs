using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Base class for all troops
/// </summary>
public class Troop : MonoBehaviour
{
    private Transform target;
    private GameObject targetObject;
    private float lastDist;
    private float atkTime;
    [SerializeField] private int hp;
    [SerializeField] protected Team team;
    /// <summary>
    /// The range at which this troop can attack. 
    /// There is currently no projectile logic in place, so ranged units just have this set higher than normal
    /// </summary>
    [SerializeField] protected float range;
    /// <summary>
    /// The number of hit points an attack from this troop deals
    /// </summary>
    [SerializeField] protected int atk;
    /// <summary>
    /// The knockback of this troop's attack
    /// </summary>
    [SerializeField] protected float kb;
    /// <summary>
    /// The delay between attacks, in seconds
    /// </summary>
    [SerializeField] protected float atkDelay;
    /// <summary>
    /// The amount of time this troop must be lying on the ground before it can get up
    /// </summary>
    [SerializeField] protected float getUpDelay;
    private Animator animator;
    private NavMeshAgent agent;
    private Rigidbody rb;
    private float timeToGetUp;

    public static List<Troop> activeTroops = new List<Troop>();
    /// <summary>
    /// The maximum distance from a potential target before a troop will not detect it
    /// </summary>
    public const float MAX_TARGET_DISTANCE = 9999f;
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
            activeTroops.Remove(this);
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        activeTroops.Add(this);
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        atkTime = atkDelay;
        Init();
    }

    /// <summary>
    /// Additional code to run when this troop is spawned, after initializing the NavMeshAgent, Animator, and Rigidbody
    /// </summary>
    protected virtual void Init()
    {
        
    }

    protected virtual void Update()
    {
        lastDist = MAX_TARGET_DISTANCE;
        for (int i = 0; i < activeTroops.Count; i++)
        {
            if (activeTroops[i].GetTeam() == team)
                continue;
            if (Vector3.Distance(activeTroops[i].transform.position, transform.position) < lastDist)
            {
                target = activeTroops[i].transform;
                targetObject = activeTroops[i].gameObject;
                lastDist = Vector3.Distance(activeTroops[i].transform.position, transform.position);
            }
        }
        if (lastDist < MAX_TARGET_DISTANCE)
        {
            if (agent.enabled)
            {
                agent.destination = target.position;
                if (Vector3.Distance(transform.position, targetObject.transform.position) <= range)
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
            agent.destination = new Vector3(-32, 2, 0);
        }

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

    /// <summary>
    /// Attacks and knocks back a target using this troop's atk and kb stats
    /// </summary>
    /// <param name="atkTarget"></param>
    protected virtual void Attack(Troop atkTarget)
    {
        animator.Play("attack");
        atkTarget.TakeDamage(this, atk, true, kb);
        atkTime = atkDelay;
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
            rb.isKinematic = false;
            agent.enabled = false;
            timeToGetUp = getUpDelay;
            rb.AddExplosionForce(kbPwr, attacker.transform.position, range * 2, 4);
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

    public Team GetTeam()
    {
        return team;
    }

    public void SetTeam(Team t)
    {
        team = t;
    }
}
