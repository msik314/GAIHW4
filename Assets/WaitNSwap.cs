using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitNSwap : MonoBehaviour {

	FormationManager formation;
	[SerializeField] GameObject[] birds;
	[SerializeField] GameObject point1;
	[SerializeField] GameObject point2;
	[SerializeField] float waitTime = 4f;

	private float swaptimer = .2f;
	private float swaptimerRESET = .2f;

	public enum State
	{
		untouched,
		waiting,
		done

	}
	public State _state = State.untouched;
	private Vector3 p1OG;
	private Vector3 p2OG;

	[SerializeField] bool triggered = false;
	// Use this for initialization
	void Start () {
		formation = FindObjectOfType<FormationManager> ();
		p1OG = point1.transform.position;
		p2OG = point2.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		switch (_state) {
		case State.untouched:
			if (triggered) {
				/*
				for(int i = 0; i < birds.Length; i++){
					if (birds [i] != null) {
						//birds[i].GetComponent<FormationBehavior>().
					}

				}*/
				_state = State.waiting;
			}
			break;
		case State.waiting:
			if (waitTime <= 0f) {
				point1.transform.position = p1OG;
				point2.transform.position = p2OG;
				_state = State.done;
				break;
			}
			waitTime -= Time.deltaTime;
			swaptimer -= Time.deltaTime;
			if (swaptimer <= 0f) {
				Vector3 temp = point1.transform.position;
				point1.transform.position = point2.transform.position;
				point2.transform.position = temp;
				swaptimer = swaptimerRESET;
			}

			break;
		case State.done:


			break;
		


		}


	}

	void swapPos(Vector3 pos1, Vector3 pos2){
		Vector3 temp = pos1;
		pos1 = pos2;
		pos2 = temp;

	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.GetComponent<FormationBehavior> () != null) {
			triggered = true;

		}

	}

}
