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
    // Start is called before the first frame update
    void Start()
    {
        posInicial = transform.position;
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
        this.vel = vel;
        this.range = range;
        this.direction = direction;
    }
}
