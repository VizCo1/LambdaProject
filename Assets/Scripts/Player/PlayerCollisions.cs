using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    [SerializeField]
    PlayerMove playerMove;

    Rigidbody playerRb;

    [SerializeField]
    GameObject sphere;

    [SerializeField]
    PlayerHealthStats healthStats;

    private bool playerIsInvulnerable;

    void Start()
    {
        playerRb = playerMove.gameObject.GetComponent<Rigidbody>();
        playerIsInvulnerable = false;
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            //Debug.Log("Collision with an enemy");
            PushPlayer(collision.contacts[0].point, 15f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerIsInvulnerable) return;

        if (other.CompareTag("Bullet"))
        {
            //Debug.Log("Hit by Bullet");
            Destroy(other.gameObject);
            StartCoroutine(DamageTaken(0.1f, 0.25f, 0));
        }
        else if (other.CompareTag("Enemy"))
        {
            //Debug.Log("Collision with an enemy");
            float damage = other.GetType() == typeof(ChargeEnemy) ? 2f : 1f;
            //Debug.Log("You take: " + damage + " damage");
            StartCoroutine(DamageTaken(0.4f, damage, 0));
            PushPlayer(other.transform.position, 400f);
        }
        else if (other.CompareTag("Laser"))
        {
            //Debug.Log("Hit by Laser");
            StartCoroutine(DamageTaken(0.1f, 2f, 0));
        }
        else if (other.CompareTag("GiantFoot"))
        {
            StartCoroutine(DamageTaken(0.4f, 2f, 0));
            PushPlayer(other.transform.position, 650f);
        }
        else if (other.CompareTag("SpecialAttackDamage"))
        {
            StartCoroutine(DamageTaken(0.4f, 2.5f, 0));
            PushPlayer(other.transform.position, 700f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerIsInvulnerable) return;

        if (other.CompareTag("Laser"))
        {
            //Debug.Log("Move out of the Laser's beam");
            StartCoroutine(DamageTaken(0.1f, 2f, 0.25f));
        }
    }

    void PlayerHit()
    {
        //StartCoroutine(DamageTaken());
    }

    void PushPlayer(Vector3 collisionPoint, float force)
    {
        // playerRb.gameObject.transform.position
        Vector3 dir = collisionPoint - playerRb.gameObject.transform.position;
        dir = -dir.normalized;
        playerRb.AddForce(dir * force);
    }

    IEnumerator DamageTaken(float time, float damage, float extraTime)
    {
        playerIsInvulnerable = true;

        healthStats.TakeDamage(damage);

        StartCoroutine(PlayerCannotMoveForTime(time));

        for (int i = 0; i < 2; i++)
        {
            sphere.SetActive(true);
            yield return new WaitForSeconds(0.05f);
            sphere.SetActive(false);
            yield return new WaitForSeconds(0.05f);
        }
        if (extraTime != 0)
            yield return new WaitForSeconds(extraTime);
        playerIsInvulnerable = false;
    }

    IEnumerator PlayerCannotMoveForTime(float time)
    {
        playerMove.canMove = false;
        yield return new WaitForSeconds(time);
        playerMove.canMove = true;
    }
}
