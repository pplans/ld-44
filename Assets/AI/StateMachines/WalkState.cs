using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : StateMachineBehaviour
{
	private PersonAI personAI;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		personAI = animator.gameObject.GetComponent<PersonAI>();

		personAI.navMeshAgent.speed = 3f;

		personAI.animations.SetTrigger("StopAiming");
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		personAI.navMeshAgent.SetDestination(personAI.target);
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		personAI.navMeshAgent.speed = 0f;

		personAI.animations.ResetTrigger("StopAiming");
	}
}
