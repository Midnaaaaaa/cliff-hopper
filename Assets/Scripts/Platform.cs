using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    Transform terrainTransf;
    // Start is called before the first frame update
    void Awake()
    {
        terrainTransf = transform.Find("Terrain");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setHeight(float h)
    {
        terrainTransf.localScale = new Vector3(terrainTransf.localScale.x, terrainTransf.localScale.x * h, terrainTransf.localScale.z);
        terrainTransf.Translate(0, -(1 - h)/2, 0);
    }
}
