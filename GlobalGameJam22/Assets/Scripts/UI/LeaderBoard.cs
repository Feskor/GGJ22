using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderBoard : MonoBehaviour
{
    public List<TextMeshProUGUI> leaderboardSpots = new List<TextMeshProUGUI>();
    public List<float> scores = new List<float>();

    private void Awake()
    {
        for(int i = 0; i < scores.Count; i++)
        {
            float score = PlayerPrefs.GetFloat("score " + i);
            Debug.Log(score);
            leaderboardSpots[i].text = (i + 1) + ". je moeke " + score;
            scores[i] = score; 
        }
    }


    /// <summary>
    /// This function sets a new highscore on the leaderbord.
    /// Je moeke can later be changed by name if we want player to use their own name.
    /// </summary>
    /// <param name="score"></param>
    public void SetNewScore(float score)
    {
        
        if (score < scores[scores.Count - 1])
            return;

        Debug.Log(score);

        for (int i = 0; i < scores.Count; i++)
        {
            Debug.Log("vrouw");
            if (score > scores[i])
            {            
                Debug.Log(i + "i in enzo");
                scores[i] = score;
                leaderboardSpots[i].text = (i + 1) + "." + " je moeke " + scores[i];
                PlayerPrefs.SetFloat("score " + i, score);
                PlayerPrefs.Save();
                break;
            }
            Debug.Log("man");
        }
    }
}
