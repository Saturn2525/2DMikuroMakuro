using System;
using CS_Script;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    [SerializeField] private GameObject smallBullet;
    [SerializeField] private GameObject bigBullet;
    [SerializeField] private Transform visualizer;
    [SerializeField] private Rigidbody2D playerRig;
    [SerializeField] private float power;

    private Vector2 shootDirection;
    private Vector2 lastSideInput;

    private void Start()
    {
        shootDirection = Vector2.right;
        lastSideInput = Vector2.right;
    }

    private void Update()
    {
        if (Gamepad.current != null && Gamepad.current.rightTrigger.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
        {
            OnBigShot();
        }

        if (Gamepad.current != null && Gamepad.current.leftTrigger.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame)
        {
            OnSmallShot();
        }

        UpdateInput();
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        // 視点のオブジェクト入力の方向に向ける
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        visualizer.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void OnBigShot()
    {
        GameObject bulletObj = Instantiate(bigBullet, visualizer.position + (Vector3)shootDirection * 0.5f, Quaternion.identity);
        Rigidbody2D bulletRig = bulletObj.GetComponent<Rigidbody2D>();
        bulletRig.velocity = shootDirection * power + new Vector2(playerRig.velocity.x, 0f);

        if (shootDirection.y > 0f)
        {
            bulletRig.velocity += new Vector2(0f, Mathf.Max(0f, playerRig.velocity.y));
        }
        else if (shootDirection.y < 0f)
        {
            bulletRig.velocity += new Vector2(0f, Mathf.Min(0f, playerRig.velocity.y));
        }
    }

    private void OnSmallShot()
    {
        GameObject bulletObj = Instantiate(smallBullet, visualizer.position + (Vector3)shootDirection * 0.5f, Quaternion.identity);
        Rigidbody2D bulletRig = bulletObj.GetComponent<Rigidbody2D>();
        bulletRig.velocity = shootDirection * power + new Vector2(playerRig.velocity.x, 0f);

        if (shootDirection.y > 0f)
        {
            bulletRig.velocity += new Vector2(0f, Mathf.Max(0f, playerRig.velocity.y));
        }
        else if (shootDirection.y < 0f)
        {
            bulletRig.velocity += new Vector2(0f, Mathf.Min(0f, playerRig.velocity.y));
        }
    }

    private void UpdateInput()
    {
        Vector2 input = Vector2.zero;
        if (Gamepad.current == null)
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        else
        {
            input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }


        float x = input.x;
        bool isDeadZone = Mathf.Abs(x) > 0.3f;
        x = isDeadZone ? x : 0f;
        input.x = x;

        // ゼロ入力は無効
        if (input == Vector2.zero)
        {
            shootDirection = lastSideInput;
        }
        else if (input.y > 0.5f && Vector2.Angle(input, Vector2.up) < 30f)
        {
            shootDirection = Vector2.up;
        }
        else if (input.y < -0.5f && Vector2.Angle(input, Vector2.down) < 30f)
        {
            shootDirection = Vector2.down;
        }
        else if (isDeadZone)
        {
            shootDirection = input.x > 0f ? Vector2.right : Vector2.left;
            lastSideInput = shootDirection;
        }
    }

    public void Flip()
    {
        if (shootDirection == Vector2.right)
        {
            shootDirection = Vector2.left;
            lastSideInput = shootDirection;
        }
        else if (shootDirection == Vector2.left)
        {
            shootDirection = Vector2.right;
            lastSideInput = shootDirection;
        }
    }
}