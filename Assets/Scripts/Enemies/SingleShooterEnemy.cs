using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SingleShooterEnemy : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        base.health = 100;
    }

    protected override void LaunchAttack()
    {
        Rigidbody rb = Instantiate(base.projectile, base.shootingTransform.position, Quaternion.identity).GetComponent<Rigidbody>();
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
