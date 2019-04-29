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
	public GameObject m_deathMark;

	public GameObject m_poolOfBlood;
	private float m_poolOfBloodAmount;

	#endregion

	#region UnityEvents

	public void Awake()
	{
		m_isAlive = true;
		m_isTarget = false;
		m_isShouting = false;
		m_poolOfBloodAmount = 0.0f;
	}

	public void Update()
	{
	}

    #endregion

    #region Methods

    public void UpdatePerson()
    {
       if(!m_isAlive)
	   {
			m_poolOfBloodAmount = Mathf.Clamp(m_poolOfBloodAmount+Time.deltaTime * 0.1f, 0.0f, 1.0f);
			m_poolOfBlood.GetComponent<Renderer>().material.SetFloat("_Amount", m_poolOfBloodAmount);
	   }
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
		personAI.visionMesh.GetComponent<Renderer>().enabled = false;
        personAI.gameObject.GetComponent<Collider>().enabled = false;
		m_poolOfBlood.SetActive(true);
	}

    public void Lock()
    {
		m_deathMark.SetActive(true);
    }

    public void UnLock()
	{
		m_deathMark.SetActive(false);
	}

    public virtual bool IsInVision(Transform f)
	{
		//throw new System.NotImplementedException();
		return false;
	}
    public virtual void StartBeingAttacked()
    {
        personAI.animations.SetTrigger("StartBeingAttacked");
    }
    public virtual void StopBeingAttacked()
    {
        personAI.animations.SetTrigger("StopBeingAttacked");
    }
    #endregion
}
