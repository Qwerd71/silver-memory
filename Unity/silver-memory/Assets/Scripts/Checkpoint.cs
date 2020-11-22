using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameManager gameManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.StartsWith("Player"))
        {
            gameManager.lastCheckpoint = gameManager.currentCheckpoint;
            gameManager.currentCheckpoint = this.gameObject;
        }
    }
}
