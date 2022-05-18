using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantSpecialAttack : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particles;
    [SerializeField]
    private BoxCollider damagesPlayer;
    private float scalingSpeed;

    private int numberOfCollisions;

    void Start()
    {
        numberOfCollisions = 0;
        scalingSpeed = 20f;
        StartCoroutine(ScaleBalls());
    }

    IEnumerator ScaleBalls()
    {
        float scale;
        while(true)
        {
            for (int i = 0; i < transform.childCount - 1; i++)
            {
                Transform ball = transform.GetChild(i);
                scale = Mathf.Lerp(ball.localScale.x, 1, Time.deltaTime * scalingSpeed);
                ball.localScale = new Vector3(scale, scale, scale);
            }
            if (transform.GetChild(0).localScale.magnitude >= new Vector3(0.98f, 0.98f, 0.98f).magnitude) break;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpecialAttack"))
        {
            numberOfCollisions++;

            //Debug.Log("Collision with bombs");
            ParticleSystem particleSystem = Instantiate(particles, new Vector3 (other.transform.position.x, 0.3f, other.transform.position.z), Quaternion.identity);
            particleSystem.Play();
 
            StartCoroutine(DestroyImmediateAfterTime(particleSystem.gameObject, 1f));

            if (numberOfCollisions == 16)
            {
                Debug.Log("Destroying the balls");
                damagesPlayer.enabled = true;
                Destroy(gameObject, 0.25f);
            }
        }
    }

    IEnumerator DestroyImmediateAfterTime(GameObject gameObject, float time)
    {
        yield return new WaitForSeconds(time);
        DestroyImmediate(gameObject, true);
    }
}
