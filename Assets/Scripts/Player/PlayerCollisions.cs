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
            Debug.Log("Collision with an enemy");
            PushPlayer(collision.contacts[0].point, 15f);
   
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerIsInvulnerable) return;

        if (other.transform.CompareTag("Bullet"))
        {
            Debug.Log("Hit by Bullet");
            Destroy(other.gameObject);
            StartCoroutine(DamageTaken(0.1f));
        }
        else if (other.transform.CompareTag("Enemy"))
        {
            Debug.Log("Collision with an enemy");
            StartCoroutine(DamageTaken(0.4f));
            PushPlayer(other.transform.position, 400f);
        }
        else if (other.transform.CompareTag("Laser"))
        {
            Debug.Log("Hit by Laser");
            StartCoroutine(DamageTaken(0.1f));
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

    IEnumerator DamageTaken(float time)
    {
        playerIsInvulnerable = true;

        StartCoroutine(PlayerCannotMoveForTime(time));

        for (int i = 0; i < 2; i++)
        {
            sphere.SetActive(true);
            yield return new WaitForSeconds(0.05f);
            sphere.SetActive(false);
            yield return new WaitForSeconds(0.05f);
        }
        playerIsInvulnerable = false;
    }

    IEnumerator PlayerCannotMoveForTime(float time)
    {
        playerMove.canMove = false;
        yield return new WaitForSeconds(time);
        playerMove.canMove = true;
    }
}
