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
	

	Rigidbody2D rbody;
	AudioSource AudioData;
	SpriteRenderer SR;

	public static PlayerMovement i
	{
		get; private set;
	}

    public GroundState groundState;

    public float Speed;
    public bool UseCustomGravity;
    public bool FlipSpriteOnMovement;

    public float JumpForce;
    public bool UseJumpRotation;
    public float JumpRotation;
    public float JumpRotationMuliplyer;
    public bool snaprotation;
    public float SmoothRotation;


    public bool UseDoubleJump = true;
    public int doubleJumps;
    public int AirJumpsLeft;

 

    
    public Sprite WalkingSprite;
    public Sprite JumpSprite;

    
    public int Jumps;

    
    public string groundTag;
    #endregion

    void OnValidate()
	{
		rbody = GetComponent<Rigidbody2D>();
		AudioData = GetComponent<AudioSource>();
		SR = GetComponent<SpriteRenderer>();
		switch (movementType)
		{
			case MovementType.Platformer:
				if (UseDoubleJump == false)
				{
					doubleJumps = 0;
				}
				if (UseJumpRotation == false)
				{
					JumpRotation = 0f;
					JumpRotationMuliplyer = 0f;
				}
				if (UseCustomGravity == false)
				{
					rbody.gravityScale = 5f;
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
		
		switch (movementType)
		{
			case MovementType.Platformer:
				Movement();
				break;
			case MovementType.TopDown:
				
				break;
		}
        Enums();
    }

	#region PlatformStuff

	void Movement()
	{
		rbody.velocity = new Vector2(Input.GetAxis("Horizontal") * Speed, rbody.velocity.y);
		switch (movingDirection)
		{
			case MovingDirection.Left:
				if (FlipSpriteOnMovement == true)
					SR.flipX = true;
				break;
			case MovingDirection.Right:
				if (FlipSpriteOnMovement == true)
					SR.flipX = false;
				break;
		}
		Jump();

	}

	void Jump()
	{
		Vector2 jumpForce = new Vector2(rbody.velocity.x, JumpForce);

		switch (groundState)
		{
			case GroundState.onGround:
			    Jumps = doubleJumps;
				if (Input.GetButtonDown("Jump"))
				{
					rbody.velocity = jumpForce;
					switch (movingDirection)
					{

						case MovingDirection.Right:
							rbody.AddTorque(-JumpRotation);
							break;
						case MovingDirection.Left:
							rbody.AddTorque(JumpRotation);
							break;
					}
					
				}
				#region SnapRot
				if (snaprotation == true)
				{
					Quaternion Target = Quaternion.Euler(0, 0, 0);

					if (transform.rotation.z == 0)
						return;
					else
						transform.rotation = Quaternion.Slerp(transform.rotation, Target, Time.deltaTime * SmoothRotation);
				}
				SR.sprite = WalkingSprite;
				#endregion
				break;


			case GroundState.InAir:
				if (Input.GetButtonDown("Jump") && Jumps != 0)
				{
					rbody.velocity = jumpForce / 1.1f;
					switch (movingDirection)
					{
						case MovingDirection.Right:
							rbody.AddTorque(JumpRotation * -JumpRotationMuliplyer);
							break;
						case MovingDirection.Left:
							rbody.AddTorque(JumpRotation * JumpRotationMuliplyer);
							break;
					}
					
					Jumps--;
				}
				SR.sprite = JumpSprite;
				break;
		}

		AirJumpsLeft = Jumps;
	}


	#region OnTrigger
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == groundTag)
		{
			groundState = GroundState.onGround;
			rbody.velocity /= 5;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == groundTag)
		{

			groundState = GroundState.InAir;
		}
	}
	#endregion OnTrigger

	#endregion



	void Enums()
	{
        if (rbody.velocity.x == 0)
            return;

		if (rbody.velocity.x < -0.1)
		{
			movingDirection = MovingDirection.Left;
		}
		if (rbody.velocity.x > 0.1)
			movingDirection = MovingDirection.Right;
		switch (movementType)
		{
			case MovementType.TopDown:

				if (rbody.velocity.y < 0.1f)
				{
					movingDirection = MovingDirection.down;
				}
				if (rbody.velocity.y > -0.1f)
					movingDirection = MovingDirection.up;

				break;
		}

		switch (groundState)
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

