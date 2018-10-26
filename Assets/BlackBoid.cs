using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoid : MonoBehaviour {

	private Pursue pursue;
	private Face face;
	private Rigidbody2D rb;


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


	void Awake(){
		rb = GetComponent<Rigidbody2D> ();
		pursue = new Pursue(transform, slowRadius, targetRadius, accelTime,  maxSpeed, maxAccel, maxPredict);
		face = new Face(transform, targetDistance, slowDistance, maxOmega, maxAlpha, timeToTarget);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){
		Vector2 target = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 force = pursue.get (target, rb.velocity);

		rb.AddForce (force);
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

	void OnCollisionEnter2D(Collision2D col){

		if (col.gameObject.GetComponent<FormationBehavior>()) {

			Destroy (col.gameObject);
		}
	}

}
