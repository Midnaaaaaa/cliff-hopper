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
                velY = jumpVel;
                jumps--;
                bJumping = true;
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
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Corner")
        {
            inCorner = false;
        }
    }
}
