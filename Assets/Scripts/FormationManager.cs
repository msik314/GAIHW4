using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FormationType
{
    Scalable,
    Emergent,
    TwoLevel
}

public class FormationManager : MonoBehaviour
{
    [SerializeField] private FormationType startingFormation = FormationType.Scalable;
    [SerializeField] private float radiusScale = 0.16666666f;
    [SerializeField] private float radiusBias = 0.5f;
	[SerializeField] private List<GameObject> birds;
    
	GameObject leader;
	Formation leaderFormation;

    public static FormationType formation{get; private set;}
	void Awake()
    {
		formation = startingFormation;
	}

    void Start()
    {
        setLeader();
    }

    public Vector2 getTarget(GameObject obj)
    {
        if(formation == FormationType.Scalable)
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

        return obj.transform.position;
    }

    public void setLeader()
    {
        leader = birds[0];
		leaderFormation = leader.GetComponent<FormationBehavior>().getFormation();
		leaderFormation.setLeader();
    }

	public Vector2 getVel()
	{
		return leaderFormation.getVel();
	}
}
