using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
	// variables
	public Transform firePoint;
	public Vector2 lastAimDir { get; private set; }
	public GameObject spitball;

	[Range(-1f, 1.5f)]
	public float shootSpeed;
	public float randangle = .1f;
	public float randspeed = 3f;
	public int spitballs = 10;


	public float shootCooldown = 1f;
	float cooldownNow = 0f;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		lastAimDir = GetFireDir();
		if (cooldownNow > 0f)
		{
			cooldownNow -= Time.deltaTime;
		}
		else
		{
			cooldownNow = 0f;

			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				cooldownNow = shootCooldown;
				for (int i = 0; i < spitballs; i++)
				{
					float angle = Random.Range(-randangle, randangle);
					float sin = Mathf.Sin(angle);
					float cos = Mathf.Cos(angle);
					Shoot(new Vector2(lastAimDir.x * cos - lastAimDir.y * sin, lastAimDir.x * sin + lastAimDir.y * cos), Mathf.Pow(10, shootSpeed) + Random.Range(-randspeed, randspeed));
				}
			}
		}
	}

	private void Shoot(Vector2 dir, float speed)
	{
		Instantiate(spitball, firePoint.position, Quaternion.LookRotation(dir.normalized, Vector3.back) * Quaternion.Euler(90f, 0f, 0f)).GetComponent<Spitball>().Initialize(speed);
	}

	private Vector2 GetFireDir()
	{
		Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousepos.z = 0f;

		return (mousepos - firePoint.position);
	}
}
