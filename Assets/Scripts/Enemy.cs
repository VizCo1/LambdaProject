using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int health;

    void Update()
    {
        
    }

    bool EnemiesLeft()
    {
        return health <= 0;
    }
}
