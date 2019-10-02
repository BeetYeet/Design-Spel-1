using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{ 

    AudioSource audioSrc;
    bool isMoving = false;
    Rigidbody2D rb;
    public PlayerMovement pm;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        isMoving = pm.movementState == MovementState.Moving;


        if (isMoving == true)
        {
            if (!audioSrc.isPlaying)
            {
                audioSrc.Play();
            }

            
        }
    }

}
