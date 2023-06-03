using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Trampas
{
    NORMAL = -1, PINXO, HUECO, LENTO, BIOMA, CORNER
}

public class LevelGenerator : MonoBehaviour
{
    // Start is called before the first frame update

    public static LevelGenerator Instance { get; private set; }

    private List<Vector2> cornersPos;
    private List<Vector3> boulaRoute;

    public Vector3 spawnPoint;
    public int minPlatformsUntilTurn;
    //public int maxPlatformsUntilTurn;
    public int totalNumPlatforms; //Controlar el tama�o del nivel

    public int minFilasInBiome;
    public int maxFilasInBiome;

    private int direction = 1; //1 derecha 0 izquierda
    private Vector3 lastPlatform;

    public float[] probPlatformLength = { 0.05f, 0.20f, 0.20f, 0.3f, 0.25f }; // Tiene que sumar 1
    public float[] probTrap = { 0.4f, 0.4f, 0.2f };
    public float probTrapBioma = 0.8f;

    public float probMoneda = 0.2f;

    public float probRampa = 0.15f;
    public float alturaRampa = 1f;


    private float velHorizontal;

    public GameObject playerPrefab;
    public GameObject platformPrefab;
    public GameObject rampaPrefab;
    public GameObject monedaPrefab;
    //public GameObject platform075Prefab;
    public GameObject cornerPrefab;
    public GameObject guidePrefab;
    public GameObject pinxoPrefab;

    public GameObject aroPrefab;
    public GameObject pinguinPrefab;

    public GameObject pilarPrefab;


    public float trapDensity = 0.5f;
    public int[] trapMaxLength = { 3, 2 };

    private readonly Color[] coloresFogBioma = { new Color(109 / 255f, 123 / 255f, 99 / 255f), new Color(123 / 255f, 107 / 255f, 99 / 255f), new Color(153 / 255f, 171 / 255f, 180 / 255f), new Color(46 / 255f, 2 / 255f, 2 / 255f) };


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            velHorizontal = GameManager.Instance.velHorizontal;
        }
    }

    private void Start()
    {
         GenerarLevel();
    }

    private void GenerarTrampaBioma(Bioma b, Vector3 inicioFila, int numBloquesFila, int direction)
    {
        switch (b)
        {
            case Bioma.FIRE: // Bola de fuego
                Instantiate(aroPrefab, inicioFila + new Vector3(1 - direction, 0, direction) * (numBloquesFila+1) + Vector3.up, Quaternion.Euler(0, -180 + 90 * (1-direction), 0)).GetComponent<Aro>().MaxDistActivation = numBloquesFila + 1;
                break;
            case Bioma.DESERT: // Desprendimiento? / Terremoto?
                break;
            case Bioma.ICE: // Pinguin 👍
                Instantiate(pinguinPrefab, inicioFila + Vector3.up + new Vector3(1 - direction, 0, direction), Quaternion.Euler(0, 90 * (1 - direction), 0)).GetComponent<Pinguin>().Init(2, numBloquesFila - 2, direction);
                break;
        }
    }

    public void ChangeBioma(Bioma b, float transitionTime)
    {
        GameManager.Instance.ChangeFogColor(coloresFogBioma[(int)b], transitionTime);
    }

    private void GenerarLevel()
    {
        int numPlatform = 0;
        Player player = Instantiate(playerPrefab, spawnPoint + Vector3.up, Quaternion.identity).GetComponent<Player>();
        player.velHorizontal = velHorizontal;

        Guide guide = Instantiate(guidePrefab, spawnPoint + Vector3.up, Quaternion.identity).GetComponent<Guide>();
        guide.Init(player.gameObject);


        Bioma bioma = Bioma.ICE;
        ChangeBioma(bioma, 0);

        /**
         *  - Mas prob a filas mas largas
         *  - Prob separadas para cada obstaculo
         *
         *
         * */
        int numFila = 0;
        int numeroFilasBioma = Random.Range(minFilasInBiome, maxFilasInBiome + 1);
        while (numPlatform < totalNumPlatforms)
        {

            if (numeroFilasBioma <= 0)
            {
                bioma = (Bioma)Random.Range(0, 3 + 1);
                numeroFilasBioma = Random.Range(minFilasInBiome, maxFilasInBiome + 1);
            }
            int numeroPlataformasSeguidas = calculateNumPlataformesSeguides();//Random.Range(minPlatformsUntilTurn, maxPlatformsUntilTurn);

            Debug.Log(numeroPlataformasSeguidas);

            Platform platform;
            if (numPlatform == 0) // Plataforma inicial
            {
                //Plataforma inicial
                platform = Instantiate(platformPrefab).GetComponent<Platform>();
                platform.transform.parent = transform;
                platform.transform.position = spawnPoint;
                platform.Bioma = bioma;
                platform.Trampa = Trampas.NORMAL;
                lastPlatform = spawnPoint;

                cornersPos = new List<Vector2>
                {
                    CoordManager.toCHCoords(platform.transform.position)
                };

                boulaRoute = new List<Vector3>();
            }
            else // Corner
            {
                //Instantiate(cornerPrefab, platform.transform.position + Vector3.up, Quaternion.identity, platform.transform);
                platform = Instantiate(platformPrefab, lastPlatform + new Vector3(1 - direction, 0, direction), Quaternion.identity, transform).GetComponent<Platform>();
                platform.Bioma = bioma;
                platform.Trampa = Trampas.CORNER;

                Instantiate(pilarPrefab, platform.transform.position + Vector3.down * 2.5f, Quaternion.identity, platform.transform).GetComponent<Pilar>().Bioma = bioma;

                // guardar posicion plataforma en sistema de coordenadas de CH
                cornersPos.Add(CoordManager.toCHCoords(platform.transform.position));
                lastPlatform += new Vector3(1 - direction, 0, direction);
                direction = 1 - direction;

            }
            boulaRoute.Add(platform.transform.position + Vector3.up);
            numPlatform++;

            int numObstaclesRestants = 0; //En la primera iteracion no genera trampa! (pasará a -1 y ya a la siguiente puede que se genere trampa)
            int trampa = -1;

            int numBloquesASaltar = 0;


            if (numFila > 0 && numeroPlataformasSeguidas > 3 && Random.value <= probTrapBioma) // Generar trampa especial de bioma, y no generar ninguna mas en la fila
            {
                trampa = (int)Trampas.BIOMA;
                GenerarTrampaBioma(bioma, lastPlatform, numeroPlataformasSeguidas, direction);
                numObstaclesRestants = 20;
            }



            for (int i = 1; i < numeroPlataformasSeguidas; i++)
            {
                //TODO: RANDOM NUM PARA SABER QUE TIPO DE PLATAFORMA GENERAR
                //numObstaclesRestants > 0 --> se coloca obstaculo
                //numObstaclesRestants == 0 --> no se coloca obstaculo ni se generan mas (para que no se genere otro obstaculo justo al acabar el anterior o en la primera casilla (i==0))
                //numObstaclesRestants < 0 --> probabilidad de que se genere nuevo obstaculo
                if (numFila > 0 && numObstaclesRestants < 0 && Random.Range(0f, 1f) < trapDensity)
                {
                    trampa = randomElementWithProbabilities(probTrap);
                    numObstaclesRestants = Random.Range(1, trapMaxLength[trampa] + 1);
                    numBloquesASaltar = numObstaclesRestants;
                    Debug.Log("Obtaculo: " + trampa + " Cantidad: " + numObstaclesRestants);
                }

                if (numObstaclesRestants <= 0)
                {
                    trampa = -1;
                }

                float alturaBola = 0.5f;
                // Generacion de terreno
                if ((Trampas)trampa != Trampas.HUECO)
                {

                    if ((Trampas)trampa != Trampas.BIOMA && Random.value < probRampa && trampa == -1 && i < numeroPlataformasSeguidas - 1)
                    {
                        platform = Instantiate(rampaPrefab, lastPlatform + new Vector3(1 - direction, 0, direction), Quaternion.Euler(0, -90 * direction, 0), transform).GetComponent<Platform>();
                        platform.SetHeight(alturaRampa);
                        lastPlatform += Vector3.down * alturaRampa;
                        alturaBola = alturaRampa + 0.4f;
                    }
                    else //No es ni rampa ni hueco --> puede pasar la roca
                    {
                        platform = Instantiate(platformPrefab, lastPlatform + new Vector3(1 - direction, 0, direction), Quaternion.identity, transform).GetComponent<Platform>(); //TODO: a�adir script a plataforma
                        alturaBola = 0;
                    }
                    if ((Trampas)trampa == Trampas.PINXO)
                        platform.SetHeight(0.75f);

                    //set bioma
                    platform.Bioma = bioma;


                    // Generacion de estructura
                    if (numObstaclesRestants > 0 && (Trampas)trampa == Trampas.PINXO)
                    {
                        Instantiate(pinxoPrefab, platform.transform.position, Quaternion.identity, platform.transform);
                        if (numObstaclesRestants == 0) trampa = -1;
                        platform.Trampa = Trampas.NORMAL;
                    }
                    else if ((Trampas)trampa == Trampas.BIOMA)
                    {
                        platform.Trampa = Trampas.NORMAL;
                    }
                    else
                    {
                        platform.Trampa = (Trampas)trampa;
                    }
                }
                else // Hueco
                {
                    alturaBola = 0.4f + (numBloquesASaltar - 1) * 0.25f;
                }



                lastPlatform += new Vector3(1 - direction, 0, direction);
                numPlatform++;

                boulaRoute.Add(lastPlatform + Vector3.up + Vector3.up * alturaBola);

                //Generacio de moneda
                generateMoneda(new Vector3(lastPlatform.x, trampa == -1 ? lastPlatform.y + 1 : lastPlatform.y + 2, lastPlatform.z));

                

                --numObstaclesRestants;
            }
            ++numFila;
            --numeroFilasBioma;
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
        return randomElementWithProbabilities(probPlatformLength) + minPlatformsUntilTurn;
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

    public List<Vector3> getBoulaRoute()
    {
        return boulaRoute;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
