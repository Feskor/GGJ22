using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeTurn : MonoBehaviour
{

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "TurnLeftPlatform")         
            GameManager.Instance.starterAssetsInputs.standardRot = ChangeQuaternion(GameManager.Instance.starterAssetsInputs.standardRot, true,);
        else if (collision.gameObject.tag == "TurnRightPlatform")
            GameManager.Instance.starterAssetsInputs.standardRot = ChangeQuaternion(GameManager.Instance.starterAssetsInputs.standardRot, false,);   
    }

    private float ChangeQuaternion(float standardRot, bool left)
    {
        float newRot = 0;

        if (standardRot == 0 && left || standardRot == 180 && !left)
            newRot = 270;
        else if (standardRot == 180 && left || standardRot == 0 && !left)
            newRot = 90;
        else if (standardRot == 90 && left || standardRot == 270 && !left)
            newRot = 0;
        else if (standardRot == 270 && !left || standardRot == 90 && left)
            newRot = 180;
        
        return newRot;
    }
}



