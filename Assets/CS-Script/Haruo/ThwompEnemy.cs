using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CS_Script.Haruo
{
    public class ThwompEnemy : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rig;
        [SerializeField] private Renderer meshRenderer;
        [SerializeField] private ScaleObject scaleObject;
        [SerializeField] private SpriteAnimation spriteAnimation;
        [SerializeField] private int hp = 8;
        [SerializeField] private float movePower = 10f;
        [SerializeField] private float fallDistance = 3f;
        [SerializeField] private float fallYOffset = 5f;
        [SerializeField] private float jumpDuration = 0.5f;
        [SerializeField] private float fallDelay = 2f;
        [SerializeField] private float startDelay = 2f;
        [SerializeField] private float fallSpeed = 2f;

        private float direction = 1f;
        private int defaultHp;
        private PlayerMovement playerMovement;
        private bool isJumping;

        private void Start()
        {
            if (!meshRenderer.isVisible)
            {
                rig.isKinematic = true;
            }

            scaleObject.OnScale += OnScale;
            playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
            defaultHp = hp;

            MoveState().Forget();
        }

        private async void OnScale(State state)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));

            if (state == State.MinScale)
            {
                Destroy(gameObject);
            }
        }

        private async UniTaskVoid MoveState()
        {
            scaleObject.ResetScale();


            float fallBeginDistance = Random.value >= 0.5f ? fallDistance : fallDistance + 4f;
            float speed = Random.value >= 0.5f ? movePower : movePower + 10f;

            // à⁄ìÆíÜ
            while (!destroyCancellationToken.IsCancellationRequested)
            {
                float dx = (playerMovement.transform.position - transform.position).x;

                // ÉvÉåÉCÉÑÅ[ÇÃï˚å¸å¸Ç≠
                direction = Mathf.Sign(dx);
                rig.velocity = new Vector2(speed * direction, rig.velocity.y);

                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, destroyCancellationToken);

                float distance = Mathf.Abs(dx);

                // àÍíËãóó£Ç…ãﬂÇ√Ç¢ÇΩÇÁà⁄ìÆäÆóπ
                if (distance <= fallBeginDistance)
                {
                    JumpState().Forget();
                    return;
                }
            }
        }

        private async UniTaskVoid JumpState()
        {
            rig.velocity = Vector2.zero;
            rig.isKinematic = true;
            isJumping = true;

            Vector2 targetPos = (Vector2)playerMovement.transform.position + new Vector2(0f, fallYOffset);
            Vector3 startPos = transform.position;
            Vector2 diff = targetPos - (Vector2)transform.position;

            float t = 0f;

            while (!destroyCancellationToken.IsCancellationRequested)
            {
                t += Time.fixedUnscaledDeltaTime / jumpDuration;
                float y = 1f - Mathf.Pow(1f - t, 2);
                transform.position = startPos + new Vector3(diff.x * t, diff.y * y);

                if (t > 0.99f)
                {
                    isJumping = false;
                    await UniTask.Delay(TimeSpan.FromSeconds(fallDelay), cancellationToken: destroyCancellationToken);
                    rig.isKinematic = false;
                    rig.velocity = new Vector2(0f, -fallSpeed);
                    await UniTask.Delay(TimeSpan.FromSeconds(startDelay), cancellationToken: destroyCancellationToken);
                    MoveState().Forget();
                    return;
                }

                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, destroyCancellationToken);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // ï«Ç…Ç†ÇΩÇ¡ÇΩÇÁîΩì]
            if (collision.gameObject.CompareTag("Ground") && Mathf.Abs(collision.GetContact(0).normal.y) < 0.01f)
            {
                direction = -direction;
            }

            if (!isJumping && collision.gameObject.CompareTag("Ground"))
            {
                hp -= scaleObject.CurrentStep;
                hp = Mathf.Max(0, hp);
                spriteAnimation.SetSprite(defaultHp - hp);
                isJumping = true;

                if (hp == 0)
                {
                    Death().Forget();
                }
            }
        }

        private async UniTaskVoid Death()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            Destroy(gameObject);
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