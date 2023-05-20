using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform playerTransform;

    private Vector3 offset;

    private Camera c;

    private float padding = 1;

    // Start is called before the first frame update
    void Start()
    {
        c = GetComponent<Camera>();
        playerTransform = GameObject.FindGameObjectWithTag("Guide").transform;
        offset = Vector3.zero;
        setZoom(8);
    }

    // Update is called once per frame
    void Update()
    {
        float xz = (playerTransform.position.x + playerTransform.position.z) / 2 + 12;
        transform.position = new Vector3(xz, playerTransform.position.y + 6, xz) + offset;
    }

    private readonly static float k = Mathf.Cos(Mathf.PI / 4) * 16 / (9 * 2);
    public void setZoom(float numBloques)
    {
        c.orthographicSize = (numBloques + padding * 2) * k;
    }
}
