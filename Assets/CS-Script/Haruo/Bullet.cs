using System;
using CS_Script.Haruo;
using UnityEngine;

namespace CS_Script
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private bool isBig;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IScalable scalable))
            {
                scalable.DoScale(isBig);
                Destroy(gameObject);
            }
        }
    }
}