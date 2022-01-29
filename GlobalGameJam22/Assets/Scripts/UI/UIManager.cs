using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public HighScore score;
    public LeaderBoard leaderBoard;
    public HealthManager healthManager;

    private void Start()
    {
        SetActive(score.gameObject);
        Deactivate(leaderBoard.gameObject);
        Deactivate(healthManager.gameOverScreen);

        for(int i = 0; i < healthManager.hearts.Length; i++)
        {
            SetActive(healthManager.hearts[i]);
        }
    }

    public void SetActive(GameObject UIElement)
    {
        UIElement.SetActive(true);
    }

    public void Deactivate(GameObject UIElement)
    {
        UIElement.SetActive(false);
    }
}
