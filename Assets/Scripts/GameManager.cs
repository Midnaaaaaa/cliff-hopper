using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject boulaPrefab;

    [SerializeField]
    private float _velHorizontal;

    private Player player;
    private Guide guide;

    public GameObject fog;

    public float VelHorizontal
    {
        get
        {
            return _velHorizontal;
        }
        set
        {
            player.velHorizontal = value;
            guide.velHorizontal = value;
        }
    }

    public static GameManager Instance { get; private set; }
    public int MonedasCogidas { get; set; }
    //public int Bioma { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            Invoke("SpawnBola", 1.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    void SpawnBola()
    {
        
        GameObject boula = Instantiate(boulaPrefab, LevelGenerator.Instance.spawnPoint + Vector3.up, Quaternion.identity);
        boula.GetComponent<Boula>().SetSpeed(_velHorizontal);
    }

    public void setPlayerAndGuide(Player p, Guide g)
    {
        player = p;
        guide = g;
    }

    public void ChangeFogColor(Color c)
    {
        fog.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", c);
    }
}
