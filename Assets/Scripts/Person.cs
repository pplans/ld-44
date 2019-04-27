using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
	#region Members

	protected bool m_isTarget;
	protected bool m_isAlive;
	protected bool m_isShouting;

	public Animator stateMachine;

	#endregion

	#region UnityEvents

	public void Awake()
	{
		m_isAlive = true;
		m_isTarget = false;
		m_isShouting = false;
	}

	public void Update()
	{
	}

	#endregion

	#region Methods

	public virtual bool IsInVision(Transform f)
	{
		//throw new System.NotImplementedException();
		return false;
	}

	#endregion
}
