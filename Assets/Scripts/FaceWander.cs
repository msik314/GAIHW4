using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceWander:Face
{
    private float offset;
    private float radius;
    private float rate;
    private float wanderAngle;
    
    public FaceWander(Transform owned, float targetDistance, float slowDistance, float maxOmega, float maxAlpha, float timeToTarget, float offset, float wanderRadius, float wanderRate):
        base(owned, targetDistance, slowDistance, maxOmega, maxAlpha, timeToTarget)
    {
        wanderAngle = Mathf.Deg2Rad * owned.rotation.eulerAngles.z;
        this.offset = offset;
        radius = wanderRadius;
        rate = wanderRate;
    }
    
    public override float get(float targetAngle, float currentOmega)
    {
        wanderAngle += randBinom() * rate;
        
        Vector2 targetPos = owned.right * offset;
        targetPos += radius * new Vector2(Mathf.Cos(wanderAngle), Mathf.Sin(wanderAngle));
        
        this.target = targetPos;
        
        return base.get(Mathf.Atan2(targetPos.x, targetPos.y), currentOmega);
    }
    
    public override void draw(GameObject target)
    {
        target.transform.position = owned.position + (Vector3)this.target;
    }
    
    private static float randBinom()
    {
        return Random.value - Random.value;
    }
}
