using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
	#region Members

	protected bool m_isTarget;
	protected bool m_isAlive;
	protected bool m_isShouting;


    public float m_blood;

	public PersonAI personAI;
	public GameObject targetMark;

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

    public void UpdatePerson()
    {
       ;
    }

    public bool GetIsAlive()
    {
        return m_isAlive;
    }

    public void Die()
    {
        Debug.Log("Person is dead");
        m_isAlive = false;
		personAI.stateMachine.SetTrigger("Die");
    }

    public void Lock()
    {
        Debug.Log("Targeted");
    }

	public virtual bool IsInVision(Transform f)
	{
		//throw new System.NotImplementedException();
		return false;
	}

	#endregion
}
