using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spitball : MonoBehaviour
{
	float velocity = 0f;
	bool active;

	public void Initialize(float startvelocity)
	{
		velocity = startvelocity;
		active = true;
	}
	void Update()
	{
		Vector2 vel = transform.up * velocity;
		transform.position = transform.position + (Vector3)vel * Time.deltaTime;
		vel += Physics2D.gravity * Time.deltaTime;
		transform.rotation = Quaternion.LookRotation(vel, Vector3.back) * Quaternion.Euler(90f, 0f, 0f);
		velocity = vel.magnitude;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{

		if (active)
		{
			active = false;
			collision.gameObject.GetComponent<Moveenemy>().Damage();
		}
		Vector2 vel = Vector2.Reflect(velocity*transform.up, collision.contacts[0].normal);
		transform.rotation = Quaternion.LookRotation(vel, Vector3.back) * Quaternion.Euler(90f, 0f, 0f);
		velocity = vel.magnitude;
	}

}
