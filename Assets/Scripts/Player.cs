using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	float m_blood;
	const float m_bloodPerSecond = 0.2f; // is static

    public Player()
	{
		m_blood = 100.0f;
	}

    void LossBloddOverTime() {
        m_blood -= m_bloodPerSecond*Time.deltaTime;
    }

    // Update is called once per frame
    public void Update()
    {
        
    }
}
