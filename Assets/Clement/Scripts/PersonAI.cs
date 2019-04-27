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
		float forwardVelocity = Vector3.Dot(navMeshAgent.velocity, navMeshAgent.transform.forward) / navMeshAgent.speed;
		//float angularVelocity = navMeshAgent.velocity.magnitude - forwardVelocity;
		animations.SetFloat("Forward", forwardVelocity);
		//animations.SetFloat("Turn", angularVelocity);
	}

	#endregion

	#region Methods

	public void SeeSomething(Vector3 pos)
	{
		// react to something being seen at the position pos
	}

	public void HearSomething(Vector3 pos)
	{
		// react to something being heard at the position T
	}

	#endregion
}
