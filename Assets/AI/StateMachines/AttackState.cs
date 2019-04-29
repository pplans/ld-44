using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState :  StateMachineBehaviour
{
	private PersonAI personAI;
	Transform playerTransform;
	private float shootDelay;
	const float shootInterval = 1f;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		personAI = animator.gameObject.GetComponent<PersonAI>();
		playerTransform = (GameImpl.instance as GameImpl).m_player.transform;
		personAI.target = playerTransform.position;

		personAI.animations.ResetTrigger("StopAiming");
		personAI.animations.SetTrigger("StartAiming");
		shootDelay = shootInterval;

		personAI.alerted.SetActive(true);
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		personAI.target = playerTransform.position;
		Vector3 lookAtPlayer = playerTransform.position - personAI.transform.position;

		RaycastHit hit;
		if (!Physics.Raycast(animator.transform.position + Vector3.up, lookAtPlayer, out hit, PersonAI.visionDepth, ~LayerMask.GetMask("Ignore Raycast"))
			|| (hit.collider.tag != "Player"))
		{
			//Line of sight is broken, switch to walk state with last seen position as target
			personAI.stateMachine.SetTrigger("Idle");
			personAI.stateMachine.SetTrigger("Noise");
			personAI.HearSomething(playerTransform.position);
			return;
		}

		personAI.transform.rotation = Quaternion.LookRotation(lookAtPlayer, Vector3.up);

		shootDelay -= Time.deltaTime;
		if (shootDelay <= 0f)
		{
			Debug.Log(animator.transform.name + " Bang!");
			personAI.Shoot();
			shootDelay = shootInterval;
		}
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		personAI.animations.ResetTrigger("StartAiming");
		personAI.animations.SetTrigger("StopAiming");

		personAI.alerted.SetActive(false);
	}
}
