using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocks : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.StartsWith("Player"))
        {
            Debug.Log("Touché");
            other.GetComponent<Player>().life -= 1;
        }
    }
}
