using System;
using UnityEngine;

namespace CS_Script.Naebo
{
    public class BallManager : MonoBehaviour
    {
        private float time;
        private void Start()
        {
            
        }

        private void Update()
        {
            time += Time.deltaTime;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                if (time >= 5f)
                    Destroy(this.gameObject);
            }
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                if (time >= 5f)
                    Destroy(this.gameObject);
            }
        }
    }
}