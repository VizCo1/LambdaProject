using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantSpecialAttack : MonoBehaviour
{
    [SerializeField]
    private GameObject particles;
    private float scalingSpeed;

    void Start()
    {
        scalingSpeed = 10f;
        StartCoroutine(ScaleBalls());
        StartCoroutine(DestroyAfterTime(this.gameObject, 8f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ScaleBalls()
    {
        float scale;

        while(true)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform ball = transform.GetChild(i);
                scale = Mathf.Lerp(ball.localScale.x, 1, Time.deltaTime * scalingSpeed);
                ball.localScale = new Vector3(scale, scale, scale);
            }
            if (transform.GetChild(0).localScale.magnitude >= new Vector3(0.98f, 0.98f, 0.98f).magnitude) break;
        }

        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpecialAttack"))
        {
            GameObject gameObject = Instantiate(particles, new Vector3 (other.transform.position.x, 0.3f, other.transform.position.z), Quaternion.identity);
            StartCoroutine(DestroyAfterTime(gameObject, 1.7f));
        }
    }

    IEnumerator DestroyAfterTime(GameObject particles, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(particles);
    }
}
