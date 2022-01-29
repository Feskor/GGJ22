using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeTurn : MonoBehaviour
{
        

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "TurnLeftPlatform")
        {
            //GameManager.Instance.starterAssetsInputs.standardRot -= 90;
            Debug.Log("hello?");
        }
        else if (collision.gameObject.tag == "TurnRightPlatform")
        {
            //GameManager.Instance.starterAssetsInputs.standardRot += 90;
            Debug.Log("hello? right");
        }
    }
}



