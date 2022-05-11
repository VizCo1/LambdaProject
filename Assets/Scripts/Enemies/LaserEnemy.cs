using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LaserEnemy : Enemy
{
    [SerializeField]
    private GameObject laserCollider;

    [SerializeField]
    private VisualEffect laser;

    private bool reloading;

    private float laserDuration;
    protected override void Awake()
    {
        base.Awake();
        base.health = 100;
        reloading = false;
        laserDuration = laser.GetFloat("Duration");
    }

    protected override void Update()
    {
        Debug.Log("Reloading: " + reloading);
        Debug.Log("AlreadyAttacked: " + alreadyAttacked);

        if (!reloading)
        {
            base.Update();
        }
        else
        {
            //agent.SetDestination(transform.position);
            //LookAtPlayer();
        }
    }

    protected override void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        LookAtPlayer();

        if (!alreadyAttacked)
        {
            ///Attack code here
            LaunchAttack();
            StartCoroutine(Reloading());
            ///End of attack code

            alreadyAttacked = true;
        }
    }

    protected override void LaunchAttack()
    {
        laser.Play();
        laserCollider.SetActive(true);
    }

    IEnumerator Reloading()
    {
        reloading = true;
        yield return new WaitForSeconds(laserDuration);

        laserCollider.SetActive(false);
        reloading = false;

        yield return new WaitForSeconds(timeBetweenAttacks);

        ResetAttack();    
    }
}
