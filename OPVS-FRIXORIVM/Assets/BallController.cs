using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RespawnBall()
    {
        rb.velocity = Vector3.zero;
        gameObject.transform.position = GameObject.FindWithTag("Ball Transform").transform.position;
    }
}
