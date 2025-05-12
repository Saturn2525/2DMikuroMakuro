using System;
using UnityEngine;

namespace CS_Script.Haruo
{
    public class Spike : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player") && other.gameObject.TryGetComponent(out Status status))
            {
                status.Damage(1);
            }
            
            if (other.gameObject.TryGetComponent(out IScalable scalable))
            {
                scalable.DoScale(false);
            }
        }
    }
}
