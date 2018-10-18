using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBehavior
{
    protected Transform owned;
    protected Vector2 target;
    
    public abstract Vector2 get(Vector2 target, Vector2 currentVelocity, Vector2 targetVelocity = new Vector2());
    
    public abstract void draw(GameObject target);
};
