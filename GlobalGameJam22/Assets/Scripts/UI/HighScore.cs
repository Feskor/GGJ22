using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScore : MonoBehaviour
{
    public TextMeshProUGUI scoreDisplay;
    public float scoreMultiplayer = 1;
    public float score = 0;
    private float time = 0;
    public LeaderBoard leaderbord;

    private void Update()
    {
            time += Time.deltaTime;
            score = Mathf.Ceil(time) * scoreMultiplayer;
            scoreDisplay.text = "Score: " + score;
    }
}
