using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MultipleShooterEnemy : Enemy
{

    private bool leftSide;

    private Transform leftShootingTransform;
    [SerializeField]
    private Transform rightShootingTransform;

    protected override void Awake()
    {
        base.Awake();
        base.health = 100;

        leftSide = Random.Range(0, 1) > 0.5f ? true : false;
        leftShootingTransform = base.shootingTransform;
    }

    protected override void LaunchAttack()
    {
        Vector3 transformPositionToUse;

        if (leftSide)
        {
            transformPositionToUse = leftShootingTransform.position;
            leftSide = false;
        }
        else
        {
            transformPositionToUse = rightShootingTransform.position;
            leftSide = true;
        }

        Rigidbody rb = Instantiate(base.projectile, transformPositionToUse, Quaternion.identity).GetComponent<Rigidbody>();
        rb.gameObject.transform.LookAt(player.transform.position);
        rb.AddForce(transform.forward * 8f, ForceMode.Impulse);
        StartCoroutine(DestroyAfterTime(rb, 5f));
        //rb.AddForce(transform.up * 8f, ForceMode.Impulse);
    }

    IEnumerator DestroyAfterTime(Rigidbody bulletRb, float time)
    {
        GameObject bullet = bulletRb.gameObject;
        yield return new WaitForSeconds(time);
        Destroy(bullet);
    }
}
