using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantBoss : Enemy
{
    [SerializeField]
    private Animator anim;

    [SerializeField]
    private GameObject rightFootCollider;
    [SerializeField]
    private GameObject leftFootCollider;
    [SerializeField]
    private ParticleSystem meleAttackParticles;
    [SerializeField]
    private Transform leftFootBone;
    [SerializeField]
    private Transform rightFootBone;

    [SerializeField]
    private ParticleSystem specialAttackParticles01;
    [SerializeField]
    private ParticleSystem specialAttackParticles02;
    [SerializeField]
    private Transform rightHandBone;
    [SerializeField]
    private GameObject specialAttack;

    [SerializeField]
    private ParticleSystem vulnerableParticles;
    [SerializeField]
    private BoxCollider vulnerableCollider;


    [SerializeField]
    private Transform waypointSpecialAttack;

    private string lastAttack;

    private int toSpecialAttack;

    private bool specialAttackInProgress;
    private bool destinationReached;

    // Start is called before the first frame update
    protected override void Start()
    {
        health = 3;
        maxHealth = health;
        agent.updatePosition = false;
        specialAttackInProgress = false;
        destinationReached = false;
        toSpecialAttack = 3;
    }

    // Update is called once per frame
    protected override void Update()
    {
        Debug.Log("toSpecialAttack: " + toSpecialAttack);

        //playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        //if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (specialAttackInProgress)
        {
            if (anim.GetBool("Walking"))
                anim.SetFloat("WalkingSpeed", agent.velocity.magnitude);
            SpecialAttack();
        }
        else if (!playerInAttackRange && !alreadyAttacked)
        {
            if (lastAttack != null)
                anim.SetBool(lastAttack, false);
            if (vulnerableCollider.enabled)
                vulnerableCollider.enabled = false;
            ChasePlayer();
        }
        else if (playerInAttackRange)
        {
            anim.SetBool("Walking", false);
            MeleAttack();  
        }
        else
        {
            LookAtPlayer();
        }
    }

    private void MeleAttack()
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
            //Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void SpecialAttack()
    {
        if (agent.destination != waypointSpecialAttack.position && !destinationReached)
        {
            agent.SetDestination(waypointSpecialAttack.position);
        }
    }

    protected override void ChasePlayer()
    {
        base.ChasePlayer();
        anim.SetBool("Walking", true);
        anim.SetFloat("WalkingSpeed", agent.velocity.magnitude);
    }

    protected override void LaunchAttack()
    {
        anim.SetFloat("MeleAttackSpeed", Random.Range(1.5f, 2f));
        if (Random.Range(0, 1f) > 0.5)
        {
            //Debug.Log("Left foot");
            lastAttack = "LeftFootAttack";
            anim.SetBool(lastAttack, true);
        }
        else
        {
            //Debug.Log("Right foot");
            lastAttack = "RightFootAttack";
            anim.SetBool(lastAttack, true);
        }
    }

    public void RightFootCollision(int a)
    {
        if (a == 0)
        {
            rightFootCollider.SetActive(false);
            //meleAttackParticles.SetActive(false);
        }
        else
        {
            rightFootCollider.SetActive(true);
            meleAttackParticles.gameObject.transform.position = rightFootBone.position;
            meleAttackParticles.Play();
        }
    }

    public void LeftFootCollision(int a)
    {
        //Vector3 offset = new Vector3(0, 0, 0.5f);
        if (a == 0)
        {
            leftFootCollider.SetActive(false);
            //meleAttackParticles.SetActive(false);
        }
        else
        {
            leftFootCollider.SetActive(true);
            meleAttackParticles.gameObject.transform.position = leftFootBone.position;
            meleAttackParticles.Play();
        }        
    }

    public void OnMeleAttackFinished()
    {
        if (!playerInAttackRange)
        {
            ResetAttack();
            toSpecialAttack--;
        }
        
        if (toSpecialAttack <= 0)
        {
            specialAttackInProgress = true;
            anim.SetBool(lastAttack, false);
            anim.SetBool("Walking", true);
        }
    }

    public void PrepareMagicAttack()
    {
        specialAttackParticles01.gameObject.transform.position = rightHandBone.position;
        specialAttackParticles01.Play();
        StartCoroutine(PlayParticlesWithDelay(specialAttackParticles01.main.duration, specialAttackParticles02));
    }

    private IEnumerator PlayParticlesWithDelay(float time, ParticleSystem particleSystem)
    {
        yield return new WaitForSeconds(time);
        particleSystem.gameObject.transform.position = rightHandBone.position;
        particleSystem.Play();
        LaunchMagicAttack();

    }

    private void LaunchMagicAttack()
    {
        Instantiate(specialAttack, new Vector3(waypointSpecialAttack.transform.position.x - 6, 5, waypointSpecialAttack.transform.position.z - 6), Quaternion.identity);
    }

    public void OnSpecialAttackFinished()
    {
        // Activate particles gameobject
        vulnerableParticles.gameObject.SetActive(true);

        anim.SetBool("SpecialAttack", false);
        anim.SetBool("Vulnerable", true);
    }

    public void OnVulnerableStart()
    {
        vulnerableCollider.enabled = true;
        vulnerableParticles.Play();
    }

    public void OnVulnerableFinished()
    {
        ResetVulnerableState();
    }

    public void ResetVulnerableState()
    {
        anim.SetBool("Vulnerable", false);
        toSpecialAttack = 3;
        vulnerableCollider.enabled = false;
        vulnerableParticles.gameObject.SetActive(false);
        //vulnerableParticles.gameObject.SetActive(true);
        destinationReached = false;
        specialAttackInProgress = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpecialWaypoint") && toSpecialAttack == 0)
        {
            destinationReached = true;
            anim.SetBool("SpecialAttack", true);
            anim.SetBool("Walking", false);
        }
    }

    public override void TakeDamage(float damageAmount, bool isSwordBlue)
    {
        toInvulnerable();

        Debug.Log("Enemy takes damage");

        health -= damageAmount;
        if (isSwordBlue) 
            healVFX.Play(); // Blue effect
        else
            takeDamageVFX.Play(); // Red effect

        if (health <= 0)
        {
            Debug.Log("Starting dissolving");

            dissolveEnemy.StartDissolveAction();
        }
    }

    public override void ResetEnemy()
    {
        base.ResetEnemy();
        anim.SetBool("SpecialAttack", false);
        anim.SetBool("Walking", false);
        anim.SetBool("Vulnerable", false);
        anim.SetBool("LeftFootAttack", false);
        anim.SetBool("RightFootAttack", false);
        StopAllCoroutines();
        health = maxHealth;
        specialAttackInProgress = false;
        destinationReached = false;
        toSpecialAttack = 3;
        vulnerableCollider.enabled = false;
        vulnerableParticles.gameObject.SetActive(false);
        specialAttackParticles01.Stop();
        specialAttackParticles02.Stop();
    }

    private void OnAnimatorMove()
    {
        Vector3 position = anim.rootPosition;
        position.y = agent.nextPosition.y;
        transform.position = position;
        agent.nextPosition = transform.position;
    }
}
