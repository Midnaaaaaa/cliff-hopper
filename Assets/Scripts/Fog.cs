using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    Transform guia;
    // Start is called before the first frame update
    void Start()
    {
        guia = GameObject.FindGameObjectWithTag("Guide").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = guia.position + Vector3.down * 1;
    }
}
