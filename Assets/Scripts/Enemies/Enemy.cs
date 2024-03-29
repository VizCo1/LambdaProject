using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    protected NavMeshAgent agent;

    protected Transform player;

    public bool isEnemyBlue;

    [SerializeField]
    protected AudioSource attackAudio;

    [SerializeField]
    protected AudioSource damageSound;

    [SerializeField]
    protected float rotationSpeed;
    protected bool playerIsInFront;

    [SerializeField]
    protected LayerMask whatIsGround, whatIsPlayer;

    protected float health;
    protected float maxHealth;

    //Patroling
    protected Vector3 walkPoint;
    protected bool walkPointSet;
    [SerializeField]
    protected float walkPointRange;

    //Attacking
    [SerializeField]
    protected float timeBetweenAttacks;
    protected bool alreadyAttacked;

    //States
    [SerializeField]
    protected float sightRange, attackRange;
    protected bool playerInSightRange, playerInAttackRange;

    [SerializeField]
    protected Transform rayCastTransform;

    [SerializeField]
    protected ParticleSystem takeDamageVFX;
    [SerializeField]
    protected ParticleSystem healVFX;

    private bool isInvulnerable;

    [SerializeField]
    protected DissolveEnemy dissolveEnemy;

    private Vector3 initialPosition;
    private Quaternion initialRotation;


    protected virtual void Awake()
    {
        isInvulnerable = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        damageSound.volume = 0.25f;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    protected virtual void FixedUpdate()
    {
        Debug.DrawLine(rayCastTransform.position, transform.forward, Color.black);

        RaycastHit hit;
        if (Physics.Raycast(rayCastTransform.position, transform.forward, out hit, 4f))
        {
            //Debug.Log(hit.transform.tag);
            if (hit.transform.CompareTag("Player"))
            {
                playerIsInFront = true;
            }
            else
            {
                playerIsInFront = false;
            }
        }
        else
        {
            playerIsInFront = false;
        }
    }

    protected virtual void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    protected virtual void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    protected virtual void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    protected virtual void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        LookAtPlayer();

        if (!alreadyAttacked && playerIsInFront)
        {
            ///Attack code here
            LaunchAttack();
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    protected virtual void ResetAttack()
    {
        alreadyAttacked = false;
    }

    protected virtual void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    protected virtual void LaunchAttack()
    {

    }

    protected virtual void LookAtPlayer()
    {
        Quaternion lookRotation = Quaternion.LookRotation(player.position - transform.position);
        lookRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        Quaternion initialRotation = transform.rotation;

        transform.rotation = Quaternion.Slerp(initialRotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    public virtual void TakeDamage(float damageAmount, bool isSwordBlue)
    {
        toInvulnerable();

        PlayDamageSound();

        Debug.Log("Enemy takes damage");

        health -= damageAmount;

        damageSound.pitch = 1f;
        takeDamageVFX.Play();

        if (health <= 0)
        {
            Debug.Log("Starting dissolving");

            dissolveEnemy.StartDissolveAction();
            if (!dissolveEnemy.isThisEnemyABoss)
                Destroy(gameObject);
        }
    }

    public void Heal(float healingAmount)
    {
        toInvulnerable();

        damageSound.pitch = 2f;
        PlayHealingSound();

        Debug.Log("Enemy is healing");

        health = health + healingAmount > maxHealth ? maxHealth : health + healingAmount;
        healVFX.Play();
    }

    protected void toInvulnerable()
    {
        isInvulnerable = true;
        Invoke(nameof(toVulnerable), 0.15f);
    }

    private void toVulnerable()
    {
        isInvulnerable = false;
    }

    public bool isEnemyInvulnerable()
    {
        return isInvulnerable;
    }

    public void PlayDamageSound()
    {
        damageSound.Play();
    }

    public void PlayHealingSound()
    {
        damageSound.Play();
    }

    public virtual void ResetEnemy()
    {
        health = maxHealth;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        alreadyAttacked = false;
        isInvulnerable = false;
    }
}
