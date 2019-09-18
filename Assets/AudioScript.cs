using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public AudioClip JumpClip;

    public AudioSource JumpSource;
    // Start is called before the first frame update
    void Start()
    {
        JumpSource.clip = JumpClip;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpSource.Play();
        }
    }
}
