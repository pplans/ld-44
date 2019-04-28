﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject cam;

    public bool isAlive = true;
    public bool isInvisible = false;
    public bool isEatingPeople = false;

    float m_blood;
    const float m_max_blood = 100f;
    const float m_bloodPerSecond = 5f; // is static
    const float m_bloodPerSecondDuringInvisibility = 15f; // is static

    public float m_timeToEatOnePeople = 1f;
    public float m_distanceToEatPeople = 1f;

	public Material m_uiBloodPoolMat;
    CapsuleCollider m_collider;


    private void Awake()
    {
        m_collider = this.gameObject.GetComponent<CapsuleCollider>();
    }

    public float Blood { get { return m_blood; } set { m_blood = System.Math.Min(value, m_max_blood); } }

    public Player()
	{
		m_blood = 100.0f;
	}

    void LossBloodOverTime() {
        if (!isEatingPeople & !isInvisible)
        {
            m_blood -= m_bloodPerSecond * Time.deltaTime;
        }
        else if (isInvisible)
        {
            m_blood -= m_bloodPerSecondDuringInvisibility * Time.deltaTime;
        }

        //Debug.Log("Blood : " +m_blood);

        if (m_blood <= 0)
        {
            PlayerDies();
        }
    }

    void PlayerDies()
    {
        Debug.Log("Player is dead");
        isAlive = false;
    }

    public void PlayerBecomeInvisible()
    {
        Debug.Log("INVISIBLE");
        isInvisible = true;
        m_collider.enabled = false;
    }

    public void PlayerBecomeVisible()
    {
        Debug.Log("VISIBLE");
        isInvisible = false;
        m_collider.enabled = true;
    }

    public void UpdatePlayer()
    {
        LossBloodOverTime();
		m_uiBloodPoolMat.SetFloat("_Percent", Mathf.Clamp(m_blood / m_max_blood, 0.0f, 1.0f));
	}
}
