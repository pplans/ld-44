﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : StateMachineBehaviour
{
	private PersonAI personAI;
	
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		personAI = animator.gameObject.GetComponent<PersonAI>();

		personAI.animations.SetTrigger("StartShouting");
		personAI.animations.SetTrigger("StopShouting");

		personAI.navMeshAgent.speed = 5f;

		personAI.alerted.SetActive(true);
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Vector3 fleeVector = animator.transform.position - personAI.target;
		fleeVector = fleeVector.normalized * 2f;
		personAI.navMeshAgent.SetDestination(animator.transform.position + fleeVector);
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		personAI.animations.ResetTrigger("StartShouting");
		personAI.animations.ResetTrigger("StopShouting");

		personAI.navMeshAgent.speed = 0f;
		personAI.alerted.SetActive(false);
	}
}
