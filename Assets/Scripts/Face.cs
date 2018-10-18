using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face
{
    protected Transform owned;
    protected float maxOmega;
    protected float maxAlpha;
    protected float targetDistance;
    protected float slowDistance;
    protected float timeToTarget;
    
    protected Vector2 target;
    
    public Face(Transform owned, float targetDistance, float slowDistance, float maxOmega, float maxAlpha, float timeToTarget)
    {
        this.owned = owned;
        this.maxOmega = maxOmega;
        this.maxAlpha = maxAlpha;
        this.targetDistance = targetDistance;
        this.slowDistance = slowDistance;
        this.timeToTarget = timeToTarget;
    }
    
    public virtual float get(float targetAngle, float currentOmega)
    {
        target = new Vector2(Mathf.Cos(targetAngle), Mathf.Sin(targetAngle));
        Debug.DrawRay(owned.position, target, Color.blue, 0.0f, false);
        float rotation = targetAngle - Mathf.Deg2Rad * owned.rotation.eulerAngles.z;
        
        while(rotation > Mathf.PI)
        {
            rotation -= Mathf.PI * 2;
        }
        while(rotation < -Mathf.PI)
        {
            rotation += Mathf.PI * 2;
        }
        
        float rotationSize = Mathf.Abs(rotation);
        if(rotationSize < targetDistance) return 0;
        
        float targetOmega = 0;
        
        if(rotationSize > slowDistance)
        {
            targetOmega = maxOmega * Mathf.Sign(rotation);
        }
        
        else
        {
            targetOmega = maxOmega * rotation / slowDistance;
        }
        
        float targetAlpha = (targetOmega - currentOmega)/timeToTarget;
        
        if(Mathf.Abs(targetAlpha) > maxAlpha)
        {
            targetAlpha = Mathf.Sign(targetAlpha) * maxAlpha;
        }
        
        return targetAlpha;
    }
    
    public virtual void draw(GameObject target)
    {
        
    }
}
