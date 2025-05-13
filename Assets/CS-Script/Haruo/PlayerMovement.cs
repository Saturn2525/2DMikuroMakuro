using UnityEngine;
using UnityEngine.InputSystem;

namespace CS_Script.Haruo
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField, Header("加速度")] private float moveAccel;
        [SerializeField, Header("最大速度")] private float maxSpeed;
        [SerializeField, Header("減衰率")] private float damping;
        [SerializeField, Header("外力の減衰率")] private Vector2 externalDamping;
        [SerializeField, Header("重力")] private float gravity;
        [SerializeField, Header("ジャンプ力")] private float jumpPower;
        [SerializeField, Header("追加ジャンプ力")] private AnimationCurve additionalJumpPower;

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Vector2 externalForce = Vector2.zero;
        [SerializeField] private bool isJumping = false;
        [SerializeField] private Shooter shooter;

        private float moveInput = 0f;
        private float lastMoveSide = 0f;
        private float jumpStartTime = 0f;
        public float Direction => spriteRenderer.flipX ? 1f : -1f;

        /// <summary>
        /// 外力を加える
        /// </summary>
        /// <param name="force"></param>
        public void AddExternalForce(Vector2 force)
        {
            externalForce += force;
        }

        private void Update()
        {
            OnMove();

            if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                OnJump();
            }

            if (Gamepad.current != null && Gamepad.current.buttonNorth.wasPressedThisFrame ||
                Keyboard.current.cKey.wasPressedThisFrame)
            {
                shooter.ChangeShootMode();
                Debug.Log("Shoot mode changed!");
            }
            if (Gamepad.current != null && Gamepad.current.buttonSouth.wasReleasedThisFrame || Keyboard.current.spaceKey.wasReleasedThisFrame)
            {
                isJumping = false;
            }
        }

        private void FixedUpdate()
        {
            Vector2 velocity = rb.velocity;
            velocity.y += gravity; // 重力を加算

            PerformMove(ref velocity); // 移動力を加える
            PerformDamping(ref velocity); // 減衰する
            PerformExternalDamping(); // 外力の減衰する

            if (isJumping)
            {
                // ジャンプ中の追加の力を加える
                float jumpTime = Time.time - jumpStartTime;
                float additionalPower = additionalJumpPower.Evaluate(jumpTime);
                velocity += new Vector2(0f, additionalPower);
            }

            rb.velocity = velocity + externalForce;
        }

        private void OnMove()
        {
            float x = Input.GetAxis("Horizontal");
            x = Mathf.Abs(x) > 0.1f ? x : 0f;

            // 移動方向で画像を反転する
            if (x == 0f)
            {
                spriteRenderer.flipX = lastMoveSide < 0f;
            }
            else
            {
                spriteRenderer.flipX = moveInput < 0f;
                lastMoveSide = moveInput;
            }

            moveInput = x;
        }

        private void OnJump()
        {
            if (!IsGround())
                return;

            // 重力を初期化して力を加える
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(new Vector2(0f, jumpPower), ForceMode2D.Impulse);
            isJumping = true;
            jumpStartTime = Time.time;
        }

        /// <summary>
        /// 地面の判定
        /// </summary>
        /// <returns></returns>
        private bool IsGround()
        {
            const float checkDistance = 1.1f;
            int mask = ~LayerMask.GetMask("Player");
            return Physics2D.BoxCast(transform.position, transform.localScale * 0.5f, 0f, -transform.up, checkDistance, mask);
        }

        private void PerformMove(ref Vector2 velocity)
        {
            // x方向に力を与える
            float vx = moveInput * moveAccel;

            // Rigidbodyの速度を設定する
            velocity.x += vx;
            velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);
        }

        private void PerformDamping(ref Vector2 velocity)
        {
            float airControl = isJumping ? 0.9f : 1f;
            velocity.x *= damping * airControl;
        }

        private void PerformExternalDamping()
        {
            externalForce *= externalDamping;

            if (externalForce.sqrMagnitude < 0.01f)
            {
                externalForce = Vector2.zero;
            }
        }
    }
}