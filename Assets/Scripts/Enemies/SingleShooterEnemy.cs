using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SingleShooterEnemy : Enemy
{
    [SerializeField]
    protected GameObject projectile;
    [SerializeField]
    private Transform shootingTransform;

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

    protected override void LaunchAttack()
    {
        Rigidbody rb = Instantiate(projectile, shootingTransform.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.gameObject.transform.LookAt(player.transform.position);
        rb.AddForce(transform.forward * 8f, ForceMode.Impulse);
        StartCoroutine(DestroyAfterTime(rb, 3.5f));
        //rb.AddForce(transform.up * 8f, ForceMode.Impulse);
    }

    IEnumerator DestroyAfterTime(Rigidbody bulletRb, float time)
    {
        GameObject bullet = bulletRb.gameObject;
        yield return new WaitForSeconds(time);
        Destroy(bullet);
    }
}
