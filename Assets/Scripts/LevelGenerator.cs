using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector3 spawnPoint;
    public int minPlatformsUntilTurn;
    public int maxPlatformsUntilTurn;
    public int totalNumPlatforms; //Controlar el tamaño del nivel
    public GameObject platformPrefab;
    private int direction = 1; //1 derecha 0 izquierda
    private Vector3 lastPlatform;
    public GameObject playerPrefab;

    void Start()
    {
        int numPlatform = 0;
        GameObject player = Instantiate(playerPrefab, spawnPoint + Vector3.up, Quaternion.identity);

        //Plataforma inicial
        GameObject platform = Instantiate(platformPrefab); //TODO: añadir script a plataforma
        platform.transform.parent = transform;
        platform.transform.position = spawnPoint;
        lastPlatform = spawnPoint;
        numPlatform++;

        while(totalNumPlatforms > numPlatform)
        {
            int numeroPlataformasSeguidas = Random.Range(minPlatformsUntilTurn, maxPlatformsUntilTurn);
            for (int i = 0; i < numeroPlataformasSeguidas; i++)
            {
                //TODO: RANDOM NUM PARA SABER QUE TIPO DE PLATAFORMA GENERAR


                platform = Instantiate(platformPrefab); //TODO: añadir script a plataforma
                platform.transform.parent = transform;
                platform.transform.position = lastPlatform + new Vector3(1 - direction, 0, direction);
                lastPlatform = lastPlatform + new Vector3(1 - direction, 0, direction);
                numPlatform++;
            }
            direction = 1 - direction;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
