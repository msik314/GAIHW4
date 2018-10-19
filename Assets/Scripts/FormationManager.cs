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

            Vector2 center = leader.transform.position - leader.transform.forward * (radiusScale * birds.Count + radiusBias);
            return center + (Vector2)(Quaternion.AngleAxis(index * 360.0f/birds.Count, Vector3.zero) * (leader.transform.position - (Vector3)center));
        }

        return obj.transform.position;
    }

    public void setLeader()
    {
        leader = birds[0];
        Formation f = leader.GetComponent<Formation>();
        f.setLeader();
    }
}
