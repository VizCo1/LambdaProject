using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollisions : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;

    [SerializeField]
    private PlayerMove playerMove;

    private Rigidbody playerRb;

    [SerializeField]
    private GameObject sphere;

    [SerializeField]
    private PlayerHealthStats healthStats;

    [SerializeField]
    private AudioSource damageSound;

    private bool playerIsInvulnerable;

    void Start()
    {
        playerRb = playerMove.gameObject.GetComponent<Rigidbody>();
        playerIsInvulnerable = false;
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
            //float damage = other.GetType() == typeof(ChargeEnemy) ? 2f : 1f;
            //Debug.Log("You take: " + damage + " damage");
            StartCoroutine(DamageTaken(0.4f, 0.5f, 0));
            PushPlayer(other.transform.position, 400f);
        }
        else if (other.CompareTag("Laser"))
        {
            //Debug.Log("Hit by Laser");
            StartCoroutine(DamageTaken(0.1f, 0.5f, 0));
        }
        else if (other.CompareTag("GiantFoot"))
        {
            StartCoroutine(DamageTaken(0.4f, 1f, 0));
            PushPlayer(other.transform.position, 650f);
        }
        else if (other.CompareTag("SpecialAttackDamage"))
        {
            StartCoroutine(DamageTaken(0.4f, 2f, 0));
            PushPlayer(other.transform.position, 700f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerIsInvulnerable) return;

        if (other.CompareTag("Laser"))
        {
            //Debug.Log("Move out of the Laser's beam");
            StartCoroutine(DamageTaken(0.1f, 0.25f, 0.25f));
        }
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

        Debug.Log(healthStats.Health);
        if (healthStats.Health <= 0)
        {
            // Do something then break;
            playerIsInvulnerable = true;
            gameController.GameOver();
            yield break;
        }

        damageSound.Play();

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
