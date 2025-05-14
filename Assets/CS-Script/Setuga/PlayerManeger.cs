using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManeger : MonoBehaviour
{
    //private Rigidbody rb;
    private bool RorL = true;
    private float RunSpeed = 1f;
    void Start()
    {
        
    }
    // Update is called once per frame

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        if (horizontal > 0) 
        {
            RorL = true;
        }

        else if (horizontal < 0)
        {
            RorL = false;
        }
    }

    private void FixedUpdate()
    {
        if (RorL == true)
        {
            this.transform.Translate(Vector3.right * 0.1f);
            if (this.gameObject.CompareTag("Player"))
            {
                transform.localScale = new Vector3(RunSpeed, 1, 1);
            }
        }

        else if (RorL == false)
        {
            this.transform.Translate(Vector3.left * 0.1f);
            if (this.gameObject.CompareTag("Player"))
            {
                transform.localScale = new Vector3(RunSpeed * -1, 1, 1);
            }
        }
    }
}
