using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PersonAI : MonoBehaviour
{
	#region Members
	
	public Animator animations;
	public NavMeshAgent navMeshAgent;
	public Animator stateMachine;
	public Vector3 target;

	#endregion

	#region UnityEvents

	public void Update()
	{
		//Synchronize motion
		float forwardVelocity = navMeshAgent.velocity.magnitude;
		//float angularVelocity = navMeshAgent.velocity.magnitude - forwardVelocity;
		animations.SetFloat("Forward", forwardVelocity);
		//animations.SetFloat("Turn", angularVelocity);

		//Vision
		
	}

	#endregion

	#region Methods
	public void CheckSight()
	{
		Vector3 ray = target - transform.position;
		if (!Physics.Raycast(transform.position, ray, ray.magnitude, ~LayerMask.GetMask("Default"))) // Check no obstacle between person and target
		{
			RaycastHit hit;
			if (Physics.Raycast(transform.position, ray, out hit, ray.magnitude) && hit.collider.tag == "Player") // Check if player is seen
				SeeSomething(target);
				//Line of sight
			else
				stateMachine.SetTrigger("Idle");
		}
	}

	public void SeeSomething(Vector3 pos)
	{
		// react to something being seen at the position pos
		Debug.Log(transform.name +  " sees something.");

		target = pos;
		Scream();
		stateMachine.SetTrigger("Seen");
	}

	public void HearSomething(Vector3 pos)
	{
		// react to something being heard at the position pos
		Debug.Log(transform.name +  " heard something.");
		target = pos;
		stateMachine.SetTrigger("Noise");
	}

	public void Scream()
	{
		Debug.Log(transform.name +  " : AAHH!");
		Collider[] colliders = Physics.OverlapSphere(transform.position, 10f, LayerMask.GetMask("Person"));
		foreach (Collider c in colliders)
		{
			if (c.transform == transform) // Don't scare yourself
				continue;

			PersonAI person = c.transform.GetComponent<PersonAI>();
			if (person)
				person.HearSomething(transform.position);
		}
	}

	#endregion
}
