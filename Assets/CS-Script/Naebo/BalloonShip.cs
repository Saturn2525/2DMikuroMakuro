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
            
        }

        private void FixedUpdate()
        {
            if (leftBalloon.CurrentStep > defaultCou)
            {
                _rb.AddForce(Vector2.up * (floatingFactor * leftBalloon.CurrentStep),  ForceMode2D.Force);
                // 回転
                _rb.AddForce(Vector2.left * floatingFactor, ForceMode2D.Force);
            }

            if (rightBalloon.CurrentStep > defaultCou)
            {
                _rb.AddForce(Vector2.up * (floatingFactor * rightBalloon.CurrentStep),  ForceMode2D.Force);
                // 回転
                _rb.AddForce(Vector2.right * floatingFactor, ForceMode2D.Force);
            }
        }
    }
}