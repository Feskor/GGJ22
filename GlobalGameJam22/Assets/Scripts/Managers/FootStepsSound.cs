using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepsSound : MonoBehaviour
{
    private void Step()
    {
        AudioManager.Instance.HandlePlayerFootSteps();
    }
}
