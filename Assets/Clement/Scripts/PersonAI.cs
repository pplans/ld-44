﻿using System.Collections;
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

	public void SeeSomething(Vector3 pos)
	{
		target = pos;
		Scream();
		// react to something being seen at the position pos
	}

	public void HearSomething(Vector3 pos)
	{
		// react to something being heard at the position pos
		target = pos;
		stateMachine.SetTrigger("Noise");
	}

	public void Scream()
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, 10f, LayerMask.GetMask("Person"));
		foreach (Collider c in colliders)
		{
			PersonAI person = c.GetComponent<PersonAI>();
			if (person)
				person.HearSomething(transform.position);
		}
	}

	#endregion
}
