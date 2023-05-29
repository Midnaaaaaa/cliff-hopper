using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Bioma
{
    GRASS, DESERT, ICE, FIRE
}

public class Platform : MonoBehaviour
{
    protected Transform terrainTransf;

    private GameObject model;
    private Bioma _bioma;
    public Bioma Bioma { 
        get {
            return _bioma; 
        } 
        set {
            _bioma = value;
            foreach (Transform child in terrainTransf)
            {
                if (child.name != value.ToString())
                {
                    child.gameObject.SetActive(false);
                }
                else
                {
                    child.gameObject.SetActive(true);
                    model = child.gameObject;
                }
            }
        }
    }

    private Trampas _tampa;
    public Trampas Trampa
    {
        get
        {
            return _tampa;
        }
        set
        {
            foreach (Transform child in model.transform)
            {
                if (child.name == value.ToString() || child.name == "default")
                {
                    child.gameObject.SetActive(true);
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        terrainTransf = transform.Find("Terrain");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void SetHeight(float h)
    {
        terrainTransf.localScale = new Vector3(terrainTransf.localScale.x, terrainTransf.localScale.x * h, terrainTransf.localScale.z);
        terrainTransf.Translate(0, -(1 - h)/2, 0);
    }
}
