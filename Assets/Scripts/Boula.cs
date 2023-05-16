using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boula : MonoBehaviour
{
    private int direction = 1;

    [System.NonSerialized]
    public float velHorizontal;

    private float correctionVel = 0.1f;

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

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            if (hit.collider.tag == "Rampa" && hit.distance)
            {
                Debug.Log("Rayo vallecano: " + hit.distance);
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.green);
                transform.position += Vector3.down * (hit.distance - transform.localScale.y);
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.red);
            }
        }

        if (direction == 0) // direccion +X
        {
            int target = Mathf.RoundToInt(transform.position.z);
            if (transform.position.z < target)
            {
                float newZPos = Mathf.Min(transform.position.z + correctionVel, target);
                transform.position = new Vector3(transform.position.x, transform.position.y, newZPos);
            }
            else if (transform.position.x > target)
            {
                float newZPos = Mathf.Max(transform.position.z - correctionVel, target);
                transform.position = new Vector3(transform.position.x, transform.position.y, newZPos);
            }
        }
        else // direccion +Z
        {
            int target = Mathf.RoundToInt(transform.position.x);
            if (transform.position.x < target)
            {
                float newXPos = Mathf.Min(transform.position.x + correctionVel, target);
                transform.position = new Vector3(newXPos, transform.position.y, transform.position.z);
            }
            else if (transform.position.x > target)
            {
                float newXPos = Mathf.Max(transform.position.x - correctionVel, target);
                transform.position = new Vector3(newXPos, transform.position.y, transform.position.z);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Corner")
        {
            direction = 1 - direction;
        }
    }
}
