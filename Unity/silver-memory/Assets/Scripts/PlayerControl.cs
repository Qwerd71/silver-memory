using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    private bool turning = false;
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        rb.rotation = Quaternion.Euler(-Input.mousePosition.y,Input.mousePosition.x,0);
        if ( vertical != 0 )
        {
            rb.AddForce(this.transform.forward * vertical,ForceMode.Acceleration);
            //rb.MovePosition(this.transform.position + vertical* this.transform.forward);
        }
        /*if (horizontal != 0 && !turning)
        {
            StartCoroutine(Turn90Degrees(Math.Sign(horizontal)));
        }*/
    }
    private IEnumerator Turn90Degrees(int side)
    {
        turning = true;
        rb.rotation = Quaternion.Euler(0, rb.rotation.eulerAngles.y + side * 90, 0);
        yield return new WaitForSeconds(0.5f);
        turning = false;
    }
}
