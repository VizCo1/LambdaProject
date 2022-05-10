using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    protected NavMeshAgent agent;

    protected Transform player;

    [SerializeField]
    protected LayerMask whatIsGround, whatIsPlayer;

    protected float health;

    //Patroling
    protected Vector3 walkPoint;
    protected bool walkPointSet;
    [SerializeField]
    protected float walkPointRange;

    //Attacking
    [SerializeField]
    protected float timeBetweenAttacks;
    protected bool alreadyAttacked;
    [SerializeField]
    protected GameObject projectile;
    [SerializeField]
    protected Transform shootingTransform;

    //States
    [SerializeField]
    protected float sightRange, attackRange;
    protected bool playerInSightRange, playerInAttackRange;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        //health = 100;
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

        if (!alreadyAttacked)
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

    protected virtual void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
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
        transform.LookAt(player);
    }

    protected virtual string Type()
    {
        return "Enemy";
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
