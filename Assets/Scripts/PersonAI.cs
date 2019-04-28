using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PersonAI : MonoBehaviour
{
	#region Members

	public const float visionDepth = 10f;
	public const float visionConeAngle = 40;
	public const int visionRaysCount = 10;
	
	public Animator animations;
	public NavMeshAgent navMeshAgent;
	public Animator stateMachine;

	public GameObject alerted;
	public GameObject suspicious;
	public GameObject stun;

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
		CheckSight();
	}

	public void OnDrawGizmos()
	{
		//Debug.DrawLine(transform.position + Vector3.up, target);

		//float angleStart = -visionConeAngle / 2;
		//float angleStep = visionConeAngle / visionRaysCount;
		//for (int r = 0 ; r < visionRaysCount ; ++r)
		//{
		//	Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + Quaternion.AngleAxis(angleStart + r * angleStep, Vector3.up) * transform.forward * visionDepth);
		//}
	}

	#endregion

	#region Methods

	public void CheckSight()
	{
		float angleStart = -visionConeAngle / 2;
		float angleStep = visionConeAngle / visionRaysCount;
		for (int r = 0 ; r <= visionRaysCount ; ++r)
		{
			Ray ray = new Ray(transform.position + Vector3.up, Quaternion.AngleAxis(angleStart + r * angleStep, Vector3.up) * transform.forward);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, visionDepth, ~LayerMask.GetMask("Ignore Raycast")))
			{
				Debug.DrawLine(ray.origin, hit.point, Color.red);
				if (hit.transform.tag == "Player")
				{
					SeeSomething(hit.transform.position);
					return;
				}
			}
			else if (r == 0 || r == visionRaysCount)
				Debug.DrawRay(ray.origin, ray.direction * visionDepth, Color.red);
		}

	}

	public void SeeSomething(Vector3 pos, bool scary = false)
	{
		// react to something being seen at the position pos
		//Debug.Log(transform.name +  " sees something.");

		Scream();
		target = pos;
		target.y = 1f;
		stateMachine.SetTrigger("Seen");
	}

	public void HearSomething(Vector3 pos)
	{
		// react to something being heard at the position pos
		//Debug.Log(transform.name +  " heard something.");
		target = pos;
		target.y = 1f;
		stateMachine.SetTrigger("Noise");
	}

	public void Scream()
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, 8f, LayerMask.GetMask("Person"));
		foreach (Collider c in colliders)
		{
			if (c.transform == transform) // Don't scare yourself
				continue;

			PersonAI person = c.transform.GetComponent<PersonAI>();
			if (person)
				person.HearSomething(transform.position);
		}
	}

	#endregion
}
