using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollow : AIBehavior
{
    private Arrive arrive;
    private float followDistance;
    private Vector2[] path;
    
    //Using arrive as opposed to seek in case path ends.
    public PathFollow(Transform owned, float slowRadius, float targetRadius, float accelTime,  float maxSpeed, float maxAccel, float followDistance, Vector2[] path)
    {
        arrive = new Arrive(owned, slowRadius, targetRadius, accelTime, maxSpeed, maxAccel);
        this.owned = owned;
        this.followDistance = followDistance;
        this.path = path;
    }
    
    //Seek like behavior with arrive
    public PathFollow(Transform owned, float accelTime, float maxSpeed, float maxAccel, float followDistance, Vector2[] path)
    {
        arrive = new Arrive(owned, 0, 0, accelTime, maxSpeed, maxAccel);
        this.owned = owned;
        this.followDistance = followDistance;
        this.path = path;
    }
    
    public PathFollow(Arrive arrive, float followDistance, Vector2[] path)
    {
        this.arrive = arrive;
        this.owned = arrive.tf;
        this.followDistance = followDistance;
        this.path = path;
    }
    
    public override Vector2 get(Vector2 target, Vector2 currentVelocity, Vector2 targetVelocity = new Vector2())
    {
        float minDist = (path[0] - (Vector2)owned.position).magnitude;
        int index = 0;
        int secondIndex;
        Vector2 forward;
        for(int i = 0; i < path.Length; ++i)
        {
            float dist = (path[i] - (Vector2)owned.position).magnitude;
            if(dist < minDist)
            {
                minDist = dist;
                index = i;
            }
        }
        if(index == 0)
        {
            secondIndex = 1;
            forward = path[secondIndex] - path[index];
        }
        else if(index == path.Length - 1)
        {
            secondIndex = path.Length - 2;
            forward = path[index] - path[secondIndex];
        }
        else
        {
            Vector2 toPrev = (Vector2)owned.position - path[index - 1];
            Vector2 toNext = (Vector2)owned.position - path[index + 1];
            forward = path[index + 1] - path[index - 1];
            float prevMag = (path[index] - path[index - 1]).magnitude;
            float nextMag = (path[index] - path[index + 1]).magnitude;
            float projToPrev = Vector2.Dot(toPrev, path[index] - path[index - 1])/prevMag;
            float projToNext = Vector2.Dot(toNext, path[index] - path[index + 1])/nextMag;
            
            if(projToNext >= nextMag && projToPrev >= prevMag)
            {
                secondIndex = index;
            }
            else if(projToNext >= nextMag)
            {
                secondIndex = index - 1;
            }
            else if(projToPrev >= prevMag)
            {
                secondIndex = index + 1;
            }
            else if(projToNext > projToPrev)
            {
                secondIndex = index  - 1;
            }
            else
            {
                secondIndex = index + 1;
            }
        }
        
        Debug.DrawLine(owned.position, path[index], Color.green);
        Debug.DrawLine(owned.position, path[secondIndex], Color.green);

        Vector2 objectDirection = (Vector2)owned.position - path[index];
        Vector2 pathDirection = path[secondIndex] - path[index];
        float len = Vector2.Dot(objectDirection, pathDirection) / pathDirection.magnitude;
        target = path[index];
        
        if(len > Mathf.Epsilon)
        {
            target += pathDirection.normalized * len;
             
            float dot = Vector2.Dot(path[index] - target, forward);
            if(dot < 0)
            {
                 ++index;
            }
        }
        else
        {
            ++index;
        }
        
        secondIndex = index + 1;
        
        len = (getOrLast(index) - target).magnitude;
        if(len > followDistance)
        {
            return arrive.get(distanceLerp(target, getOrLast(index), followDistance), currentVelocity);
        }
        len = followDistance - len;
        return arrive.get(distanceLerp(getOrLast(index), getOrLast(secondIndex), len), currentVelocity);

    }
    
    public override void draw(GameObject target)
    {
        
    }
    
    private Vector2 getOrLast(int index)
    {
        if(index >= path.Length)
        {
            return path[path.Length - 1];
        }
        
        return path[index];
    }
    
    private Vector2 distanceLerp(Vector2 start, Vector2 end, float distance)
    {
        return start + (end - start).normalized * distance;
    }
}
