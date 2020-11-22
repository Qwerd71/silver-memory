using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Actionable firstTrigger;
    public List<GameObject> obstacles;

    public GameObject currentCheckpoint;
    public GameObject lastCheckpoint;

    public Player player;
    public Boss boss;

    public AudioSource audioSource;
    public AudioClip bossSound;
    private void Update()
    {
        if(lastCheckpoint != currentCheckpoint)
        {
            if(lastCheckpoint != null)
                GlowingSkull(lastCheckpoint, Color.white);
            lastCheckpoint = currentCheckpoint;
            GlowingSkull(currentCheckpoint, Color.green);
        }
    }
    private void GlowingSkull(GameObject skull,Color color)
    {
        skull.GetComponent<Renderer>().material.color = color;
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
    public void BossScene()
    {
        audioSource.Stop();
        audioSource.clip = bossSound;
        audioSource.Play();
    }
    public void PlayerDeath()
    {
        boss.life = boss.baseLife;
    }
}
