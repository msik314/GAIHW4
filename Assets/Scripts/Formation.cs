using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formation : AIBehavior
{
    private Pursue pursue;
    private FormationManager manager;

    void Awake()
    {
        manager = Object.FindObjectOfType<FormationManager>();
    }
    
    public override Vector2 get(Vector2 target, Vector2 currentVelocity, Vector2 targetVelocity = new Vector2())
    {
        return pursue.get(manager.getTarget(owned.gameObject), currentVelocity, targetVelocity);
    }

    public override void draw(GameObject target)
    {
        pursue.draw(target);
    }

    public bool isLeader{get; private set;}

    public void setLeader()
    {
        isLeader = true;
    }
}