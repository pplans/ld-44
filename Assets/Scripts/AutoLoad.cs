﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLoad : MonoBehaviour
{
	private float timer;
	public LoadLevel loadLevel;

	// Start is called before the first frame update
	void Start()
	{
		timer = 3f;
	}

	// Update is called once per frame
	void Update()
	{
		timer -= Time.deltaTime;
		if (timer <= 0)
			loadLevel.LoadLevelX(0);
	}
}
