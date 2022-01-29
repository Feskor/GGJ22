using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapSystem : MonoBehaviour
{
    [SerializeField] private KeyCode swapKey = KeyCode.E;
    [SerializeField] [Range(0.5f, 2f)] float swapDelay = 1f;
    private bool canSwap = true;

    [SerializeField] private Material transparent;
    [SerializeField] [Range(0.1f, 1f)] private float alpha = 0.3f;

    [SerializeField] private List<GameObject> envOfColorX;
    [SerializeField] private Color colorX = Color.red;
    private int layerColorX = 6;

    [SerializeField] private List<GameObject> envOfColorY;
    [SerializeField] private Color colorY = Color.green;
    private int layerColorY = 7;

    [SerializeField] GameObject player;
    private Renderer playerRen;


    // Start is called before the first frame update
    void Start()
    {
        playerRen = player.GetComponent<SkinnedMeshRenderer>();

        // Picks random color side to start game
        int sidePicker = Random.Range(0, 2);

        if (sidePicker == 0)
        {
            playerRen.material.color = colorX;
            player.layer = layerColorX;
        }
        else
        {
            playerRen.material.color = colorY;
            player.layer = layerColorY;
        }

        SetEnvProperties(envOfColorX, layerColorX, colorX);
        SetEnvProperties(envOfColorY, layerColorY, colorY);
        SwapEnv(playerRen.material.color);
    }

    // Set properties of objects in environment list
    void SetEnvProperties(List<GameObject> envList, int envLayer, Color envColor)
    {
        foreach (GameObject item in envList)
        {
            foreach (Transform child in item.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = envLayer;
            }

            foreach (MeshRenderer child in item.GetComponentsInChildren<MeshRenderer>())
            {
                child.material = transparent;

                if (playerRen.material.color == envColor)
                {
                    child.material.color = envColor;
                }
                else
                {
                    Color lowAlphaColor = envColor;
                    lowAlphaColor.a = alpha;
                    child.material.color = lowAlphaColor;
                }
            }
        }
    }

    // Swamps colors and alpha
    void SwapEnv(Color oldColor)
    {
        List<GameObject> activeEnvList, deactiveEnvList;
        int currentlayer;
        Color activeEnvColor, deactiveEnvColor;

        if (oldColor == colorX)
        {
            activeEnvList = envOfColorY;
            activeEnvColor = colorY;
            currentlayer = layerColorY;

            deactiveEnvColor = colorX;
            deactiveEnvList = envOfColorX;
        }
        else
        {
            activeEnvList = envOfColorX;
            activeEnvColor = colorX;
            currentlayer = layerColorX;

            deactiveEnvColor = colorY;
            deactiveEnvList = envOfColorY;
        }

        playerRen.material.color = activeEnvColor;
        player.layer = currentlayer;

        // Reset alpha
        foreach (GameObject item in activeEnvList)
        {
            foreach (MeshRenderer child in item.GetComponentsInChildren<MeshRenderer>())
            {
                child.material.color = activeEnvColor;
            }
        }

        Color lowAlphaColor = deactiveEnvColor;
        lowAlphaColor.a = alpha;

        // Reduce alpha
        foreach (GameObject item in deactiveEnvList)
        {
            foreach (MeshRenderer child in item.GetComponentsInChildren<MeshRenderer>())
            {
                child.material.color = lowAlphaColor;
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canSwap == true)
        {
            canSwap = !canSwap;
            StartCoroutine(Swap());
        }
    }

    IEnumerator Swap()
    {
        SwapEnv(playerRen.material.color);
        yield return new WaitForSeconds(swapDelay);
        canSwap = !canSwap;
    }
}
