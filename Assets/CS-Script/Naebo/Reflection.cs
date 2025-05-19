using System;
using UnityEngine;
using System.Collections;

namespace CS_Script.Naebo
{
    public class Reflection : MonoBehaviour
    {
        [Header("反射時は力を〇倍する")]
        [SerializeField] private Vector3 scalePower;
        private Rigidbody2D _rb;
        private bool _wait;
        private int _bounceCou;
        private Vector2 _inDirection; 
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (_wait)
                return;

            _bounceCou++;
            if (_bounceCou >= 5)
                Destroy(gameObject);
            
            if (!other.gameObject.CompareTag("Ground"))
                return;
            
            if (_rb == null)
            {
                _rb = gameObject.GetComponent<Rigidbody2D>();
            }
            // 中心 + 高さの半分
            float topY = other.transform.position.y + (other.transform.localScale.y / 2f);    // cubeの上面の高さ取得
            
            // 上面じゃなければ反射
            if (_rb.transform.position.y < topY)
            {
                Reflect(other);
                StartCoroutine("WaitAndReflective");
            }
            else
            {
                Debug.Log("UpSide");
            }
        }

        private void Reflect(Collision2D collision)
        {
            // 入射ベクトル (速度）
            _inDirection = (Vector2)_rb.velocity;
            // 法線ベクトル 接地点から取得
            // Vector3 inNormal =  other.contacts[0].normal;
            Vector2 inNormal = collision.contacts[0].normal;
        
            // 反射ベクトル（速度）
            Vector2 result = Vector2.Reflect(_inDirection, inNormal);
            
            // 反射後のベクトルが上方向なら速度を反映
            if (result.y > 0)
            {
                _rb.velocity = Vector2.zero;
                _rb.velocity = Vector2.Scale(result, scalePower);
                Debug.Log("Reflect");
            }
            else
            {
               // Debug.Log("NOT Reflect");
            }
        }
        

        //OnCollisionが何度も起きないように待機時間入れてみる
        private IEnumerator WaitAndReflective()
        {
            _wait = true;
            yield return new WaitForSeconds(0.1f);
            _wait = false;
        }
        
    }
}