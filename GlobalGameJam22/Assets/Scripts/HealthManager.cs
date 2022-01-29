using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{

    public GameObject[] hearts;
    public GameObject gameOverScreen;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Heal();
        }
    }

    public void Die()
    {
        //  TO-DO: Play death sound
        gameOverScreen.SetActive(true);
    }

    //  Method to take damage, hand in parameter if more than 1 damage
    public void TakeDamage(int damage = 1)
    {
        if (damage <= 0) damage = 1;

        AudioManager.Instance.HandlePlayerHitSound();

        //  TO-DO: Add invincibility after being hit
        currentLives -= damage;
        hearts[currentLives].SetActive(false);

        if (currentLives <= 0)
        {
            Die();
        }
    }
    
    //  Method to heal the player, hand in parameter if healing more than 1 live
    public void Heal(int amount = 1)
    {
        if (amount <= 0) amount = 1;

        currentLives += amount;

        if(currentLives > maxLives)
        {
            currentLives = maxLives;
        }
        hearts[currentLives - 1].SetActive(true);
    }

    //  Method to increase max lives of the player, hand in parameter if increasing by more than 1
    //public void IncreaseMaxHealth(int amount = 1)
    //{
    //    if (amount <= 0) amount = 1;

    //    maxLives += amount;
    //    currentLives += amount;
        
    //}
}
