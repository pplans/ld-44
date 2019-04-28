using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : StateMachineBehaviour
{
	private PersonAI personAI;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		personAI = animator.gameObject.GetComponent<PersonAI>();

		personAI.animations.SetTrigger("Die");
		personAI.enabled = false;
	}
}
