using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private bool activated = false;
    private bool coroutineStarted = false;
    private Material color;
    private BoxCollider posCollider;
    private void Start()
    {
        color = this.GetComponent<Renderer>().material;
        posCollider = this.GetComponent<BoxCollider>();
    }
    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            this.color.color = Color.red;

        }
        else
        {
            this.color.color = Color.green;
        }
        if (!coroutineStarted)
        {
            StartCoroutine(Activation());
        }
    }
    private IEnumerator Activation()
    {
        coroutineStarted = true;
        yield return new WaitForSeconds(2f);
        activated = !activated;
        posCollider.center = new Vector3(0, (Convert.ToInt32(activated) *2 -1) * (posCollider.size.y /2));
        coroutineStarted = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.StartsWith("Player"))
        {
            other.gameObject.GetComponent<Player>().life -= 1;
        }
    }
}
