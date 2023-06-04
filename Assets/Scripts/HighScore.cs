using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScore : MonoBehaviour
{
    public int Highscore { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
