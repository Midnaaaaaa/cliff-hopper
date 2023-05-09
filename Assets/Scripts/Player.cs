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
        if (other.tag == "Platform")
        {
            bJumping = false;
            jumps = maxJumps;
            velY = 0;
        }

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

    private void Muelto()
    {
        Debug.Log("Muelto");
        Salto();
        direction = 1 - direction;
    }
}
