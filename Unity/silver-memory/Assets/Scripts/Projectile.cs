using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        StartCoroutine(ProjectileReturn());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator ProjectileReturn()
    {
        yield return new WaitForSeconds(1f);
        rb.velocity = -rb.velocity;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.StartsWith("Player"))
        {
            Debug.Log("Touché");
        }
    }
}
