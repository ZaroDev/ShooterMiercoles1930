using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100f;

    void Start()
    {
        
    }

    public void Damage(float damage)
    {
        health -= damage;
        Debug.Log("Damage: " + damage);
        if(health <= 0)
        {
            KillEnemy(); 
        }
    }
    void KillEnemy()
    {
        //TODO: Poner efectos y cosas guays
        Destroy(gameObject);
    }
}
