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

    private int baseGameSpeed = 11;

    public GameObject level;

    public Material platformMat;

    private Vector3 currentPlatformCoord = Vector3.zero;
    private const int STARTPLATFORMS = 10;

    private void Awake()
    {
        // spawn STARTPLATFORMS amount of platforms before the game starts
        for (int i = 0; i < STARTPLATFORMS; i++)
        {
            CreateMesh(currentPlatformCoord);
        }

        // When done start Coroutine
        StartCoroutine(GenerateLevel(gameSpeed));
    }

    private IEnumerator GenerateLevel(int currentGameSpeed)
    {
        CreateMesh(currentPlatformCoord);

        yield return new WaitForSeconds(baseGameSpeed - currentGameSpeed);

        StartCoroutine(GenerateLevel(gameSpeed));
    }

    public void CreateMesh(Vector3 middlePoint)
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

        currentPlatformCoord = new Vector3(middlePoint.x, middlePoint.y, middlePoint.z + platformSize);
    }
}
