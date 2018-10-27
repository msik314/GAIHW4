using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InvisLeader : MonoBehaviour
{
    [SerializeField] private GameObject[] pathObjects;
    [SerializeField] private float accelTime = 0.1f;
    [SerializeField] private float maxSpeed = 3;
    [SerializeField] private float maxAccel = 6;
    [SerializeField] private float followDistance = 3;
    [SerializeField] private float slowScale = 0.25f;
    
    private PathFollow pathFollow;
    private Vector2[] path;
    private Rigidbody2D rb;
    private FormationManager formation;

    void Awake()
    {
        path = pathObjects.Select(o => (Vector2)o.transform.position).ToArray();
        pathFollow = new PathFollow(transform, accelTime, maxSpeed, maxAccel, followDistance, path);
        rb = GetComponent<Rigidbody2D>();
        formation = FindObjectOfType<FormationManager>();
    }

    void FixedUpdate()
    {
        float speed = formation.getTotalSlowdown(slowScale);
        pathFollow.slowDown(speed);
		Vector2 force = pathFollow.get(Vector2.zero, rb.velocity);
		rb.AddForce(force);
		if(rb.velocity.magnitude > maxSpeed * speed)
		{
			rb.velocity = rb.velocity.normalized * maxSpeed * speed;
		}
    }

    public Vector2 getVel()
    {
        return rb.velocity;
    }

    public Vector2 getForward()
    {
        return rb.velocity.normalized;
    }
}
