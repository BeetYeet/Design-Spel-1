using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour
{
	#region vars
	public MovementType movementType;
	[Space]
	public MovementState movementState;
	public MovingDirection movingDirection;
	[Space]
	public PlatformVars platformVaribles;
	public TopDownVars TopdownVaribles;

	Rigidbody2D rbody;
    AudioSource AudioData;

	public static PlayerMovement i
	{
		get; private set;
	}
	#endregion

	void OnValidate()
	{
		rbody = GetComponent<Rigidbody2D>();
		switch (movementType)
		{
			case MovementType.Platformer:
				if (platformVaribles.UseDoubleJump == false)
				{
					platformVaribles.doubleJumps = 0;
				}
				if (platformVaribles.UseJumpRotation == false)
				{
					platformVaribles.JumpRotation = 0f;
					platformVaribles.JumpRotationMuliplyer = 0f;
				}
				if (platformVaribles.UseCustomGravity == false)
				{
					rbody.gravityScale = 5f;
				}
				break;

			case MovementType.TopDown:
				if (TopdownVaribles.SameSpeedOnXAndY == true)
				{
					TopdownVaribles.YSpeed = TopdownVaribles.XSpeed;
				}
				break;
		}
	}

	void Start()
	{
		i = this;
		switch (movementType)
		{
			case MovementType.Platformer:
				rbody.bodyType = RigidbodyType2D.Dynamic;

				break;
			case MovementType.TopDown:
				rbody.bodyType = RigidbodyType2D.Kinematic;
				break;
		}
	}

	void Update()
	{
		Enums();
		switch (movementType)
		{
			case MovementType.Platformer:
				Movement();
				break;
			case MovementType.TopDown:
				TopDownMovement();
				break;
		}
	}

	#region PlatformStuff

	void Movement()
	{
		rbody.velocity = new Vector2(Input.GetAxis("Horizontal") * platformVaribles.Speed, rbody.velocity.y);
		Jump();
	}

	void Jump()
	{
		Vector2 jumpForce = new Vector2(rbody.velocity.x, platformVaribles.JumpForce);

		switch (platformVaribles.groundState)
		{
			case GroundState.onGround:
                platformVaribles.Jumps = platformVaribles.doubleJumps;
				if (Input.GetButtonDown("Jump"))
				{
					rbody.velocity = jumpForce;
					switch (movingDirection)
					{
						case MovingDirection.Right:
							rbody.AddTorque(-platformVaribles.JumpRotation);
							break;
						case MovingDirection.Left:
							rbody.AddTorque(platformVaribles.JumpRotation);
							break;
					}
                    AudioData.PlayOneShot(platformVaribles.JumpSound);
				}
                if(platformVaribles.snaprotation == true)
                {
                    Quaternion Target = Quaternion.Euler(0, 0, 0);

                    transform.rotation = Quaternion.Slerp(transform.rotation, Target, Time.deltaTime * platformVaribles.SmoothRotation);
                }
				break;

			case GroundState.InAir:
				if (Input.GetButtonDown("Jump") && platformVaribles.Jumps != 0)
				{
					rbody.velocity = jumpForce / 1.1f;
					switch (movingDirection)
					{
						case MovingDirection.Right:
							rbody.AddTorque(platformVaribles.JumpRotation * -platformVaribles.JumpRotationMuliplyer);
							break;
						case MovingDirection.Left:
							rbody.AddTorque(platformVaribles.JumpRotation * platformVaribles.JumpRotationMuliplyer);
							break;
					}
                    AudioData.PlayOneShot(platformVaribles.JumpSound);
                    platformVaribles.Jumps--;
				}
				break;
		}

		platformVaribles.AirJumpsLeft = platformVaribles.Jumps;
	}

	#region OnTrigger
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == platformVaribles.groundTag)
		{
			platformVaribles.groundState = GroundState.onGround;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == platformVaribles.groundTag)
		{

			platformVaribles.groundState = GroundState.InAir;
		}
	}
	#endregion OnTrigger

	#endregion

	#region TopdownStuff

	void TopDownMovement()
	{
		if (TopdownVaribles.SmoothMovement == true)
			rbody.velocity = new Vector2(Input.GetAxis("Horizontal") * TopdownVaribles.XSpeed, Input.GetAxis("Vertical") * TopdownVaribles.YSpeed);
		if (TopdownVaribles.SmoothMovement == false)
			rbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * TopdownVaribles.XSpeed, Input.GetAxisRaw("Vertical") * TopdownVaribles.YSpeed);

	}

	#endregion

	void Enums()
	{
		if ((rbody.velocity.x < 0 || -rbody.velocity.x < 0) && platformVaribles.groundState == GroundState.onGround)
			movementState = MovementState.Moving;
		else
			movementState = MovementState.Idle;

		if (rbody.velocity.x < 0)
		{
			movingDirection = MovingDirection.Left;
		}
		if (-rbody.velocity.x < 0)
			movingDirection = MovingDirection.Right;
		switch (movementType)
		{
			case MovementType.TopDown:

				if (rbody.velocity.y < 0)
				{
					movingDirection = MovingDirection.down;
				}
				if (-rbody.velocity.y < 0)
					movingDirection = MovingDirection.up;

				break;
		}

		switch (platformVaribles.groundState)
		{
			case GroundState.InAir:
				movementState = MovementState.Jumping;
				break;
		}
	}
}


#region Enums
public enum MovementType
{
	Platformer,
	TopDown
}

public enum MovementState
{
	Idle,
	Moving,
	Jumping
}

public enum GroundState
{
	InAir,
	onGround
}

public enum MovingDirection
{
	Left,
	Right,
	up,
	down
}
#endregion

[System.Serializable]
public class PlatformVars
{
	[Space]
	[Header("States")]
	[Space]
	[Header("Remember to have a ground trigger collider!")]
	public GroundState groundState;

	[Header("Movement")]
	[Space]
	public float Speed;
	public bool UseCustomGravity;

	[Header("Jump values")]
	public float JumpForce;
	public bool UseJumpRotation;
	public float JumpRotation;
	public float JumpRotationMuliplyer;
    public bool snaprotation;
    public float SmoothRotation;

    [Header("double jump")]
	[Space]
	public bool UseDoubleJump = true;
	public int doubleJumps;
    public int AirJumpsLeft;

    [Header("Sounds")]
    [Space]
    public AudioClip JumpSound;
    public AudioClip walkingSound;

	[HideInInspector]
	public int Jumps;

	[Header("string")]
	[Space]
	public string groundTag;
}

[System.Serializable]
public class TopDownVars
{
	[Header("Movement speed")]
	public bool SmoothMovement = true;
	[Space]
	public bool SameSpeedOnXAndY;
	public float XSpeed, YSpeed;
}