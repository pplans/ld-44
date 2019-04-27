using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	private Animator m_animator;

	private void Start()
	{
		m_animator = GetComponent<Animator>();
		m_animator.enabled = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if(!m_animator.enabled)
		{
			m_animator.enabled = true;
			m_animator.Play("OpenSingleDoor");
			Debug.Log("Play OpenSingleDoor");
		}
	}

	private void OnTriggerExit(Collider other)
	{
		m_animator.enabled = false;
	}
}
