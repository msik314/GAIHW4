using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue : AIBehavior
{
    private Arrive arrive;
    private float maxPredict;
    
    public Pursue(Transform owned, float slowRadius, float targetRadius, float accelTime,  float maxSpeed, float maxAccel, float maxPredict)
    {
        arrive = new Arrive(owned, slowRadius, targetRadius, accelTime, maxSpeed, maxAccel);
        this.maxPredict = maxPredict;
    }
    
    public Pursue(Arrive arrive, float maxPredict)
    {
        this.arrive = arrive;
        this.maxPredict = maxPredict;
    }
    
    public override Vector2 get(Vector2 target, Vector2 currentVelocity, Vector2 targetVelocity = new Vector2())
    {
        Vector2 direction = target - arrive.pos;
        float length = direction.magnitude;
        float speed = currentVelocity.magnitude;
        float predict;
        if(speed <= length / maxPredict)
        {
            predict = maxPredict;
        }
        else
        {
            predict = length / speed;
        }
        
        return arrive.get(target + targetVelocity * predict, currentVelocity);
    }
    
    public override void draw(GameObject target)
    {
        arrive.draw(target);
    }
}
