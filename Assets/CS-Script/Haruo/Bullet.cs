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
            if (other.TryGetComponent(out Box box))
            {
                box.DoScale(isBig);
                Destroy(gameObject);
            }
        }
    }
}