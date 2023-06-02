using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mud : MonoBehaviour
{

    float normalJumpVel;

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
        if (other.tag == "Player")
        {
            normalJumpVel = other.GetComponent<Player>().getJumpVel();
            GameManager.Instance.VelHorizontal = 1;
            other.GetComponent<Player>().setJumpVel(normalJumpVel - 2);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.VelHorizontal = GameManager.Instance.VelHorizontal;
            other.GetComponent<Player>().setJumpVel(normalJumpVel);
        }
    }
}
