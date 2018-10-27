using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum FormationType
{
    Scalable,
    Emergent,
    TwoLevel
}

public class FormationManager : MonoBehaviour
{
    internal readonly Color[] groupColors = new[]{Color.cyan, Color.magenta, Color.red, Color.yellow};
    [SerializeField] private FormationType startingFormation = FormationType.Scalable;
    [SerializeField] private float radiusScale = 0.16666666f;
    [SerializeField] private float radiusBias = 0.5f;
    [SerializeField] private float emergentSeparation = 0.75f;
    [SerializeField] private float slowDownRadius = 0.5f;
    [SerializeField] private float slowScale = 0.5f;
    [SerializeField] private float groupSacing = 2;
	[SerializeField] private List<GameObject> birds;
    [SerializeField] private InvisLeader invisLeader;
    
	private GameObject leader;
	private Formation leaderFormation;
    private List<List<Formation>> subLeaders;
    private FormationType _formation;
    public FormationType formation{get{return _formation;} set{setFormation(value);}}

    void Start()
    {
        subLeaders = new List<List<Formation>>();
        setLeader();
        setFormation(startingFormation);

        if(birds.Count  == 0) return;
        subLeaders.Add(new List<Formation>(new[]{birds[0].GetComponent<FormationBehavior>().getFormation()}));
        for(int i = 1; i < birds.Count; ++i)
        {
            Formation current = birds[i].GetComponent<FormationBehavior>().getFormation();
            Formation previous = birds[i - 1].GetComponent<FormationBehavior>().getFormation();

            current.setParent(previous);
            previous.setChild(current);
            if(subLeaders.Count <= i / 3)
            {
                subLeaders.Add(new List<Formation>());
            }
            subLeaders[i / 3].Add(current);
        }
    }

    void setFormation(FormationType type)
    {
        _formation = type;
        foreach(Formation f in birds.Select(b => b.GetComponent<FormationBehavior>().getFormation()))
        {
            f.setType(type);
        }
    }

    public Vector2 getTarget(GameObject obj, Formation f)
    {
        if(_formation == FormationType.Scalable)
        {
            int index = birds.IndexOf(obj);
            if(index < 0) return obj.transform.position;

            Vector2 center = leader.transform.position - leader.transform.right * (radiusScale * birds.Count + radiusBias);
            Vector2 res =  leader.transform.right * (radiusScale * birds.Count + radiusBias);
            res = Quaternion.Euler(0, 0, 360.0f/birds.Count * index) * res;
            res = res + center;
            Debug.DrawLine(center, res, Color.yellow);
            return res;
        }
        else if(_formation == FormationType.TwoLevel)
        {
            int flIndex = -1;
            int slIndex = -1;
            for(int i = 0; i < subLeaders.Count; ++i)
            {
                slIndex = subLeaders[i].IndexOf(f);
                if(slIndex >= 0)
                {
                    flIndex = i;
                    break;
                }
            }
            if(flIndex < 0 ) return obj.transform.position;
            Vector2 center;
            Vector2 forward;
            if(flIndex == 0)
            {
                forward = invisLeader.getForward();
                center = (Vector2)invisLeader.transform.position - groupSacing * forward;
            }
            else
            {
                forward = subLeaders[flIndex - 1][0].getVel();
                if(forward.sqrMagnitude < 0.1)
                {
                    forward = subLeaders[flIndex - 1][0].getForward();
                }
                else
                {
                    forward = forward.normalized;
                }
                center = subLeaders[flIndex - 1][0].getPos();
                center -= groupSacing * forward;
            }
            Vector2 res = forward * (radiusScale * subLeaders[flIndex].Count + radiusBias);
            res = Quaternion.Euler(0, 0, 360.0f/subLeaders[flIndex].Count * slIndex) * res;
            res = res + center;
            Debug.DrawLine(center, res, groupColors[flIndex]);
            return res;           
        }

        return obj.transform.position;
    }

    public float getTotalSlowdown(float scale)
    {
        int slowedCount = 0;
        for(int i = 1; i < birds.Count; ++i)
        {
            if(((Vector2)birds[i].transform.position - getTarget(birds[i], null)).sqrMagnitude > slowDownRadius * slowDownRadius)
            {
                ++slowedCount;
            }
        }

        return 1.0f - (float)(slowedCount) / Mathf.Max(birds.Count - 1, 1) * scale;
    }

    public float getSlowdown(Formation fObj)
    {
        int slowedCount = 0;
        if(_formation != FormationType.TwoLevel)
        {
            return getTotalSlowdown(slowScale);
        }
        else
        {
            int flIndex = -1;
            int slIndex = -1;
            for(int i = 0; i < subLeaders.Count; ++i)
            {
                slIndex = subLeaders[i].IndexOf(fObj);
                if(slIndex == 0)
                {
                    flIndex = i;
                    break;
                }
                if(slIndex > 0) return 1;
            }
            for(int i = 1; i < subLeaders[i].Count; ++i)
            {
                Formation f = subLeaders[flIndex][i];
                if((f.getPos() - getTarget(f.getObject(), f)).sqrMagnitude > slowDownRadius * slowDownRadius)
                {
                    ++slowedCount;
                }
            }
            float quot = (float)(slowedCount) / Mathf.Max(subLeaders[flIndex].Count - 1, 1.0f);
            float res = 1.0f - quot * slowScale;
            return res;
        }
    }

    void setLeader()
    {
        leader = birds[0];
		leaderFormation = leader.GetComponent<FormationBehavior>().getFormation();
		leaderFormation.setLeader();
    }

    public void Kill(GameObject go, Formation f)
    {
        birds.Remove(go);
        for(int i = 0; i < subLeaders.Count; ++i)
        {
            if(subLeaders[i].Contains(f))
            {
                subLeaders[i].Remove(f);
                if(subLeaders[i].Count == 0)
                {
                    subLeaders.RemoveAt(i);
                }
                break;
            }
        }
        setLeader();
    }

	public Vector2 getVel(Formation f)
	{
        if(_formation == FormationType.TwoLevel)
        {
            int flIndex = -1;
            int slIndex = -1;
            for(int i = 0; i < subLeaders.Count; ++i)
            {
                slIndex = subLeaders[i].IndexOf(f);
                if(slIndex >= 0)
                {
                    flIndex = i;
                    break;
                }
            }
            if(flIndex == 0)
            {
                return invisLeader.getVel();
            }
            else
            {
                return subLeaders[flIndex - 1][0].getVel();
            }    
        }
		return leaderFormation.getVel();
	}

    public float getSeparation()
    {
        return emergentSeparation;
    }
}
