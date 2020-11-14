using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Start is called before the first frame update
    public Material ice;
    public Material water;
    private Material material; 
    private Collider liquid;

    void Start()
    {
        liquid = GetComponent<Collider>();
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (liquid.isTrigger)
        {
            material.color = water.color;
        }
        else
        {
            material.color = ice.color;
        }
    }
}
