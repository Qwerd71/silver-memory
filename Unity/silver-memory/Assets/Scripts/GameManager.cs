using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool firstTrigger = false;
    public List<GameObject> obstacles;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (firstTrigger)
        {
            Debug.Log("First Trigger activated");
            foreach (GameObject obstacle in obstacles)
            {
                obstacle.GetComponent<Collider>().isTrigger ^= true;
            }
            firstTrigger = false;
        }
    }
}
