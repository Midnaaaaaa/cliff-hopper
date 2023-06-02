using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinguin : MonoBehaviour
{
    private int direction;
    private int range;
    private float pos;
    private Vector3 posInicial;

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
        if (pos > range)
        {
            float resto = pos - range;
            pos = pos - 2 * resto;
            vel = -vel; //Invertir direccion
        }
        transform.position = posInicial + new Vector3(1 - direction, 0, direction) * pos;
    }
}
