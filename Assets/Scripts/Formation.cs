using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formation : AIBehavior
{
	private Pursue pursue;
    private PathFollow pathFollow;
    private AIBehavior behavior;
    private FormationManager manager;
	private Rigidbody2D rb;
    private Formation parent;
    private Formation child;
    private FormationType type;

	public Formation(Transform owned, float accelTime, float maxSpeed, float maxAccel, float maxPredict, float pathFollowDistance, Vector2[] path, Rigidbody2D rb)
	{
		pursue = new Pursue(owned, 0, 0, accelTime, maxSpeed, maxAccel, maxPredict);
        pathFollow = new PathFollow (pursue.getArrive(), pathFollowDistance, path);
        behavior = pursue;
		manager = Object.FindObjectOfType<FormationManager> ();
		this.owned = owned;
		this.rb = rb;
        parent = null;
        child = null;
	}
    
    public override Vector2 get(Vector2 target, Vector2 currentVelocity, Vector2 targetVelocity = new Vector2())
    {
        if(type == FormationType.TwoLevel)
        {
            return pursue.get(manager.getTarget (owned.gameObject, this), currentVelocity, manager.getVel(this));
        }
        else if(type != FormationType.Emergent || isLeader)
        {
		    return behavior.get(manager.getTarget (owned.gameObject, this), currentVelocity, manager.getVel(this));
        }

        Vector2 vel = parent.getVel();
        Vector2 direction;
        if(vel.sqrMagnitude < 0.05f)
        {
            vel = parent.getForward();
            direction = vel;
        }
        else
        {
            direction = vel.normalized;
        }

        return pursue.get(parent.getPos() - manager.getSeparation() * direction, currentVelocity, parent.getVel());
    }

    public void setType(FormationType type)
    {
        this.type = type;
    }

    public override void draw(GameObject target)
    {
		behavior.draw(target);
    }

    public bool isLeader{get; private set;}

    public void setLeader()
    {
        isLeader = true;
		behavior = pathFollow;
    }

    public void setNotLeader()
    {
        isLeader = false;
        behavior = pursue;
    }

    public void kill()
    {
        parent?.setChild(child);
        child?.setParent(parent);
        manager.Kill(owned.gameObject, this);
    }

	public Vector2 getVel()
	{
		return rb.velocity;
	}

    public void setParent(Formation f)
    {
        parent = f;
    }

    public void setChild(Formation f)
    {
        child = f;
    }

    public Vector2 getPos()
    {
        return owned.position;
    }

    public Vector2 getForward()
    {
        return owned.forward;
    }

    public GameObject getObject()
    {
        return owned.gameObject;
    }

    public void AdjustSpeed(float maxSpeed)
    {
        if(isLeader || type == FormationType.TwoLevel)
        {
            pursue.getArrive().setMaxSpeed(maxSpeed * manager.getSlowdown(this));
        }
        else
        {
            pursue.getArrive().setMaxSpeed(maxSpeed);
        }
    }
}