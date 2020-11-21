using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Actionable firstTrigger;
    public List<GameObject> obstacles;

    public Transform lastCheckpoint;
    public Player player;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (firstTrigger.actioned)
        {
            firstTrigger.actioned = false;
            StartCoroutine(LiquidWater());
        }
    }
    private IEnumerator LiquidWater()
    {
        firstTrigger.gameObject.SetActive(false);
        Debug.Log("First Trigger activated");
        foreach (GameObject obstacle in obstacles)
        {
            obstacle.GetComponent<Collider>().isTrigger ^= true;
        }
        yield return new WaitForSeconds(firstTrigger.timer);
        foreach (GameObject obstacle in obstacles)
        {
            obstacle.GetComponent<Collider>().isTrigger ^= true;
        }
        firstTrigger.gameObject.SetActive(true);
    }
}
