using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCapsule : MonoBehaviour
{
    [SerializeField]
    private Transform capsule;

    void Update()
    {
        float y = Mathf.PingPong(Time.time * 0.5f, 1);
        //Debug.Log(y);
        capsule.localPosition = new Vector3(0, 1 +  y, 0);
    }
}
