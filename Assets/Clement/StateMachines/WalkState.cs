using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkState : StateMachineBehaviour
{
	private NavMeshAgent navMeshAgent;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		navMeshAgent = animator.gameObject.GetComponent<NavMeshAgent>();
		if (navMeshAgent == null)
			throw new System.NotImplementedException();

		navMeshAgent.destination = new Vector3(-5, 0, 0);
	}
}
