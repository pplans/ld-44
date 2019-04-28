﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject cam;

    public bool isAlive = true;
    public bool isEatingPeople = false;

    float m_blood;
    const float m_max_blood = 100f;
    const float m_bloodPerSecond = 5f; // is static

    public float m_timeToEatOnePeople = 1f;
    public float m_distanceToEatPeople = 1f;

    public float Blood { get { return m_blood; } set { m_blood = System.Math.Min(value, m_max_blood); } }

    public Player()
	{
		m_blood = 100.0f;
	}

    void LossBloodOverTime() {
        if (!isEatingPeople)
        {
            m_blood -= m_bloodPerSecond * Time.deltaTime;
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

    public void UpdatePlayer()
    {
        LossBloodOverTime();
    }

    // Update is called once per frame
    public void Update()
    { 

    }
}
