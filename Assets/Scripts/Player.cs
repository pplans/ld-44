using System.Collections;
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
	const float m_obfuscationTimerMax = 1.0f;
	float m_obfuscationTimer = 1.0f;

	public float m_timeToEatOnePeople = 1f;
    public float m_distanceToEatPeople = 1f;

	public Material m_uiBloodPoolMat;
	public Material m_ObfuscationMat;
	private Material m_PlayerMat;
	public GameObject m_model;
	private Renderer m_modelRenderer;

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
		m_uiBloodPoolMat = new Material(m_uiBloodPoolMat);
	}

    public void PlayerBecomeInvisible()
    {
        Debug.Log("INVISIBLE");
        isInvisible = true;
        m_collider.enabled = false;
		m_modelRenderer.material = m_ObfuscationMat;

	}

    public void PlayerBecomeVisible()
    {
        Debug.Log("VISIBLE");
        isInvisible = false;
        m_collider.enabled = true;
		m_modelRenderer.material = m_PlayerMat;
	}

    public void UpdatePlayer()
    {
        LossBloodOverTime();
		m_uiBloodPoolMat.SetFloat("_Percent", Mathf.Clamp(m_blood / m_max_blood, 0.0f, 1.0f));

		/*
		 * DO NOT REMOVE
		 * if(isInvisible)
		{
			m_modelRenderer.material.Lerp(m_ObfuscationMat, m_PlayerMat, m_obfuscationTimer);
			m_obfuscationTimer = Mathf.Max(0.0f, m_obfuscationTimer - Time.fixedDeltaTime);
		}
		else if(Mathf.Abs(m_obfuscationTimer - m_obfuscationTimerMax)<0.0001f)
		{
			m_modelRenderer.material.Lerp(m_ObfuscationMat, m_PlayerMat, m_obfuscationTimer);
			m_obfuscationTimer = Mathf.Min(m_obfuscationTimerMax, m_obfuscationTimer + Time.fixedDeltaTime);
		}
		Debug.Log("Obfs Timer = "+m_obfuscationTimer);*/
	}
}
