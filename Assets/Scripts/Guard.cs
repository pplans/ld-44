﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : Person
{
	#region UnityEvents

	public void Start()
	{
		stateMachine.SetTrigger("Walk");
	}

	#endregion

	#region Methods

	public override bool IsInVision(Transform f)
	{
		// @TODO
		throw new System.NotImplementedException();
	}

	#endregion
}
