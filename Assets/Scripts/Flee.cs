using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : AIBehavior
{
    private float maxAccel;
    
    public Flee(Transform owned, float maxAccel)
    {
        this.owned = owned;
        this.maxAccel = maxAccel;
    }
    
    public override Vector2 get(Vector2 target, Vector2 currentVelocity, Vector2 targetVelocity = new Vector2())
    {
        this.target = target;
        
        Vector2 direction =  new Vector2(owned.position.x, owned.position.y) - target;
        return direction.normalized * maxAccel;
    }
    
    public override void draw(GameObject target)
    {
        target.transform.position = this.target;
    }
    
    public Vector2 pos {get {return new Vector2(owned.position.x, owned.position.y);}}
}
