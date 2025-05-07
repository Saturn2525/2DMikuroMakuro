using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float moveAccel;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float damping;
    [SerializeField] private Vector2 externalDamping;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpPower;
    [SerializeField] private AnimationCurve additionalJumpPower;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2 externalForce = Vector2.zero;

    private float moveInput = 0f;
    private float lastMoveSide = 0f;
    private float jumpStartTime = 0f;
    [SerializeField] private bool isJumping = false;

    private void Update()
    {
        OnMove();

        if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            OnJump();
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

        PerformMove(ref velocity);
        PerformDamping(ref velocity);
        PerformExternalForce();

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

    private bool IsGround()
    {
        const float checkDistance = 1.2f;
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
        // 速度を減少させる
        velocity.x *= damping;
    }

    private void PerformExternalForce()
    {
        externalForce *= externalDamping;

        if (externalForce.sqrMagnitude < 0.01f)
        {
            externalForce = Vector2.zero;
        }
    }
}