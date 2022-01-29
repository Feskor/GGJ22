using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public int maxLives = 3;
    public int currentLives;

    // Start is called before the first frame update
    void Start()
    {
        currentLives = maxLives;
    }

    // Update is called once per frame
    void Update()
    {
          
    }

    public void Die()
    {
        Debug.Log("I'm dead blegh");
    }

    public void TakeDamage(int damage = 1)
    {
        currentLives -= damage;

        if(currentLives <= 0)
        {
            Die();
        }
    }
    
    public void Heal(int amount = 1)
    {
        currentLives += amount;
        if(currentLives > maxLives)
        {
            currentLives = maxLives;
        }
    }

    public void IncreaseMaxHealth(int amount = 1)
    {
        maxLives += amount;
        currentLives += amount;
    }
}
