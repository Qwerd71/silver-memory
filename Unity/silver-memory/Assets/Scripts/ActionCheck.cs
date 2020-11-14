using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCheck : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerController player;
    private void OnTriggerEnter(Collider other)
    {
        player.actionCheck = other.gameObject;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.Equals(player.actionCheck))
        {
            player.actionCheck = null;
        }
    }
}
