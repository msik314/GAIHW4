using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evade : AIBehavior {
    private Flee flee;
    private float maxPredict;
    
    public Evade(Transform owned, float maxAccel, float maxPredict)
    {
        flee = new Flee(owned, maxAccel);
        this.maxPredict = maxPredict;
    }
    
    public Evade(Flee flee, float maxPredict)
    {
        this.flee = flee;
        this.maxPredict = maxPredict;
    }
    
    public override Vector2 get(Vector2 target, Vector2 currentVelocity, Vector2 targetVelocity = new Vector2())
    {
        Vector2 direction = target - flee.pos;
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
        
        return flee.get(target + targetVelocity * predict, currentVelocity);
    }
    
    public override void draw(GameObject target)
    {
        flee.draw(target);
    }
}
