using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private HealthManager hm;
    private HighScore hs;

    [SerializeField] private int bonusScore = 200;
    void Start()
    {
        hm = GameObject.FindObjectOfType<HealthManager>();
    }

    public void Activate(int pickUp)
    {
        switch (pickUp)
        {
            case 0:// Collectables
                hs.AddScore(bonusScore);
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
