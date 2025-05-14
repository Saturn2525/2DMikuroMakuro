using System;
using CS_Script.Haruo;
using UnityEngine;
using UnityEngine.Serialization;

namespace CS_Script.Naebo
{
    public class BalloonShip : MonoBehaviour
    {
        [SerializeField] private BackBox leftBalloon;
        [SerializeField] private BackBox rightBalloon;

        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private int defaultCou;
        [Header("風船一つの浮力係数")]
        [SerializeField] private float floatingFactor;
        private void Start()
        {
            _currentPos = transform.position; // 現在位置を初期化
            _beforePos = transform.position; // 前回の位置を初期化
        }

        private void FixedUpdate()
        {
            if (leftBalloon.CurrentStep > defaultCou)
            {
                _rb.AddForce(Vector2.up * (floatingFactor * leftBalloon.CurrentStep),  ForceMode2D.Force);
               // transform.Rotate();
                _rb.AddForce(Vector2.left * floatingFactor, ForceMode2D.Force);
            }

            if (rightBalloon.CurrentStep > defaultCou)
            {
                _rb.AddForce(Vector2.up * (floatingFactor * rightBalloon.CurrentStep),  ForceMode2D.Force);
                // 回転
                _rb.AddForce(Vector2.right * floatingFactor, ForceMode2D.Force);
            }

            CalculateMovementAmount();
        }

        private Vector2 _movementAmount;
        // 移動量を取得するプロパティ
        public Vector2 MovementAmount 
        {
            get { return _movementAmount; }
        }
        
        private Vector2 _beforePos; // 前回の位置を格納
        private Vector2 _currentPos; // 現在の位置を格納
        private void CalculateMovementAmount()
        {
            // 現在位置と前回の位置の差分を計算
            _currentPos = (Vector2)transform.position;
            _movementAmount = _currentPos - _beforePos;
            _beforePos = _currentPos; // 前回位置を更新
            Debug.Log(_movementAmount);
        }
    }
}