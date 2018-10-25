using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive : AIBehavior
{
    private float maxAccel;
    private float maxSpeed;
    private float slowRadius;
    private float targetRadius;
    private float accelTime;
    
    public Arrive(Transform owned, float slowRadius, float targetRadius, float accelTime,  float maxSpeed, float maxAccel)
    {
        this.owned = owned;
        this.slowRadius = slowRadius;
        this.maxAccel = maxAccel;
        this.maxSpeed = maxSpeed;
        this.accelTime = accelTime;
        this.targetRadius = targetRadius;
    }
    
    public override Vector2 get(Vector2 target, Vector2 currentVelocity, Vector2 targetVelocity = new Vector2())
    {
        this.target = target;
        
        Vector2 direction = target - new Vector2(owned.position.x, owned.position.y);
        float dist = direction.magnitude;
        
        if(dist < targetRadius) return Vector2.zero;
        
        float speed = maxSpeed;
        if(dist < slowRadius)
        {
            speed = maxSpeed * dist/slowRadius;
        }
        
        Vector2 velocity = direction.normalized * speed;
        Vector2 accel = velocity - currentVelocity;
        accel /= accelTime;
        
        if(accel.magnitude > maxAccel)
        {
            accel = accel.normalized * maxAccel;
        }
        
        return accel;
    }
    
    public override void draw(GameObject target)
    {
        target.transform.position = this.target;
    }
    
    public void setMaxSpeed(float speed)
    {
        maxSpeed = speed;
    }

    public Vector2 pos {get {return new Vector2(owned.position.x, owned.position.y);}}
    public Transform tf {get{return owned;}}
}
