using System.Collections;
using UnityEngine;

public class ChargeEnemy : Enemy
{
    private const float INITIAL_FRESNEL_THRESHOLS = -0.2f;

    [SerializeField]
    private new Renderer renderer;

    private MaterialPropertyBlock propBlock;
    private Rigidbody rb;

    private bool coroutineChargingIsActive;

    protected override void Awake()
    {
        base.Awake();
        health = 2;
        maxHealth = health;

        coroutineChargingIsActive = false;

        rb = GetComponent<Rigidbody>();
        propBlock = new MaterialPropertyBlock();
    }

    protected override void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (coroutineChargingIsActive)
        {
            LookAtPlayer();
        }

        if (!playerInAttackRange) Patroling();
        else if (playerInAttackRange && !alreadyAttacked) AttackPlayer();
    }

    protected override void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        LookAtPlayer();

        if (playerIsInFront)
        {
            ///Attack code here
            StartCoroutine(PrepareCharge(2f));
            ///End of attack code 
        }
    }

    IEnumerator PrepareCharge(float time)
    {
        alreadyAttacked = true;
        coroutineChargingIsActive = true;

        float timeQuarter = time / 8f;
        for (int i = 0; i < 8; i ++)
        {
            //Make sure enemy doesn't move
            agent.SetDestination(transform.position);
            propBlock.SetFloat("_FresnelThreshold", propBlock.GetFloat("_FresnelThreshold") - 0.1f);
            renderer.SetPropertyBlock(propBlock);
            yield return new WaitForSeconds(timeQuarter);
        }

        LaunchAttack();
        yield return new WaitForSeconds(0.15f);
        coroutineChargingIsActive = false;
        rb.velocity = Vector3.zero;

        for (int i = 0; i < 8; i++)
        {
            propBlock.SetFloat("_FresnelThreshold", propBlock.GetFloat("_FresnelThreshold") + 0.1f);
            renderer.SetPropertyBlock(propBlock);
            yield return new WaitForSeconds(timeQuarter);
        }

        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }

    protected override void LaunchAttack()
    {
        rb.AddForce(transform.forward * 75f, ForceMode.Impulse);
    }

    public override void ResetEnemy()
    {
        base.ResetEnemy();
        //Debug.Log("Reset charge enemy");
        propBlock.SetFloat("_FresnelThreshold", -0.2f);
        StopCoroutine("PrepareCharge");
        coroutineChargingIsActive = false;
        alreadyAttacked = false;
        coroutineChargingIsActive = false;
    }
}
