using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private HealthManager hm;
    private HighScore hs;
    
    void Start()
    {
        hm = GameObject.FindObjectOfType<HealthManager>();
        hs = GameObject.FindObjectOfType<HighScore>();
    }

    public void Activate(int pickUp)
    {
        switch (pickUp)
        {
            case 0:// Collectables
                hs.AddScore();
                break;
            case 1:// Health
                hm.Heal();
                break;
            case 2: // Shoot
                // TODO
                Debug.Log("Shoot!");
                break;
            default:
                Debug.Log("Something went wrong when deciding to give a powerup!");
                break;
        }
    }
}
