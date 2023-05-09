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

    private int direction = 1; //1 derecha 0 izquierda
    private Vector3 lastPlatform;

    public float velHorizontal;

    public GameObject playerPrefab;
    public GameObject platformPrefab;
    //public GameObject platform075Prefab;
    public GameObject cornerPrefab;
    public GameObject guidePrefab;
    public GameObject pinxoPrefab;

    public float trapDensity = 0.5f;
    public int trapMaxLength = 3;


    void Awake()
    {
        int numPlatform = 0;
        GameObject player = Instantiate(playerPrefab, spawnPoint + Vector3.up, Quaternion.identity);
        player.GetComponent<Player>().velHorizontal = velHorizontal;

        GameObject guide = Instantiate(guidePrefab, spawnPoint + Vector3.up, Quaternion.identity);
        guide.GetComponent<Boula>().velHorizontal = velHorizontal;

        //Plataforma inicial
        GameObject platform = Instantiate(platformPrefab); //TODO: añadir script a plataforma
        platform.transform.parent = transform;
        platform.transform.position = spawnPoint;
        lastPlatform = spawnPoint;
        numPlatform++;


        /**
         *  - Mas prob a filas mas largas
         *  - Prob separadas para cada obstaculo
         *  
         * 
         * */
        int numFila = 0;
        while(totalNumPlatforms > numPlatform)
        {
            int numeroPlataformasSeguidas = Random.Range(minPlatformsUntilTurn, maxPlatformsUntilTurn);

            int numObstaclesRestants = 0;
            int obstacle = -1;

            for (int i = 0; i < numeroPlataformasSeguidas; i++)
            {
                //TODO: RANDOM NUM PARA SABER QUE TIPO DE PLATAFORMA GENERAR
                if (numFila > 0 && i > 0 && numObstaclesRestants == 0 && Random.Range(0f,1f) < trapDensity)
                {
                    numObstaclesRestants = Random.Range(1, trapMaxLength);
                    obstacle = Random.Range(0, 1);
                }

                if (obstacle == 0 && i < numeroPlataformasSeguidas - 1)
                {// Pinxo
                    platform = Instantiate(platformPrefab, lastPlatform + new Vector3(1 - direction, 0, direction), Quaternion.identity, transform); //TODO: añadir script a plataforma
                    platform.GetComponent<Platform>().setHeight(0.75f);
                }
                else if (obstacle != 1 || i == numeroPlataformasSeguidas - 1) // no es Hueco
                    platform = Instantiate(platformPrefab, lastPlatform + new Vector3(1 - direction, 0, direction), Quaternion.identity, transform); //TODO: añadir script a plataforma

                lastPlatform = lastPlatform + new Vector3(1 - direction, 0, direction);
                numPlatform++;


                if (i == numeroPlataformasSeguidas - 1)
                {
                    Instantiate(cornerPrefab, lastPlatform + Vector3.up, Quaternion.identity, platform.transform);
                    numObstaclesRestants = 0;
                }
                else if (numObstaclesRestants > 0 && obstacle == 0)
                {
                    Instantiate(pinxoPrefab, lastPlatform, Quaternion.identity, platform.transform);
                    if (--numObstaclesRestants == 0) obstacle = -1;
                }
            }
            direction = 1 - direction;
            ++numFila;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
