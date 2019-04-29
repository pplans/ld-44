﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public Animator animations;
	public Rigidbody rigidbody;
	private Vector2 inputDirection;
    public bool canMove = false;

	public void FixedUpdate()
	{
        if (canMove)
        {
            inputDirection = new Vector2(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"));
            float magnitude = Mathf.Min(inputDirection.magnitude, 1f);
            inputDirection.Normalize();

            Vector3 move = magnitude * transform.forward * 5f;

            //Synchronize motion
            float forwardVelocity = move.magnitude;
            animations.SetFloat("Forward", forwardVelocity);

            if (inputDirection.magnitude < 0.2f)
                return;

            transform.rotation = Quaternion.LookRotation(new Vector3(inputDirection.x, 0, inputDirection.y), Vector3.up);

            rigidbody.MovePosition(transform.position + move * Time.deltaTime);
        }
	}
}
