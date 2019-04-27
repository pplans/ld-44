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
		Debug.Log(animator.transform.name + " Bang!");
	}
	
	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		personAI.navMeshAgent.SetDestination(personAI.target);
	}
}
