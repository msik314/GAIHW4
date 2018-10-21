using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidRay : AIBehavior 
{

	private float castRadius;
	private float castOffset;
	private string tagCompare;
	private float rayAvoidDistance;
	private Arrive arrive;

	public AvoidRay(Transform owned, float maxAccel, float castRadius, float castOffset, float rayAvoidDistance, float maxSpeed, float accelTime)
	{
		this.owned = owned;
		this.castRadius = castRadius;
		this.castOffset = castOffset;
		this.rayAvoidDistance = rayAvoidDistance;
		this.arrive = new Arrive(owned, 0, 0, accelTime, maxSpeed, maxAccel);
		this.tagCompare = tagCompare;
	}

	public override Vector2 get(Vector2 target, Vector2 currentVelocity, Vector2 targetVelocity = new Vector2())
	{
		Vector2 vel = currentVelocity;
		if (vel.sqrMagnitude < Mathf.Epsilon) {
			vel = owned.forward;
		}

		Vector2 horiz = new Vector2 (vel.y, -vel.x).normalized;

		Vector2[] rays = new Vector2[]{
			(Vector2)owned.position + horiz * castOffset,
			(Vector2)owned.position + -horiz * castOffset
		};

		RaycastHit2D[] hitData = new RaycastHit2D[2];
		bool[] hits = new bool[]{false, false};

		for (int i = 0; i < rays.Length; ++i) {
			hitData[i] = Physics2D.Raycast(rays[i], vel.normalized, castRadius, ~256);
			hits [i] = hitData[i].collider != null;
		}
		RaycastHit2D hit;

		if (hits[0] && hits[1]) {
			Debug.DrawRay(rays[0], vel.normalized * castRadius, Color.red);
			Debug.DrawRay(rays[1], vel.normalized * castRadius, Color.red);
			Vector2[] hitRelativePoints = new Vector2[2];
			hitRelativePoints[0] = hitData[0].point;
			hitRelativePoints[1] = hitData[1].point;
			hit = hitRelativePoints[0].sqrMagnitude <= hitRelativePoints[1].sqrMagnitude ? hitData[0] : hitData[1];
		} else if(hits[0]) {
			Debug.DrawRay(rays[0], vel.normalized * castRadius, Color.red);
			Debug.DrawRay(rays[1], vel.normalized * castRadius, Color.blue);
			hit = hitData[0];
		} else if(hits[1]) {
			Debug.DrawRay(rays[0], vel.normalized * castRadius, Color.blue);
			Debug.DrawRay(rays[1], vel.normalized * castRadius, Color.red);
			hit = hitData[1];
		} else {
			Debug.DrawRay(rays[0], vel.normalized * castRadius, Color.blue);
			Debug.DrawRay(rays[1], vel.normalized * castRadius, Color.blue);
			return Vector2.zero;
		}

		Debug.DrawLine(owned.position, hit.point + hit.normal * rayAvoidDistance, Color.green, 0);

		return arrive.get(hit.point + hit.normal * rayAvoidDistance, currentVelocity);

	}

	public override void draw(GameObject target){arrive.draw (target);}
}
