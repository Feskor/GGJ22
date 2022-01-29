using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    [Range(10f, 50f)]
    private float platformSize;
    public float PlatformSize
    {
        get { return platformSize; }
        set 
        { 
            // Do some kind of check to make sure that the change in size still let's the player continue
            // Connection check OR distance check with old and new platform size
            platformSize = value;
        }
    }

    [Range(1f, 10f)]
    public int gameSpeed;
    private int baseGameSpeed = 10;

    public GameObject level;
    private Queue<GameObject> platformsToRemove = new Queue<GameObject>();
    public Material platformMat;

    private Vector3 prevPlatformCoord = Vector3.zero, currentPlatformCoord = Vector3.zero, nextPlatformCoord = Vector3.zero;

    private const int STARTPLATFORMS = 10;
    private int platformUntilTurnCount = 0;
    private bool zDirection = true;

    private void Awake()
    {
        // spawn STARTPLATFORMS amount of platforms before the game starts
        for (int i = 0; i < STARTPLATFORMS; i++)
        {            
            if (i == 0)
                CreateMesh(currentPlatformCoord, true);
            else
                CreateMesh(currentPlatformCoord);
        }

        // When done start Coroutine
        StartCoroutine(GenerateLevel());
        StartCoroutine(RemovePlatform());
    }

    private IEnumerator GenerateLevel()
    {
        CreateMesh(currentPlatformCoord);

        yield return new WaitForSeconds(baseGameSpeed - gameSpeed);

        StartCoroutine(GenerateLevel());
    }

    public void CreateMesh(Vector3 middlePoint, bool firstPlatform = false)
    {
        Vector3 bottomLeftV = new Vector3(middlePoint.x - platformSize / 2f, middlePoint.y, middlePoint.z - platformSize / 2f);
        Vector3 bottomRightV = new Vector3(middlePoint.x + platformSize / 2f, middlePoint.y, middlePoint.z - platformSize / 2f);
        Vector3 topLeftV = new Vector3(middlePoint.x - platformSize / 2f, middlePoint.y, middlePoint.z + platformSize / 2f);
        Vector3 topRightV = new Vector3(middlePoint.x + platformSize / 2f, middlePoint.y, middlePoint.z + platformSize / 2f);

        Vector3[] vertices = new Vector3[]
        {
            topLeftV,
            topRightV,
            bottomLeftV,
            bottomRightV
        };

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        int[] triangles = new int[]
        {
            0,
            1,
            2,
            2,
            1,
            3
        };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        GameObject platform = new GameObject("Platform " + middlePoint, typeof(MeshFilter), typeof(MeshRenderer));
        platform.transform.position = Vector3.zero;
        platform.transform.localScale = Vector3.one;
        platform.GetComponent<MeshFilter>().mesh = mesh;
        platform.GetComponent<MeshRenderer>().material = platformMat;
        platform.AddComponent<BoxCollider>();
        platform.transform.parent = level.transform;
        platform.GetComponent<MeshFilter>().mesh.RecalculateNormals();

        if (platformUntilTurnCount < STARTPLATFORMS)
            platformUntilTurnCount++;

        if (platformUntilTurnCount >= STARTPLATFORMS)
        {
            // We can turn now
            if (Random.Range(1, 11) == 1) // 1 in 10 chance to turn
            {
                if (zDirection) // Swapping to X direction
                {
                    if (Random.Range(1, 3) == 1) // Left turn (negative)
                    {
                        nextPlatformCoord = new Vector3(middlePoint.x - platformSize, middlePoint.y, middlePoint.z);
                    }
                    else // Right turn (positive)
                    {
                        nextPlatformCoord = new Vector3(middlePoint.x + platformSize, middlePoint.y, middlePoint.z);
                    }
                    zDirection = false;
                }
                else // Swapping to Z direction
                {
                    if (Random.Range(1, 3) == 1) // Left turn (negative)
                    {
                        nextPlatformCoord = new Vector3(middlePoint.x, middlePoint.y, middlePoint.z - platformSize);
                    }
                    else // Right turn (positive)
                    {
                        nextPlatformCoord = new Vector3(middlePoint.x, middlePoint.y, middlePoint.z + platformSize);
                    }
                    zDirection = true;
                }
                platformUntilTurnCount = 0;
            }
            else
                nextPlatformCoord = (currentPlatformCoord + (currentPlatformCoord - prevPlatformCoord));
        }
        else // dont turn yet
        {
            if (firstPlatform)
            {
                prevPlatformCoord = new Vector3(middlePoint.x, middlePoint.y, middlePoint.z - platformSize);
                currentPlatformCoord = Vector3.zero;
                nextPlatformCoord = new Vector3(middlePoint.x, middlePoint.y, middlePoint.z + platformSize);
            }
            else
                nextPlatformCoord = (currentPlatformCoord + (currentPlatformCoord - prevPlatformCoord));
        }

        prevPlatformCoord = currentPlatformCoord;
        currentPlatformCoord = nextPlatformCoord;

        platformsToRemove.Enqueue(platform);
    }

    private IEnumerator RemovePlatform()
    {
        yield return new WaitForSeconds(baseGameSpeed - gameSpeed);

        Destroy(platformsToRemove.Dequeue());

        StartCoroutine(RemovePlatform());
    }
}
