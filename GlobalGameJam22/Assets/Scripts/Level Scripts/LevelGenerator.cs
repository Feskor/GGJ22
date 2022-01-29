using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    [Range(10f, 50f)]
    private float platformSize;
    private float prevPlatformSize, platformCorrection;

    private float maxGameSpeed = 10;
    [Range(1f, 10f)]
    public float gameSpeed;    

    public GameObject level;
    private Queue<GameObject> platformsToRemove = new Queue<GameObject>();
    public Material platformMat;
    private GameObject previousPlatform;

    private Vector3 prevPlatformCoord = Vector3.zero, currentPlatformCoord = Vector3.zero, nextPlatformCoord = Vector3.zero;

    private const int STARTPLATFORMS = 10;
    private int platformUntilTurnCount = 0;
    private bool zDirection = true, platformSizeChanged = false, goingNegative = false, turnLeftPlatform = false, turnRightPlatform = false;

    private void Awake()
    {
        prevPlatformSize = platformSize;

        // spawn STARTPLATFORMS amount of platforms before the game starts
        for (int i = 0; i < STARTPLATFORMS; i++)
        {
            CreateMesh(currentPlatformCoord);

            if (i == 0)
                GetNewCoords(currentPlatformCoord, true);
            else
                GetNewCoords(currentPlatformCoord);
        }

        // When done start Coroutine
        StartCoroutine(GenerateLevel());
        StartCoroutine(RemovePlatform());
    }

    private IEnumerator GenerateLevel()
    {
        CreateMesh(currentPlatformCoord);

        yield return new WaitForSeconds(maxGameSpeed - gameSpeed);

        turnLeftPlatform = turnRightPlatform = false;

        GetNewCoords(currentPlatformCoord);

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
        if (zDirection && goingNegative) // moving z-axis to negative
            platform.transform.position = new Vector3(platform.transform.position.x, platform.transform.position.y, platform.transform.position.z + platformCorrection);
        else if (zDirection && !goingNegative)
            platform.transform.position = new Vector3(platform.transform.position.x, platform.transform.position.y, platform.transform.position.z - platformCorrection);
        else if (!zDirection && goingNegative)
            platform.transform.position = new Vector3(platform.transform.position.x + platformCorrection, platform.transform.position.y, platform.transform.position.z);
        else if (!zDirection && !goingNegative)
            platform.transform.position = new Vector3(platform.transform.position.x - platformCorrection, platform.transform.position.y, platform.transform.position.z);

        platform.transform.localScale = Vector3.one;
        platform.GetComponent<MeshFilter>().mesh = mesh;
        platform.GetComponent<MeshRenderer>().material = platformMat;
        platform.AddComponent<BoxCollider>();
        platform.transform.parent = level.transform;
        platform.GetComponent<MeshFilter>().mesh.RecalculateNormals();

        if (turnLeftPlatform)
        {
            GameObject boxColliderObject = new GameObject("TurnLeftColliderObject");
            boxColliderObject.transform.position = prevPlatformCoord;
            boxColliderObject.transform.rotation = Quaternion.Euler(previousPlatform.transform.rotation.x, -45, previousPlatform.transform.rotation.x);
            boxColliderObject.transform.parent = previousPlatform.transform;
            boxColliderObject.AddComponent<BoxCollider>().isTrigger = true;
            boxColliderObject.GetComponent<BoxCollider>().size = new Vector3(SquareRootOfPlatformSize(), 5f, 1f);
            boxColliderObject.tag = "TurnLeftPlatform";
        }
        else if (turnRightPlatform)
        {
            GameObject boxColliderObject = new GameObject("TurnRightColliderObject");
            boxColliderObject.transform.position = prevPlatformCoord;
            boxColliderObject.transform.rotation = Quaternion.Euler(previousPlatform.transform.rotation.x, 45, previousPlatform.transform.rotation.x);
            boxColliderObject.transform.parent = previousPlatform.transform;
            boxColliderObject.AddComponent<BoxCollider>().isTrigger = true;
            boxColliderObject.GetComponent<BoxCollider>().size = new Vector3(SquareRootOfPlatformSize(), 5f, 1f); 
            boxColliderObject.tag = "TurnRightPlatform";
        }

        previousPlatform = platform;

        platformsToRemove.Enqueue(platform);
    }

    private float SquareRootOfPlatformSize()
    {
        return Mathf.Sqrt(platformSize * platformSize + platformSize * platformSize);
    }

    public void GetNewCoords(Vector3 middlePoint, bool firstPlatform = false)
    {
        if (platformSize != prevPlatformSize)
        {
            platformCorrection = (platformSize - 10f) / 2f;

            prevPlatformSize = platformSize;
            platformSizeChanged = true;

            // any value different than the base 10 needs a change
            // (new number - 10 (base number)) / 2 = -z value
        }            

        if (platformUntilTurnCount < STARTPLATFORMS)
            platformUntilTurnCount++;

        if (platformUntilTurnCount >= STARTPLATFORMS)
        {
            // We can turn now
            if (Random.Range(1, 3) == 1 && !platformSizeChanged) // 1 in 6 chance to turn
            {
                if (zDirection) // Swapping to X direction
                {
                    if (Random.Range(1, 3) == 1) // Left turn (negative)
                    {
                        nextPlatformCoord = new Vector3(middlePoint.x - platformSize, middlePoint.y, middlePoint.z);
                        goingNegative = true;
                        turnLeftPlatform = true;
                    }
                    else // Right turn (positive)
                    {
                        nextPlatformCoord = new Vector3(middlePoint.x + platformSize, middlePoint.y, middlePoint.z);
                        goingNegative = false;
                        turnRightPlatform = true;
                    }
                   
                    zDirection = false;
                }
                else // Swapping to Z direction
                { 
                    if (Random.Range(1, 3) == 1) // Left turn (negative)
                    {
                        nextPlatformCoord = new Vector3(middlePoint.x, middlePoint.y, middlePoint.z + platformSize);
                        goingNegative = true;
                        turnLeftPlatform = true;
                    }
                    else // Right turn (positive)
                    {
                        nextPlatformCoord = new Vector3(middlePoint.x, middlePoint.y, middlePoint.z - platformSize);
                        goingNegative = false;
                        turnRightPlatform = true;
                    }
                    
                    zDirection = true;
                }
                platformUntilTurnCount = 0;
            }
            else if (platformSizeChanged)
            {
                if (zDirection && goingNegative) // moving z-axis to negative
                    nextPlatformCoord = new Vector3(middlePoint.x, middlePoint.y, middlePoint.z - platformSize);
                else if (zDirection && !goingNegative)
                    nextPlatformCoord = new Vector3(middlePoint.x, middlePoint.y, middlePoint.z + platformSize);
                else if (!zDirection && goingNegative)
                    nextPlatformCoord = new Vector3(middlePoint.x - platformSize, middlePoint.y, middlePoint.z);
                else if (!zDirection && !goingNegative)
                    nextPlatformCoord = new Vector3(middlePoint.x + platformSize, middlePoint.y, middlePoint.z);

                platformSizeChanged = false;
            }
            else
                nextPlatformCoord = currentPlatformCoord + (currentPlatformCoord - prevPlatformCoord);
        }
        else // dont turn yet
        {
            if (firstPlatform)
            {
                prevPlatformCoord = new Vector3(middlePoint.x, middlePoint.y, middlePoint.z - platformSize);
                currentPlatformCoord = Vector3.zero;
                nextPlatformCoord = new Vector3(middlePoint.x, middlePoint.y, middlePoint.z + platformSize);
            }
            else if (platformSizeChanged)
            {
                if (zDirection && goingNegative) // moving z-axis to negative
                    nextPlatformCoord = new Vector3(middlePoint.x, middlePoint.y, middlePoint.z - platformSize);
                else if (zDirection && !goingNegative)
                    nextPlatformCoord = new Vector3(middlePoint.x, middlePoint.y, middlePoint.z + platformSize);
                else if (!zDirection && goingNegative)
                    nextPlatformCoord = new Vector3(middlePoint.x - platformSize, middlePoint.y, middlePoint.z);
                else if (!zDirection && !goingNegative)
                    nextPlatformCoord = new Vector3(middlePoint.x + platformSize, middlePoint.y, middlePoint.z);

                platformSizeChanged = false;
            }
            else
                nextPlatformCoord = currentPlatformCoord + (currentPlatformCoord - prevPlatformCoord);
        }

        prevPlatformCoord = currentPlatformCoord;
        currentPlatformCoord = nextPlatformCoord;
    }

    private IEnumerator RemovePlatform()
    {
        yield return new WaitForSeconds(maxGameSpeed - gameSpeed);

        Destroy(platformsToRemove.Dequeue());

        StartCoroutine(RemovePlatform());
    }
}
