using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playerToFollow;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = playerToFollow.transform.position;
        this.transform.rotation =Quaternion.Lerp(this.transform.rotation, playerToFollow.transform.rotation,0.1f);
    }
}
