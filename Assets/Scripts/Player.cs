using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : Aplastable
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

    private float time;
    public float rotationTime;
    private int initialDirection;

    public Animator animator;

    public UnityEvent OnCornerLit;

    private bool muelto = false;


    void Start()
    {
        jumps = maxJumps;
        rg = GetComponent<Rigidbody>();
        OnCornerLit.AddListener(GameManager.Instance.IncreaseCorners);
        OnCornerLit.AddListener(UIManager.Instance.UpdateCornerText);
        time = 0f;
        initialDirection = direction;

    }

    void Update()
    {
        if (muelto) return;

        transform.localEulerAngles = new Vector3(0, Mathf.LerpAngle(90 * (1 - initialDirection), 90 * (1 - direction), time/rotationTime), 0);

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
        time += Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (muelto) return;

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
            //GetComponent<MeshRenderer>().material.color = Color.blue;
            // transform.localScale.y porque el collider mide 2 veces eso
            transform.position += Vector3.down * (hit.distance - transform.localScale.y);
        }
        // RAYO 1: MIRAR SI ESTAS TOCANDO SUELO
        // Does the ray intersect any objects excluding the player layer
        else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, transform.localScale.y + 0.001f, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.red);
            //Debug.Log("Did Hit");
            //GetComponent<MeshRenderer>().material.color = Color.red;
            // transform.localScale.y porque el collider mide 2 veces eso
            transform.position += Vector3.down * (hit.distance - transform.localScale.y);
            if (bJumping)
                Suelo();

        }
        else
        {
            //Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask);
            //Debug.Log("Did not Hit");
            //GetComponent<MeshRenderer>().material.color = Color.green;
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
        float rand = Random.value;
        if(rand <= 0.5) SoundManager.Instance.SelectAudio(0, 0.5f);
        else SoundManager.Instance.SelectAudio(1, 0.5f);
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

    public void Muelto()
    {
        Debug.Log("Muelto");
        Salto();
        Girar();
    }
    private void Girar()
    {
        initialDirection = direction;
        direction = 1 - direction;
        time = 0f;
        lastCorner.setGlow(true);
        LevelGenerator.Instance.ChangeBioma(lastCorner.Bioma, 1f);
        OnCornerLit?.Invoke();
        SoundManager.Instance.SelectAudio(2, 0.5f);

    }

    public float getJumpVel()
    {
        return jumpVel;
    }

    public void setJumpVel(float jumpVel)
    {
        this.jumpVel = jumpVel;
    }

    public override void Aplastar(float scale)
    {
        base.Aplastar(scale);
        velHorizontal = 0;
        animator.Play("Aplastao");
    }
}
