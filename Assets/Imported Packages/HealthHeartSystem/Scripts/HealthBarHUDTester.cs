/*
 *  Author: ariel oliveira [o.arielg@gmail.com]
 */

using UnityEngine;

public class HealthBarHUDTester : MonoBehaviour
{
    public void AddHealth()
    {
        PlayerHealthStats.Instance.AddHealth();
    }

    public void Heal(float health)
    {
        PlayerHealthStats.Instance.Heal(health);
    }

    public void Hurt(float dmg)
    {
        PlayerHealthStats.Instance.TakeDamage(dmg);
    }
}
