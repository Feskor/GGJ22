using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScore : MonoBehaviour
{
    public TextMeshProUGUI scoreDisplay;
    public float scoreMultiplayer = 1;
    private float score;
    private float time;
    public LeaderBoard leaderbord;

    //for testing
    public bool dead = false;

    private void Update()
    {
        if (!dead)
        {
            time += Time.deltaTime;
            score = Mathf.Ceil(time) * scoreMultiplayer;
            scoreDisplay.text = "Score: " + score;
        }

        // for testting
        if (dead)
        {
            dead = false;
            leaderbord.SetNewScore(score);
           
        }
    
    }
}
