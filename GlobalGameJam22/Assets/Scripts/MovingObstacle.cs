using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    public float speed = 2;
    public GameObject raycastPointA, raycastPointB;
    private Vector3 thing;
    // Start is called before the first frame update
    void Start()
    {
        float rnd = Random.Range(0f, 1f);
        if (rnd <= 0.5f)
        {
            thing = Vector3.right;
        }
        else
        {
            thing = Vector3.left;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Physics.Raycast(raycastPointA.transform.position, raycastPointA.transform.TransformDirection(Vector3.down), Mathf.Infinity))
        {
            thing = Vector3.right;
        }
        if (!Physics.Raycast(raycastPointB.transform.position, raycastPointB.transform.TransformDirection(Vector3.down), Mathf.Infinity))
        {
            thing = Vector3.left;
        }

        transform.position += Time.deltaTime * speed * thing;
        
    }
}
