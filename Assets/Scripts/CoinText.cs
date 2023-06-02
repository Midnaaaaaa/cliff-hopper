using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinText : MonoBehaviour
{
    private TextMeshPro textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncrementText()
    {
        textMeshPro.text = (Convert.ToInt32(textMeshPro.text) + 1).ToString();
    }
}
