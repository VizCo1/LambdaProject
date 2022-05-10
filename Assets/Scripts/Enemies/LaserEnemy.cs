using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LaserEnemy : Enemy
{
    [SerializeField]
    private CapsuleCollider laserCollider;

    [SerializeField]
    private VisualEffect laser;

    protected override void Awake()
    {
        base.Awake();
        base.health = 100;
    }

    protected override void LookAtPlayer()
    {
        base.LookAtPlayer(); // this is --> transform.LookAt(player);
        //if (!trailsRotation.activeSelf) trailsRotation.SetActive(true);

    }

    protected override void ChasePlayer()
    {
        base.ChasePlayer();
        //if (trailsRotation.activeSelf) trailsRotation.SetActive(false);
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
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    protected override void LaunchAttack()
    {
        
    }

    IEnumerator DestroyAfterTime(Rigidbody bulletRb, float time)
    {
        GameObject bullet = bulletRb.gameObject;
        yield return new WaitForSeconds(time);
        Destroy(bullet);
    }
}
