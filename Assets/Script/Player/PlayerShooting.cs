using System;
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

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		lastAimDir = GetFireDir();

		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			Instantiate(spitball, firePoint.position, Quaternion.LookRotation(lastAimDir, Vector3.back) * Quaternion.Euler(90f, 0f, 0f)).GetComponent<Spitball>().Initialize(Mathf.Pow(10, shootSpeed));
		}
	}

	private Vector2 GetFireDir()
	{
		Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousepos.z = 0f;

		return (mousepos - firePoint.position);
	}
}
