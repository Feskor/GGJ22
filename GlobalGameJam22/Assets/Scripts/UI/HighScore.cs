using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScore : MonoBehaviour
{
    public TextMeshProUGUI scoreDisplay;
    public float scoreMultiplayer = 1;
    public float score;
    private float time;
    private int collectables;
    [SerializeField] private int collectValue = 50;
    public LeaderBoard leaderbord;

    //for testing
    public bool dead = false;

    private void Update()
    {
        if (!dead)
        {
            time += Time.deltaTime;
            score = Mathf.Ceil(time) * scoreMultiplayer + (collectables * collectValue);
            scoreDisplay.text = "Score: " + score;
        }

        // for testting
        if (dead)
        {
            dead = false;
            leaderbord.SetNewScore(score);
        }

    }

    public void AddScore()
    {
        collectables++;
    }
}
