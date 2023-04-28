using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update

    private int direction = 1;
    public float jumpVel;
    private float velY;
    public float velHorizontal;
    public float gravity;
    public int maxJumps;
    private int jumps;
    private bool bJumping = false;
    

    void Start()
    {
        jumps = maxJumps;
    }

    void Update()
    {
        if (jumps > 0 && Input.GetButtonDown("Jump"))
        {
            velY = jumpVel;
            jumps--;
            bJumping = true;
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
        bJumping = false;
        jumps = maxJumps;
        velY = 0;
    }


}
