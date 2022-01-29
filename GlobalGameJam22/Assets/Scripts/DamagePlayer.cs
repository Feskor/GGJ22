using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{

    public int damageToGive;
    bool invincible = false;
    public GameObject gameController;

    private void Start()
    {
        gameController = GameObject.Find("GameController");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !invincible)
        {
            gameController.GetComponent<HealthManager>().TakeDamage(damageToGive);
            invincible = true;
            Invoke("setBool", 0.3f);
        }
    }

    void setBool()
    {
        invincible = false;
    }

}
