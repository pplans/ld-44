using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : StateMachineBehaviour
{
	private PersonAI personAI;

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		personAI = animator.gameObject.GetComponent<PersonAI>();

		Vector3 fleeVector = animator.transform.position - personAI.target;
		fleeVector = fleeVector.normalized * 2f;
		personAI.navMeshAgent.SetDestination(animator.transform.position + fleeVector);
	}
}
