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
        health = 3;
        maxHealth = health;
    }

    protected override void LaunchAttack()
    {
        attackAudio.Play();
        Rigidbody rb = Instantiate(projectile, shootingTransform.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.gameObject.transform.LookAt(player.transform.position);
        rb.AddForce(transform.forward * 8f, ForceMode.Impulse);
        Destroy(rb.gameObject, 3.5f);
        //rb.AddForce(transform.up * 8f, ForceMode.Impulse);
    }
}
