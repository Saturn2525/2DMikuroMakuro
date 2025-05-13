using System;
using CS_Script.Haruo;
using UnityEngine;

namespace CS_Script
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private bool isBig;

        public float Direction => direction;
        private float direction;

        public void SetDirection(float direction)
        {
            this.direction = direction;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IScalable scalable))
            {
                scalable.DoScale(isBig);
                Destroy(gameObject);

                if (other.TryGetComponent(out EraserEnemy enemy))
                {
                    enemy.SetDirection(direction);
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.TryGetComponent(out IScalable scalable))
            {
                scalable.DoScale(isBig);
                Destroy(gameObject);

                if (collision.collider.TryGetComponent(out EraserEnemy enemy))
                {
                    enemy.SetDirection(direction);
                }
            }
        }
    }
}