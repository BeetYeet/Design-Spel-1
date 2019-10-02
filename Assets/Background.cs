using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
	public Transform player;
	public List<Transform> ye = new List<Transform>();
	public float extra = .8f;
	public float _ = .1f;
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		float __ = _;
		foreach (Transform t in ye)
		{
			t.localPosition = player.position * __;
			__ *= extra;
		}
	}
}
