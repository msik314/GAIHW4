using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Separate : AIBehavior
{
    private float castRadius;
    private string tagCompare;
    private Flee flee;
    
    public Separate(Transform owned, float maxAccel, float castRadius, string tagCompare = null)
    {
        this.owned = owned;
        this.castRadius = castRadius;
        this.flee = new Flee(owned, maxAccel);
        this.tagCompare = tagCompare;
    }
    
    public override Vector2 get(Vector2 target, Vector2 currentVelocity, Vector2 targetVelocity = new Vector2())
    {
        GameObject[] birds = Physics2D.OverlapCircleAll(owned.position, castRadius).Select(c => c.gameObject).ToArray();
        Vector2 f = Vector2.zero;
        int birdCount = 0;
        foreach(var b in birds)
        {
            if(tagCompare == null || b.tag.Contains(tagCompare))
            {
                f += (Vector2)b.transform.position;
                ++birdCount;
            }
        }
    
    if(birdCount > 0)
    {    
        f /= birdCount;
        return flee.get(f, currentVelocity);
    }

    return Vector2.zero;
}
	public override void draw(GameObject target){flee.draw (target);}
}
