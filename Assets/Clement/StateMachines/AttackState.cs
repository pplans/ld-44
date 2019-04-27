using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState :  StateMachineBehaviour
{
	private PersonAI personAI;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		personAI = animator.gameObject.GetComponent<PersonAI>();

		personAI.navMeshAgent.speed = 3f;
		personAI.navMeshAgent.SetDestination(personAI.target);
	}
}
