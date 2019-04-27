using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
	public PersonAI[] persons;

	void Start()
	{
		foreach (PersonAI p in persons)
		{
			p.SeeSomething(transform.position);
		}
	}
}
