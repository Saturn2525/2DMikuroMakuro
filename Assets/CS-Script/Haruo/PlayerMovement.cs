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

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2 externalForce = Vector2.zero;

    private float moveInput = 0f;

    public float Direction
    {
        get => moveInput;
        set
        {
            moveInput = value;
            spriteRenderer.flipX = moveInput < 0f;
        }
    }

    private void Start()
    {
        OnMove();
    }

    private void FixedUpdate()
    {
        Vector2 velocity = rb.velocity;
        velocity.y += gravity; // 重力を加算

        PerformMove(ref velocity);
        PerformDamping(ref velocity);
        PerformExternalForce();

        rb.velocity = velocity + externalForce;
    }

    private void OnMove()
    {
        // float x = Input.GetAxis("Horizontal");
        // x = Mathf.Abs(x) > 0.3f ? x : 0f;
        // moveInput = x;

        moveInput = 1f;
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