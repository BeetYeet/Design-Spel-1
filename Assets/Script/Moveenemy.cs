using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveenemy : MonoBehaviour
{
	public bool dirRight = true;
	public float speed = 2.0f;
	public float leftblock;
	public float rightblock;

	public int health = 20;

	public AudioClip steveOof;
	public AudioClip damageSound;
	AudioSource source;

	private void Start()
	{
		source = GetComponent<AudioSource>();
	}
	void Update()
	{
		if (dirRight)
			transform.Translate(Vector2.right * speed * Time.deltaTime);
		else
			transform.Translate(-Vector2.right * speed * Time.deltaTime);

		if (transform.position.x >= rightblock)
		{
			dirRight = false;
		}

		if (transform.position.x <= leftblock)
		{
			dirRight = true;
		}
		if (Input.GetKeyDown(KeyCode.B))
		{
			Damage();
		}
	}

	public void Damage()
	{
		health--;
		if (health <= 0)
			source.clip = damageSound;
		else
			source.clip = steveOof;
		source.Play();
	}
}
