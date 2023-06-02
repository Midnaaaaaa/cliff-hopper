using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalizeV2 : MonoBehaviour
{
    public bool x, y, z;
    // Start is called before the first frame update
    void Start()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        Vector3 meshSize = mf.mesh.bounds.size;

        float scaleX = (x) ? 1 / meshSize.x : 1;
        float scaleY = (y) ? 1 / meshSize.y : 1;
        float scaleZ = (z) ? 1 / meshSize.z : 1;

        transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
        transform.Translate(-mf.mesh.bounds.center * 1 / scaleY);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
