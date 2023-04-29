using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private Transform playerTransform;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Guide").transform;
        offset = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        float xz = (playerTransform.position.x + playerTransform.position.z) / 2 + 12;
        transform.position = new Vector3(xz, playerTransform.position.y + 6, xz) + offset;
    }
}
