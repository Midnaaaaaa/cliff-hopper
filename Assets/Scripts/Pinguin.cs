using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinguin : MonoBehaviour
{
    private int direction;
    private float pos = 0;
    private Vector3 posInicial;

    public int range = 5;
    public float vel;
    public float scale;

    private float lastVel;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale *= scale;
        transform.Translate(Vector3.down * (1-scale) / 2);
        posInicial = transform.position;

        lastVel = vel;
    }

    // Update is called once per frame
    void Update()
    {
        pos += vel * Time.deltaTime;
        if (pos > range || pos < 0)
        {
            float resto = (pos < 0) ? pos : pos - range;
            pos = pos - 2 * resto;
            vel = -vel; //Invertir direccion

            transform.Rotate(new Vector3(0, 180, 0));
        }
        transform.position = posInicial + new Vector3(1 - direction, 0, direction) * pos;
    }

    public void Init(float vel, int range, int direction)
    {
        float var = vel * 0.25f;
        this.vel = vel + Random.Range(-var, var);
        this.range = range;
        this.direction = direction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (vel > 0) lastVel = vel;
        vel = 0;
    }

    private void OnCollisionExit(Collision collision)
    {
        vel = lastVel;
    }


}
