﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class PersonAI : MonoBehaviour
{
	#region Members

	public const float visionDepth = 10f;
	public const float visionConeAngle = 80f;
	public const int visionRaysCount = 20;
	
	public Animator animations;
	public NavMeshAgent navMeshAgent;
	public Animator stateMachine;

	public MeshFilter visionMesh;
	public LineRenderer shootTrail;
	private float shootTimer = 0f;

	public GameObject alerted;
	public GameObject suspicious;
	public GameObject stun;

	public Vector3 target;
    public bool isaguard;

	#endregion

	#region UnityEvents

	public void Start()
	{
		//Generate VisionMesh
		int verticesCount = visionRaysCount + 2;
		List<Vector3> vertices = new List<Vector3>();
		int[] triangles = new int[verticesCount * 3 - 6];
		
		for (int i = 0 ; i < visionRaysCount ; ++i)
		{
			vertices.Add(Vector3.zero);
			triangles[i * 3] = i;
			triangles[i * 3 + 1] = i + 1;
			triangles[i * 3 + 2] = verticesCount - 1;
		}
		vertices.Add(Vector3.zero);
		vertices.Add(Vector3.zero);

		visionMesh.mesh = new Mesh();
		visionMesh.mesh.SetVertices(vertices);
		visionMesh.mesh.SetTriangles(triangles, 0);
	}

	public void Update()
	{
		//Synchronize motion
		float forwardVelocity = navMeshAgent.velocity.magnitude;
		//float angularVelocity = navMeshAgent.velocity.magnitude - forwardVelocity;
		animations.SetFloat("Forward", forwardVelocity);
		//animations.SetFloat("Turn", angularVelocity);

		//Vision
		CheckSight();

		if (shootTrail.gameObject.activeSelf)
		{
			shootTimer -= Time.deltaTime;
			if (shootTimer <= 0f)
				shootTrail.gameObject.SetActive(false);
		}
	}

	public void OnDrawGizmosSelected()
	{
		float angleStart = -visionConeAngle / 2;
		float angleStop = -angleStart;

		Ray ray = new Ray(transform.position + Vector3.up, Quaternion.AngleAxis(angleStart, Vector3.up) * transform.forward);
		Debug.DrawRay(ray.origin, ray.direction * visionDepth, Color.red);

		ray = new Ray(transform.position + Vector3.up, Quaternion.AngleAxis(angleStop, Vector3.up) * transform.forward);
		Debug.DrawRay(ray.origin, ray.direction * visionDepth, Color.red);
	}

	#endregion

	#region Methods

	public void CheckSight()
	{
		List<Vector3> vertices = new List<Vector3>();
		visionMesh.mesh.GetVertices(vertices);

		float angleStart = -visionConeAngle / 2;
		float angleStep = visionConeAngle / visionRaysCount;
		for (int r = 0 ; r <= visionRaysCount ; ++r)
		{
			Ray ray = new Ray(transform.position + Vector3.up, Quaternion.AngleAxis(angleStart + r * angleStep, Vector3.up) * transform.forward);
			vertices[r] = transform.worldToLocalMatrix * ray.direction;
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, visionDepth, ~LayerMask.GetMask("Ignore Raycast")))
			{
				Debug.DrawLine(ray.origin, hit.point, Color.red);
				vertices[r] *= hit.distance;
				if (hit.transform.tag == "Player" && (isaguard || hit.transform.GetComponent<Player>().isSuspicious))
				{
					SeeSomething(hit.transform.position);
					//return;
				}
			}
			else// if (r == 0 || r == visionRaysCount)
			{
				vertices[r] *= visionDepth;
				Debug.DrawRay(ray.origin, ray.direction * visionDepth, Color.red);
			}
		}

		visionMesh.mesh.SetVertices(vertices);
		visionMesh.mesh.RecalculateBounds();
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

	public void Shoot()
	{
		shootTrail.gameObject.SetActive(true);
		shootTimer = 0.2f;

		shootTrail.SetPosition(0, transform.position + Vector3.up);
		shootTrail.SetPosition(1, target + Vector3.up);

		(Game.instance as GameImpl).m_player.HitPlayer(35f);
	}

	#endregion
}
