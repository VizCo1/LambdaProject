using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LaserEnemy : Enemy
{
    const float LASER_COLLIDER_MAX_HEIGHT = 14.3f;
    const float LASER_COLLIDER_MIN_HEIGHT = 10f;

    [SerializeField]
    private CapsuleCollider laserCapsuleCollider;

    [SerializeField]
    private ParticleSystem reloadingParticles;

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
        laser.Stop();
    }

    protected override void Update()
    {
        Debug.Log("is player in front? " + playerIsInFront);
        if (!reloading)
        {
            base.Update();
        }
        else
        {

        }
    }

    protected override void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        LookAtPlayer();

        if (!alreadyAttacked && playerIsInFront)
        {
            alreadyAttacked = true;

            ///Attack code here
            LaunchAttack();
            StartCoroutine(Reloading());
            ///End of attack code

        }
    }

    protected override void LaunchAttack()
    {
        laser.Play();
        laserCapsuleCollider.enabled = true;
        StartCoroutine(IncreaseLaserColliderHeight());
    }

    IEnumerator Reloading()
    {
        reloading = true;
        yield return new WaitForSeconds(laserDuration);

        reloadingParticles.Play();

        laserCapsuleCollider.enabled = false;
        laserCapsuleCollider.height = LASER_COLLIDER_MIN_HEIGHT;

        yield return new WaitForSeconds(timeBetweenAttacks);

        reloadingParticles.Stop();
        reloading = false;
        ResetAttack();    
    }

    IEnumerator IncreaseLaserColliderHeight()
    {
        while (laserCapsuleCollider.height <= LASER_COLLIDER_MAX_HEIGHT)
        {
            laserCapsuleCollider.height += 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
