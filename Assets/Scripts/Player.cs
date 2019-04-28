﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject cam;

    public bool isAlive = true;
    public bool isInvisible = false;
    public bool isEatingPeople = false;
    public bool isSuspicious = false;

    float m_blood;
    const float m_max_blood = 100f;
	const float m_bloodPerSecond = 0.0f;//5f; // is static
    const float m_bloodPerSecondDuringInvisibility = 15f; // is static
	const float m_obfuscationTimerMax = 0.5f;
	private float m_obfuscationTimer = 1.0f;

	public float m_timeToEatOnePeople = 1f;
    public float m_distanceToEatPeople = 1f;
    public float m_bloodSpentToKillFromDistance = 40f;
    public float m_distanceToKillFromDistance = 10f;

    public Material m_uiBloodPoolMat;
	public Material m_uiObfuscationMat;
	public Material m_ObfuscationMat;
	private Material m_PlayerMat;
	public GameObject m_model;
	private Renderer m_modelRenderer;
	public ParticleSystem m_partSysBats;
	public ParticleSystem m_partSysPuff;
    public Animator m_animator;

    CapsuleCollider m_collider;


    private void Awake()
    {
        m_collider = this.gameObject.GetComponent<CapsuleCollider>();
    }

    public float Blood { get { return m_blood; } set { m_blood = System.Math.Min(value, m_max_blood); } }

	public void Start()
	{
		m_modelRenderer = m_model.GetComponent<Renderer>();
		m_PlayerMat = new Material(m_modelRenderer.material);
		m_partSysBats.Stop();
		m_partSysPuff.Stop();
	}

	public Player()
	{
		m_blood = 100.0f;
		m_obfuscationTimer = m_obfuscationTimerMax;
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
        this.gameObject.layer = 2;
        m_modelRenderer.material = m_ObfuscationMat;
		m_partSysBats.Play();
		m_partSysPuff.Play();
	}

    public void PlayerBecomeVisible()
    {
        Debug.Log("VISIBLE");
        isInvisible = false;
        isSuspicious = true;
        this.gameObject.layer = 9;
        m_modelRenderer.material = m_PlayerMat;
    }

    public void UpdatePlayer()
    {
        LossBloodOverTime();
		if (isInvisible)
		{
			//m_modelRenderer.material.Lerp(m_ObfuscationMat, m_PlayerMat, m_obfuscationTimer);
			m_obfuscationTimer = Mathf.Max(0.0f, m_obfuscationTimer - Time.fixedDeltaTime);
		}
		else
		{
			//m_modelRenderer.material.Lerp(m_ObfuscationMat, m_PlayerMat, m_obfuscationTimer);
			m_obfuscationTimer = Mathf.Min(m_obfuscationTimerMax, m_obfuscationTimer + Time.fixedDeltaTime);
		}
		m_uiBloodPoolMat.SetFloat("_Percent", Mathf.Clamp(m_blood / m_max_blood, 0.0f, 1.0f));
		m_uiObfuscationMat.SetFloat("_FlowAnim", Mathf.Clamp((m_obfuscationTimerMax - m_obfuscationTimer)/ m_obfuscationTimerMax, 0.0f, 1.0f));
	}
}
