using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : StateMachineBehaviour
{
	private PersonAI personAI;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		personAI = animator.gameObject.GetComponent<PersonAI>();

		personAI.navMeshAgent.speed = 0;

		personAI.animations.SetTrigger("Die");
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		personAI.navMeshAgent.speed = 0;
	}
}
