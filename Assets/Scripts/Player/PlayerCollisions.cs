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

    void Start()
    {
        playerRb = playerMove.gameObject.GetComponent<Rigidbody>();
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
        if (other.transform.CompareTag("Bullet"))
        {
            Debug.Log("Hit by Bullet");
            Destroy(other.gameObject);
            StartCoroutine(DamageTaken(0.1f));
        }
        else if (other.transform.CompareTag("Enemy"))
        {
            Debug.Log("Collision with an enemy");
            StartCoroutine(DamageTaken(0.25f));
            PushPlayer(other.transform.position, 400f);
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
        StartCoroutine(PlayerCannotMoveForTime(time));

        for (int i = 0; i < 4; i++)
        {
            sphere.SetActive(true);
            yield return new WaitForSeconds(0.05f);
            sphere.SetActive(false);
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator PlayerCannotMoveForTime(float time)
    {
        playerMove.canMove = false;
        yield return new WaitForSeconds(time);
        playerMove.canMove = true;
    }
}
