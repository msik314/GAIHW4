using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockScript : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxAccel;
    [SerializeField] private float slowRadius;
    [SerializeField] private float targetRadius;
    [SerializeField] private float accelTime;
    [SerializeField] private float maxPredict;
    [SerializeField] private float targetDistance;
    [SerializeField] private float slowDistance;
    [SerializeField] private float maxOmega;
    [SerializeField] private float maxAlpha;
    [SerializeField] private float timeToTarget;
    [SerializeField] private float separateCastRadius;
    [SerializeField][Range(0, 1)] private float[] weights;

    private AIBehavior follow;
    private AIBehavior congregate;
    private AIBehavior separate;
    private Vector2 leaderPos;
    private Vector2 cgPos;
    private Rigidbody2D rb;
    private Face face;
    private Vector2 targetVel;
    private GameObject cg;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        follow = new Pursue(transform, slowRadius, targetRadius, accelTime,  maxSpeed, maxAccel, maxPredict);
        face = new Face(transform, targetDistance, slowDistance, maxOmega, maxAlpha, timeToTarget);
        congregate = new Arrive(transform, slowRadius, targetRadius, accelTime,  maxSpeed, maxAccel);
        separate = new Separate(transform, maxAccel, separateCastRadius);
        cg = FindObjectOfType<CGScript>().gameObject;
    }

    void Update()
    {
        leaderPos = targetObject.transform.position;
        targetVel = targetObject.GetComponent<Rigidbody2D>().velocity;
        cgPos = cg.transform.position;
    }

    void FixedUpdate()
    {
        Vector2 pursueForce = follow.get(leaderPos, rb.velocity, targetVel);
        Vector2 cgForce = congregate.get(cgPos, rb.velocity);
        Vector2 sepForce = separate.get(Vector2.zero, rb.velocity);
        Debug.DrawRay(transform.position, sepForce, Color.red);

        Vector2 force = pursueForce * weights[0] + cgForce * weights[1] + sepForce * weights[2]; 

        rb.AddForce(force);
        if(rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        
        float torque = face.get(Mathf.Atan2(rb.velocity.y, rb.velocity.x), rb.angularVelocity * Mathf.Deg2Rad);
        rb.AddTorque(torque);
        if(Mathf.Abs(rb.angularVelocity)* Mathf.Deg2Rad > maxOmega)
        {
            rb.angularVelocity = Mathf.Sign(rb.angularVelocity) * maxOmega * Mathf.Rad2Deg;
        }
    }
}
