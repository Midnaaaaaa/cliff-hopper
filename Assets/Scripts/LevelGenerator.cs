using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Trampas
{
    PINXO, HUECO
}

public class LevelGenerator : MonoBehaviour
{
    // Start is called before the first frame update

    public static LevelGenerator Instance { get; private set; }

    private List<Vector2> cornersPos;

    public Vector3 spawnPoint;
    public int minPlatformsUntilTurn;
    public int maxPlatformsUntilTurn;
    public int totalNumPlatforms; //Controlar el tama�o del nivel

    private int direction = 1; //1 derecha 0 izquierda
    private Vector3 lastPlatform;

    private float[] probPlatformLength = { 0.05f, 0.20f, 0.20f, 0.3f, 0.25f }; // Tiene que sumar 1
    private float[] probTrap = { 0.6f, 0.4f };

    public float probMoneda = 0.2f;

    public float probRampa = 0.15f;
    public float alturaRampa = 1f;


    public float velHorizontal;

    public GameObject playerPrefab;
    public GameObject platformPrefab;
    public GameObject rampaPrefab;
    public GameObject monedaPrefab;
    //public GameObject platform075Prefab;
    public GameObject cornerPrefab;
    public GameObject guidePrefab;
    public GameObject pinxoPrefab;


    public float trapDensity = 0.5f;
    public int[] trapMaxLength = { 3, 2 };


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            generarLevel();
        }
    }

    private void generarLevel()
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

        cornersPos = new List<Vector2>();
        cornersPos.Add(CoordManager.toCHCoords(platform.transform.position));

        /**
         *  - Mas prob a filas mas largas
         *  - Prob separadas para cada obstaculo
         *
         *
         * */
        int numFila = 0;
        while (totalNumPlatforms > numPlatform)
        {

            int numeroPlataformasSeguidas = calculateNumPlataformesSeguides();//Random.Range(minPlatformsUntilTurn, maxPlatformsUntilTurn);

            int numObstaclesRestants = 0; //En la primera iteracion no genera trampa!
            int trampa = -1;

            for (int i = 0; i < numeroPlataformasSeguidas; i++)
            {
                //TODO: RANDOM NUM PARA SABER QUE TIPO DE PLATAFORMA GENERAR
                //numObstaclesRestants > 0 --> se coloca obstaculo
                //numObstaclesRestants == 0 --> no se coloca obstaculo ni se generan mas (para que no se genere otro obstaculo justo al acabar el anterior o en la primera casilla (i==0))
                //numObstaclesRestants < 0 --> probabilidad de que se genere nuevo obstaculo
                if (numFila > 0 && numObstaclesRestants < 0 && Random.Range(0f, 1f) < trapDensity)
                {
                    trampa = randomElementWithProbabilities(probTrap);
                    numObstaclesRestants = Random.Range(1, trapMaxLength[trampa]);
                    Debug.Log("Obtaculo: " + trampa + " Cantidad: " + numObstaclesRestants);
                }

                if (i == numeroPlataformasSeguidas - 1 || numObstaclesRestants <= 0)
                {
                    trampa = -1;
                }

                // Generacion de terreno
                if ((Trampas)trampa != Trampas.HUECO)
                {

                    if (Random.value < probRampa && trampa == -1 && i < numeroPlataformasSeguidas - 1)
                    {
                        platform = Instantiate(rampaPrefab, lastPlatform + new Vector3(1 - direction, 0, direction), Quaternion.Euler(0, -90 * direction, 0), transform);
                        platform.GetComponent<Platform>().SetHeight(alturaRampa);
                        lastPlatform += Vector3.down * alturaRampa;
                    }
                    else
                    {
                        platform = Instantiate(platformPrefab, lastPlatform + new Vector3(1 - direction, 0, direction), Quaternion.identity, transform); //TODO: a�adir script a plataforma

                    }
                    if ((Trampas)trampa == Trampas.PINXO)
                        platform.GetComponent<Platform>().SetHeight(0.75f);
                }




                lastPlatform += new Vector3(1 - direction, 0, direction);
                numPlatform++;

                //Generacio de moneda
                generateMoneda(new Vector3(lastPlatform.x, trampa == -1 ? lastPlatform.y + 1 : lastPlatform.y + 2, lastPlatform.z));

                // Generacion de estructura
                if (i == numeroPlataformasSeguidas - 1)
                {
                    Instantiate(cornerPrefab, lastPlatform + Vector3.up, Quaternion.identity, platform.transform);
                    numObstaclesRestants = 0;

                    // guardar posicion plataforma en sistema de coordenadas de CH
                    cornersPos.Add(CoordManager.toCHCoords(platform.transform.position));
                }
                else if (numObstaclesRestants > 0 && (Trampas)trampa == Trampas.PINXO)
                {
                    Instantiate(pinxoPrefab, lastPlatform, Quaternion.identity, platform.transform);
                    if (numObstaclesRestants == 0) trampa = -1;
                }

                --numObstaclesRestants;
            }
            direction = 1 - direction;
            ++numFila;
        }
    }


    private void generateMoneda(Vector3 posMoneda)
    {
        float randNum = Random.value;
        if(randNum <= probMoneda)
        {
            Instantiate(monedaPrefab, posMoneda, Quaternion.identity, transform); 
        }
    }

    private int calculateNumPlataformesSeguides()
    {
        return randomElementWithProbabilities(probPlatformLength) + 2;
    }

    private int randomElementWithProbabilities(float[] probs)
    {
        float probability = Random.value; // "101" valores (se cuenta el 0), no es del todo exacto, pero no importa
        for (int i = 0; i < probs.Length; ++i)
        {
            if (probability <= probs[i]) return i;
            probability -= probs[i];
        }
        return -1;
    }

    public List<Vector2> getCornersPos()
    {
        return cornersPos;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
