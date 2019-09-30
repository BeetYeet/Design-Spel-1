using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMOvement : MonoBehaviour
{


    public float Speed;
    public float JumpForce;
    public float JumpRotation;
    public float JumpRotationMuliplyer;

    public bool UseDoubleJump = true;
    public int doubleJumps;

    private int AirJumpsLeft;

    int Jumps;

    Rigidbody2D rbody;
    AudioSource AudioData;
    SpriteRenderer SR;


    public Sprite WalkingSprite;
    public Sprite JumpSprite;
    public MovingDirection movingDirection;
    public GroundState groundState;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        rbody.velocity = new Vector2(Input.GetAxis("Horizontal") * Speed, rbody.velocity.y);
        Vector2 jumpForce = new Vector2(rbody.velocity.x, JumpForce);
        switch (groundState)
        {
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
            case GroundState.onGround:
                Quaternion Target = Quaternion.Euler(0, 0, 0);

                if (transform.rotation.z == 0)
                    return;
                else
                    transform.rotation = Quaternion.Slerp(transform.rotation, Target, Time.deltaTime * 10000);


                SR.sprite = WalkingSprite;
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
                break;
        }
       

        if (rbody.velocity.x == 0)
            return;

        if (rbody.velocity.x < -0.1)
        {
            movingDirection = MovingDirection.Left;
            SR.flipX = true;
        }
        if (rbody.velocity.x > 0.1)
        {
            movingDirection = MovingDirection.Right;
            SR.flipX = false;
        }

      
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            groundState = GroundState.onGround;
            rbody.velocity /= 5;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {

            groundState = GroundState.InAir;
        }
    }
    public enum MovingDirection
    {
        Left,
        Right
    }
    public enum GroundState
    {
        InAir,
        onGround
    }
}
