using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spitball : MonoBehaviour
{
	float velocity = 0f;

	public void Initialize(float startvelocity)
	{
		velocity = startvelocity;
	}
	void FixedUpdate()
	{
		Vector2 vel = transform.up * velocity;
		transform.position = transform.position + (Vector3) vel * Time.fixedDeltaTime;
		vel += Physics2D.gravity * Time.fixedDeltaTime;
		transform.rotation = Quaternion.LookRotation(vel, Vector3.back) * Quaternion.Euler(90f, 0f, 0f);
		velocity = vel.magnitude;
	}
}
