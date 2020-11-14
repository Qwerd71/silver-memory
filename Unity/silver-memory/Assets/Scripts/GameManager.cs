using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool firstTrigger = false;
    public GameObject obstacle;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (firstTrigger)
        {
            Debug.Log("First Trigger activated");
            obstacle.GetComponent<Collider>().isTrigger ^= true;
            firstTrigger = false;
        }
    }
}
