using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CS_Script.Naebo
{
    public class PushBox : MonoBehaviour
    {
     
        
        private Rigidbody2D rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (collision.transform.forward == gameObject.transform.forward)
                {
                    rb.velocity = collision.relativeVelocity;
                }
            }
        }
    }
}

