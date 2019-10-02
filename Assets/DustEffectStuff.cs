using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustEffectStuff : MonoBehaviour
{
    [SerializeField]
    GameObject dustEffect;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        {
            if (collision.gameObject.tag == "Ground")
            {
                Instantiate(dustEffect, transform.position, dustEffect.transform.rotation);
            }
        }
    }
}
