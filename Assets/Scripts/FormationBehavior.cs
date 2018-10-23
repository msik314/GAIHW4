using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FormationBehavior : MonoBehaviour {

	[SerializeField] private float accelTime;
	[SerializeField] private float maxSpeed;
	[SerializeField] private float maxAccel;
	[SerializeField] private float maxPredict;
	[SerializeField] private float pathFollowDistance;
	[SerializeField] private float maxOmega;
	[SerializeField] private float maxAlpha;
	[Space]
	[SerializeField] private float castRadius;
	[SerializeField] private float castOffset;
	[SerializeField] private float rayAvoidDistance;
	[Space]
	[SerializeField] private GameObject[] path;

	private Formation formation;
	private AvoidRay rayAvoid;
	private Face face;
	private Rigidbody2D rb;

	void Awake(){
		Vector2[] pathPoints = path.Select (g => (Vector2)g.transform.position).ToArray();
		rb = GetComponent<Rigidbody2D> ();
		formation = new Formation (transform, accelTime, maxSpeed, maxAccel, maxPredict, pathFollowDistance, pathPoints, rb);
		rayAvoid = new AvoidRay (transform, maxAccel, castRadius, castOffset, rayAvoidDistance, maxSpeed, accelTime);
		face = new Face (transform, 0.05f, 0.1f, maxOmega, maxAlpha, accelTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){
		Vector2 force = formation.get(Vector2.zero, rb.velocity);
		Vector2 hitAvoid = rayAvoid.get (Vector2.zero, rb.velocity);
		if (hitAvoid.sqrMagnitude > 0.01) {
			force = hitAvoid;
		}

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

    void OnDestroy()
    {
        formation.kill();
    }

	public Formation getFormation()
	{
		return formation;
	}
}
