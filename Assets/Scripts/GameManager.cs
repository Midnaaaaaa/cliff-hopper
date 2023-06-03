using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public GameObject boulaPrefab;

    public float velHorizontal;

    private Player player;
    private Guide guide;

    public GameObject fog;

    public int Coins { get; private set; } = 0;

    public int Corners { get; private set; } = 0;

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
        boula.GetComponent<Boula>().SetSpeed(velHorizontal*0.95f);
    }

    public void setPlayerAndGuide(Player p, Guide g)
    {
        player = p;
        guide = g;
    }

    public void ChangeFogColor(Color c, float transitionTime)
    {
        fog.GetComponent<Fog>().ChangeFogColor(c, transitionTime);
    }

    public void IncreaseCoins(){
        ++Coins;
    }


    public void IncreaseCorners()
    {
        ++Corners;
    }
}
