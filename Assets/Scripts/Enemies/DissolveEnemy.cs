using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveEnemy : MonoBehaviour
{
    [SerializeField]
    private GameObject alive;
    [SerializeField]
    private GameObject dissolve;

    private bool canDissolve;
    private Material[] dissolveMaterials;
    public bool isThisEnemyABoss;
    
    void Start()
    {
        canDissolve = false;
        if (alive != null)
        {
            dissolveMaterials = dissolve.GetComponent<MeshRenderer>().materials;
            dissolve.SetActive(false);
            isThisEnemyABoss = false;
        }
        else
        {
            dissolveMaterials = dissolve.GetComponent<SkinnedMeshRenderer>().materials;
            isThisEnemyABoss = true;
        }
    }

    void Update()
    {
        if (canDissolve)
            Dissolve(2f);
    }

    public void StartDissolveAction()
    {
        canDissolve = true;

        if (!isThisEnemyABoss)
        {
            dissolve.transform.position = new Vector3(alive.transform.position.x, 0.5f, alive.transform.position.z);
            dissolve.transform.rotation = alive.transform.GetChild(0).rotation;
 
            alive.SetActive(false);
            dissolve.SetActive(true);
        }   
    }

    private void Dissolve(float speed)
    {
        float value;
        for (int i = 0; i < dissolveMaterials.Length; i++)
        {
            //Debug.Log("Actual value: " + dissolveMaterials[i].GetFloat("_Dissolve"));

            value = Mathf.Lerp(dissolveMaterials[i].GetFloat("_Dissolve"), 1, speed * Time.deltaTime);

            dissolveMaterials[i].SetFloat("_Dissolve", value);

            //Debug.Log("NEW value: " + dissolveMaterials[i].GetFloat("_Dissolve"));

            if (dissolveMaterials[i].GetFloat("_Dissolve") >= 0.9f)
            {
                //Destroy(dissolve);
                if (!isThisEnemyABoss)
                    Destroy(gameObject);
                else
                    Destroy(transform.parent.parent.gameObject);
            }
        }
    }
}
