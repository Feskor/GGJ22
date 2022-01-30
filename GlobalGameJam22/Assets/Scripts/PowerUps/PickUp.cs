using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{

    public enum Pickups { Collectables, Health, Shoot }
    public Pickups pickups;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PowerUp>().Activate((int)pickups);
            Destroy(gameObject);
        }
    }
}
