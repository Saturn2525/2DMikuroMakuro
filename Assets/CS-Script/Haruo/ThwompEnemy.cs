using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CS_Script.Haruo
{
    public class ThwompEnemy : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rig;
        [SerializeField] private Renderer meshRenderer;
        [SerializeField] private ScaleObject scaleObject;
        [SerializeField] private float rotationPower;

        private float direction = 1f;
        private PlayerMovement playerMovement;

        private void Start()
        {
            if (!meshRenderer.isVisible)
            {
                rig.isKinematic = true;
            }

            scaleObject.OnScale += OnScale;
            playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        }

        private async void OnScale(State state)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));

            if (state == State.MinScale)
            {
                Destroy(gameObject);
            }
        }

        private void FixedUpdate()
        {
            direction = -Mathf.Sign((playerMovement.transform.position - transform.position).x);
            rig.angularVelocity = rotationPower * direction;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground") && Mathf.Abs(collision.GetContact(0).normal.y) < 0.01f)
            {
                direction = -direction;
            }

            if (collision.gameObject.CompareTag("Dirt"))
            {
                if (scaleObject.CurrentStep > 0)
                {
                    Destroy(collision.gameObject);
                }
                else
                {
                    direction = -direction;
                }
            }
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out Status status))
            {
                status.Damage(1);
            }
        }

        private void OnBecameVisible()
        {
            rig.isKinematic = false;
        }
    }
}