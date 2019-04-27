using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody rigidbody;
	private Vector2 inputDirection;

	public void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
	}

	public void Update()
	{
		inputDirection = new Vector2(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"));
		inputDirection.Normalize();
		

		if (inputDirection.magnitude < 0.2f)
			return;

		transform.rotation = Quaternion.LookRotation(new Vector3(inputDirection.x, 0, inputDirection.y), Vector3.up);

		rigidbody.MovePosition(transform.position + transform.forward * 5f * Time.deltaTime);
	}
}
