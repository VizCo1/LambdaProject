using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MultipleShooterEnemy : Enemy
{
    private bool leftSide;

    [SerializeField]
    protected GameObject projectile;
    [SerializeField]
    private Transform leftShootingTransform;
    [SerializeField]
    private Transform rightShootingTransform;

    protected override void Awake()
    {
        base.Awake();
        health = 4;
        maxHealth = health;

        leftSide = Random.Range(0, 1) > 0.5f ? true : false;
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
        attackAudio.Play();
        Rigidbody rb = Instantiate(projectile, transformPositionToUse, Quaternion.identity).GetComponent<Rigidbody>();
        rb.gameObject.transform.LookAt(player.transform.position);
        rb.AddForce(transform.forward * 8f, ForceMode.Impulse);
        Destroy(rb.gameObject, 3.5f);
        //rb.AddForce(transform.up * 8f, ForceMode.Impulse);
    }
}
