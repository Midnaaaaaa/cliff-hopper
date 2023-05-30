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

    Rigidbody rg;

    private Platform lastCorner;

    void Start()
    {
        jumps = maxJumps;
        rg = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Debug.Log(CoordManager.toCHCoords(transform.position));
        if (Input.GetButtonDown("Jump"))
        {
            if (inCorner)
            {
                Girar();
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

        // Hay que mirar antes este porque si estas encima de escalera con bJumping=false se hace snap, y no estas cayendo aun que estes flotando un poco
        // RAYO 2: MIRAR SI ESTAS BAJANDO ESCALERA
        if (!bJumping && Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask) && hit.collider.tag == "Rampa")
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.green);
            GetComponent<MeshRenderer>().material.color = Color.blue;
            // transform.localScale.y porque el collider mide 2 veces eso
            transform.position += Vector3.down * (hit.distance - transform.localScale.y);
        }
        // RAYO 1: MIRAR SI ESTAS TOCANDO SUELO
        // Does the ray intersect any objects excluding the player layer
        else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, transform.localScale.y, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.red);
            //Debug.Log("Did Hit");
            GetComponent<MeshRenderer>().material.color = Color.red;
            // transform.localScale.y porque el collider mide 2 veces eso
            transform.position += Vector3.down * (hit.distance - transform.localScale.y);
            if (bJumping)
                Suelo();

        }
        else
        {
            //Debug.Log("Did not Hit");
            GetComponent<MeshRenderer>().material.color = Color.green;
            if (!bJumping)
                Caer();
        }


        //// RAYO 3: MIRAR SI CHOCAS (TRUCAZO CHAVAL) DE FRENTE
        //Debug.DrawRay(transform.position, transform.forward * 0.5f, Color.cyan);
        //if (Physics.Raycast(transform.position, transform.forward, out hit, 0.5f, layerMask))
        //{
        //    if (hit.collider.tag == "Rampa")
        //    {
        //        GetComponent<MeshRenderer>().material.color = Color.blue;
        //        transform.position += Vector3.down * (hit.distance - transform.localScale.y);
        //    }
        //}

        if (direction == 0) // direccion +X
        {
            int target = Mathf.RoundToInt(transform.position.z);
            if (transform.position.z < target)
            {
                float newZPos = Mathf.Min(transform.position.z + correctionVel, target);
                transform.position = new Vector3(transform.position.x, transform.position.y, newZPos);
            }
            else if (transform.position.z > target)
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
            //Debug.Log(other.gameObject.transform.parent.name);
            lastCorner = other.gameObject.transform.parent.GetComponent<Platform>();
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
            lastCorner = other.GetComponent<Platform>();
        }
    }

    private void Salto()
    {
        velY = jumpVel;
        //rg.velocity = new Vector3(0, jumpVel, 0);
        jumps--;
        bJumping = true;
        inCorner = false;
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
        Girar();
    }
    private void Girar()
    {
        direction = 1 - direction;
        transform.localEulerAngles = new Vector3(0, 90*(1-direction), 0);
        lastCorner.setGlow(true);
    }
}
