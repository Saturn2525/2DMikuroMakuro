using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CS_Script.Haruo
{
    public class JumperEnemy : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rig;
        [SerializeField] private ScaleObject scaleObject;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float jumpPower = 10f;
        [SerializeField] private float jumpInterval = 1f;

        private float direction = 1f;

        private void Start()
        {
            rig.isKinematic = true;
            JumpLoop().Forget();
            scaleObject.OnScale += OnScale;
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
            if (rig.isKinematic)
                return;

            rig.velocity = new Vector2(moveSpeed * direction, rig.velocity.y);
        }

        private async UniTaskVoid JumpLoop()
        {
            while (!destroyCancellationToken.IsCancellationRequested)
            {
                await UniTask.WaitUntil(() => !rig.isKinematic, cancellationToken: destroyCancellationToken);
                await UniTask.WaitUntil(IsGround, cancellationToken: destroyCancellationToken);
                Jump();
                await UniTask.Delay(TimeSpan.FromSeconds(jumpInterval), cancellationToken: destroyCancellationToken);
            }
        }

        private void Jump()
        {
            rig.velocity = new Vector2(rig.velocity.x, jumpPower);
        }

        private bool IsGround()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f);
            return hit.collider != null;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground") && Mathf.Abs(collision.GetContact(0).normal.y) < 0.01f)
            {
                direction = -direction;
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