using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Person
{
	protected bool m_isTarget;
	protected bool m_isAlive;
	protected bool m_isShouting;

	private Transform m_model;

    // Start is called before the first frame update
    public Person()
    {
		m_isAlive = true;
		m_isTarget = false;
		m_isShouting = false;
    }

	public abstract bool IsInVision(Transform f);

    // Update is called once per frame
    public abstract void Update();
}
