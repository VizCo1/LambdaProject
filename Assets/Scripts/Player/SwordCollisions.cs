using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollisions : MonoBehaviour
{
    private static float damage;
    public bool isColorBlue;

    private static PlayerInputActions playerInputActions;
    private void Awake()
    {

        playerInputActions = new PlayerInputActions();
        playerInputActions.SwordCollisions.Enable();


        playerInputActions.SwordCollisions.ColorIsRed.performed += ctx =>  { isColorBlue = false; };

        playerInputActions.SwordCollisions.ColorIsBlue.performed += ctx => { isColorBlue = true;  };
    }

    private void Start()
    {
        damage = 1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy.isEnemyInvulnerable()) return;

            if (enemy.isEnemyBlue)
            {
                if (!isColorBlue)
                    enemy.TakeDamage(damage, false);
                else
                    enemy.Heal(damage);
            }
            else if (!enemy.isEnemyBlue)
            {
                if (isColorBlue)
                    enemy.TakeDamage(damage, false);
                else
                    enemy.Heal(damage);
            }      
        }
        else if (other.CompareTag("GiantVulnerable"))
        {
            GiantBoss giantBoss = other.transform.parent.GetComponent<GiantBoss>();

            if (giantBoss.isEnemyInvulnerable()) return;

            Debug.Log("You damaged the boss");
            giantBoss.ResetVulnerableState();
            giantBoss.TakeDamage(damage, isColorBlue);

            giantBoss.PlayDamageSound();
        }
    }
}
