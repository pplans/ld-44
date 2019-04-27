using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBlinking : MonoBehaviour
{

	public bool m_bIsBlinking = true;
	public float m_fBlinkingOffset = 0.1f;
	public float m_fBlinkingCycle = 1.0f;
	public float m_fBlinkingCycleMin = 0.2f;
	public float m_fBlinkingCycleMax = 1.0f;
	private Light m_light;

	private void Start()
	{
		m_light = GetComponent<Light>();
	}

	// Update is called once per frame
	void Update()
    {
        if(m_bIsBlinking)
		{
			m_fBlinkingCycle -= Time.fixedDeltaTime;
			m_light.enabled = !(m_fBlinkingCycle < -m_fBlinkingOffset);
			if (m_fBlinkingCycle< -m_fBlinkingOffset)
			{
				m_fBlinkingCycle = Random.Range(m_fBlinkingCycleMin, m_fBlinkingCycleMax);
			}
		}
    }
}
