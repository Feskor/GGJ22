using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        //set the static instance
        if (instance == null) { instance = this; }
        else { Destroy(this); }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public LevelGenerator levelGenerator;
    public StarterAssetsInputs starterAssetsInputs;
    public UIManager UIManager;
}
