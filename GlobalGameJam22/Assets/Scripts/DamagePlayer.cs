using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{

    public int damageToTake = 1;

    [SerializeField] [Range(1, 10)] int cooldown = 2;
    private bool invulnerable = false;
    private GameObject gameManager;
    public bool falling = false;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("doe iets");
        if (falling)
        {
            StartCoroutine(DamageTrigger());
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.layer == 6 && !invulnerable || hit.gameObject.layer == 7 && !invulnerable)
        {
            invulnerable = !invulnerable;
            StartCoroutine(DamageTrigger());
        }
    }

    IEnumerator DamageTrigger()
    {
        gameManager.GetComponentInChildren<HealthManager>().TakeDamage(damageToTake);
        yield return new WaitForSeconds(cooldown);
        invulnerable = !invulnerable;
    }
}
