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
	
//hi david its sera im coding your code

//jhfsllaksidjjfknlanmakjlsnlknddoajhsddnkjskljnajjskdljjnnm,sopwijeiiurl8989929001-04384892jjnfjnsljnjbflnj
//it was cvery dark thats intense was there cahnting DING DONG DING DONG who is that axel is a flamingo oh its max hi max i guess he didnt eat anything and will be staying for dinner :/ ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS ASS hack the world HACK MAINFRAME HACK HACK HACK HACK HACK HACK ASS; HACKK ASSSSSSSSS