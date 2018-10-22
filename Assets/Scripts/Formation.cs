using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formation : AIBehavior
{
	private AIBehavior behavior;
    private FormationManager manager;
	private float pathFollowDistance;
	private Vector2[] path;
	private Rigidbody2D rb;

	public Formation(Transform owned, float accelTime, float maxSpeed, float maxAccel, float maxPredict, float pathFollowDistance, Vector2[] path, Rigidbody2D rb)
	{
		behavior = new Pursue(owned, 0, 0, accelTime, maxSpeed, maxAccel, maxPredict);
		manager = Object.FindObjectOfType<FormationManager> ();
		this.pathFollowDistance = pathFollowDistance;
		this.path = path;
		this.owned = owned;
		this.rb = rb;
	}

    void Awake()
    {
        manager = Object.FindObjectOfType<FormationManager>();
    }
    
    public override Vector2 get(Vector2 target, Vector2 currentVelocity, Vector2 targetVelocity = new Vector2())
    {
		return behavior.get (manager.getTarget (owned.gameObject), currentVelocity, manager.getVel());
    }

    public override void draw(GameObject target)
    {
		behavior.draw(target);
    }

    public bool isLeader{get; private set;}

    public void setLeader()
    {
        isLeader = true;
		behavior = new PathFollow (((Pursue)behavior).getArrive(), pathFollowDistance, path);
    }

	public Vector2 getVel()
	{
		return rb.velocity;
	}
}