using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mud : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Player p = other.GetComponent<Player>();
        p.velHorizontal = 0.5f;
    }

    private void OnTriggerExit(Collider other)
    {
        Player p = other.GetComponent<Player>();
        p.velHorizontal = LevelGenerator.Instance.velHorizontal;
    }
}
