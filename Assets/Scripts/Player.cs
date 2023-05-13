using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update

    private int direction = 1;
    public float jumpVel;
    private float velY;

    [System.NonSerialized]
    public float velHorizontal;

    public float gravity;
    public int maxJumps;
    private int jumps;
    private bool bJumping = false;

    private bool inCorner = false;

    private float correctionVel = 0.1f;

    void Start()
    {
        jumps = maxJumps;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (inCorner)
            {
                direction = 1 - direction;
                inCorner = false;
            }
            else if (jumps > 0)
            {
                Salto();
            }
        }
    }

    void FixedUpdate()
    {
        if (bJumping)
        {
            velY += gravity * Time.fixedDeltaTime;
            transform.position += new Vector3(0, velY, 0) * Time.fixedDeltaTime;
        }
        transform.position += new Vector3(1 - direction, 0, direction) * velHorizontal * Time.fixedDeltaTime;


        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 2;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, transform.localScale.y, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.red);
            Debug.Log("Did Hit");
            GetComponent<MeshRenderer>().material.color = Color.red;
            if (bJumping)
                Suelo();

        }
        else
        {
            Debug.Log("Did not Hit");
            GetComponent<MeshRenderer>().material.color = Color.green;
            if (!bJumping)
                Caer();
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
        //if (other.tag == "Platform")
        //{
        //    Suelo();
        //}

        if (other.tag == "Corner")
        {
            inCorner = true;
        }

        if (other.tag == "Trap")
        {
            Muelto();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Corner" && inCorner)
        {
            inCorner = false;
            bJumping = true;
        }
    }

    private void Salto()
    {
        velY = jumpVel;
        jumps--;
        bJumping = true;
    }

    private void Suelo()
    {
        bJumping = false;
        jumps = maxJumps;
        velY = 0;
    }

    private void Caer()
    {
        inCorner = false;
        bJumping = true;
    }

    private void Muelto()
    {
        Debug.Log("Muelto");
        Salto();
        direction = 1 - direction;
    }
}
