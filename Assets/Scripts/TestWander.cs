using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TestWander : MonoBehaviour
{
    [SerializeField] private float targetDistance;
    [SerializeField] private float slowDistance;
    [SerializeField] private float maxOmega;
    [SerializeField] private float maxAlpha;
    [SerializeField] private float timeToTarget;
    [SerializeField] private float offset;
    [SerializeField] private float wanderRadius;
    [SerializeField] private float wanderRate;
    [SerializeField] private float maxAccel;
    [SerializeField] private float maxVel;
    
    private FaceWander fw;
    private Rigidbody2D rb;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        fw = new FaceWander(transform, targetDistance, slowDistance, maxOmega, maxAlpha, timeToTarget, offset, wanderRadius, wanderRate);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    
    void FixedUpdate()
    {
        rb.AddTorque(fw.get(0, rb.angularVelocity * Mathf.Deg2Rad));
        if(Mathf.Abs(rb.angularVelocity) * Mathf.Deg2Rad > maxOmega)
        {
            rb.angularVelocity = Mathf.Sign(rb.angularVelocity) * maxOmega * Mathf.Rad2Deg;
        }
        rb.AddForce(maxAccel * transform.right);
        if(rb.velocity.magnitude > maxVel)
        {
            rb.velocity = rb.velocity.normalized * maxVel;
        }
    }
}
