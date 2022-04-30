using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPickUp : MonoBehaviour
{

    [SerializeField] GameObject swordGroup;
    [SerializeField] Collider playerCollider;

    PlayerSword playerSword;
    // MIRAR EVENTOS UNITY PARA QUE LA FUNCIÓN QUE LLAMA SEA DIFERENTE DEPENDIENDO EL CASO
    private void Start()
    {
        playerSword = swordGroup.GetComponent<PlayerSword>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerSword.TwoSwordsSkill();
        }   
    }
}
