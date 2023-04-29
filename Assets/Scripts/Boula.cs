using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boula : MonoBehaviour
{
    private int direction = 1;

    [System.NonSerialized]
    public float velHorizontal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        transform.position += new Vector3(1 - direction, 0, direction) * velHorizontal * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Corner")
        {
            direction = 1 - direction;
        }
    }
}
