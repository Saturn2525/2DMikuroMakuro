using CS_Script.Naebo;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace CS_Script.Haruo
{
    public class Shooter : MonoBehaviour
    {
        [SerializeField] private float power;
        [SerializeField] private float shootInterval = 0.3f;
        [SerializeField] private GameObject smallBullet;
        [SerializeField] private GameObject bigBullet;
        [SerializeField] private GameObject smallGrenade;
        [SerializeField] private GameObject bigGrenade;
        [SerializeField] private Transform visualizer;
        [SerializeField] private Rigidbody2D playerRig;
        [SerializeField] private Transform shootDir;
        [SerializeField, Range(0f, 90f)] float grenadeAngle = 45f;
        [Header("最初の射撃モードをグレに")]
        [SerializeField] private bool isFirstGrenade;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private BallSimulator ballSimulator;

        private Vector2 shootDirection;
        private Vector2 lastSideInput;
        private float lastShootTime;
        private bool isGrenadeShoot;

        private void Start()
        {
            shootDirection = Vector2.right;
            lastSideInput = Vector2.right;
            isGrenadeShoot = isFirstGrenade;
            ballSimulator.SimulateSwitch = isFirstGrenade;
        }

        private void Update()
        {
            CalcVelocity();
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
          
                // 左右
                if (isGrenadeShoot && shootDirection == Vector2.right)
                {
                    if (ballSimulator.SimulateSwitch == false)
                        ballSimulator.SimulateSwitch = true;
                    
                    visualizer.rotation = Quaternion.Euler(0f, 0f, angle + grenadeAngle);
                }
                else if (isGrenadeShoot && shootDirection == Vector2.left)
                {
                    if (ballSimulator.SimulateSwitch == false)
                        ballSimulator.SimulateSwitch = true;
                    
                    visualizer.rotation = Quaternion.Euler(0f, 0f, angle - grenadeAngle);
                }
                else  
                {
                    // 上下
                    visualizer.rotation = Quaternion.Euler(0f, 0f, angle);
                    ballSimulator.SimulateSwitch = false;
                }
            
        }
       
        public void ChangeShootMode()
        {
            if (isGrenadeShoot)
            {
                // 十時撃ちに切り替え
                isGrenadeShoot = false;
                ballSimulator.SimulateSwitch = false;
            }
            else
            {
                // グレポン撃ちに切り替え
                isGrenadeShoot = true;
                ballSimulator.SimulateSwitch = true;
            }
        }

        private Vector2 _velocity;
        private void CalcVelocity()
        {
            shootDirection = (shootDir.position - visualizer.position).normalized;

            _velocity = shootDirection * power + new Vector2(playerRig.velocity.x, 0f);
            if (shootDirection.y > 0f)
            {
                _velocity += new Vector2(0f, Mathf.Max(0f, playerRig.velocity.y));
            }
            else if (shootDirection.y < 0f)
            {
               _velocity += new Vector2(0f, Mathf.Min(0f, playerRig.velocity.y));
            }
            
            ballSimulator.Simulate(_velocity);
        }

        private void OnBigShot()
        {
            if (Time.time - lastShootTime < shootInterval)
            {
                return;
            }

            GameObject bulletObj;
            
            if (isGrenadeShoot)
            {
                // 絶対にもっと楽な方法あるだろ
                bulletObj = Instantiate(bigGrenade, visualizer.position + (Vector3)shootDirection * 0.5f, Quaternion.identity);
            }
            else
            {
                 bulletObj = Instantiate(bigBullet, visualizer.position + (Vector3)shootDirection * 0.5f, Quaternion.identity);
            }
           
            Rigidbody2D bulletRig = bulletObj.GetComponent<Rigidbody2D>();

            // 弾道予測のためにVelocityの計算をCalcVelocity()に移動
            bulletRig.velocity = _velocity;

            bulletObj.GetComponent<Bullet>().SetDirection(playerMovement.Direction);

            lastShootTime = Time.time;
        }

        private void OnSmallShot()
        {
            if (Time.time - lastShootTime < shootInterval)
            {
                return;
            }

            GameObject bulletObj;
            
            if (isGrenadeShoot)
            {
                bulletObj = Instantiate(smallGrenade, visualizer.position + (Vector3)shootDirection * 0.5f, Quaternion.identity);
            }
            else
            {
                bulletObj = Instantiate(smallBullet, visualizer.position + (Vector3)shootDirection * 0.5f, Quaternion.identity);
            }
            Rigidbody2D bulletRig = bulletObj.GetComponent<Rigidbody2D>();

            // 弾道予測のためにVelocityの計算をCalcVelocity()に移動
            bulletRig.velocity = _velocity;
          
            bulletObj.GetComponent<Bullet>().SetDirection(playerMovement.Direction);

            lastShootTime = Time.time;
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
}