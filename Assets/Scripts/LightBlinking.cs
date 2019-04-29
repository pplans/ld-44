using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBlinking : MonoBehaviour
{
	public float m_fBlinkingOffset = 0.1f;
	public float m_fBlinkingCycle = 1.0f;
	public float m_fBlinkingCycleMin = 0.2f;
	public float m_fBlinkingCycleMax = 5.0f;
	private Light m_light;
	private float m_originalIntensity;

	private void Start()
	{
		m_light = GetComponent<Light>();
		m_originalIntensity = m_light.intensity;
	}

	// Update is called once per frame
	void Update()
    {
		m_fBlinkingCycle -= Time.fixedDeltaTime;
		m_light.intensity = Mathf.Lerp(0.0f, m_originalIntensity, (m_fBlinkingCycleMax-m_fBlinkingCycle)/(m_fBlinkingCycleMax- m_fBlinkingCycleMin));
		m_light.enabled = !(m_fBlinkingCycle < -m_fBlinkingOffset);
		if (m_fBlinkingCycle< -m_fBlinkingOffset)
		{
			m_fBlinkingCycle = Random.Range(m_fBlinkingCycleMin, m_fBlinkingCycleMax);
		}
    }
}
