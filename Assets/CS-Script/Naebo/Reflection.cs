using System;
using UnityEngine;
using System.Collections;

namespace CS_Script.Naebo
{
    public class Reflection : MonoBehaviour
    {
        [Header("���ˎ��͗͂��Z�{����")]
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
            // ���S + �����̔���
            float topY = other.transform.position.y + (other.transform.localScale.y / 2f);    // cube�̏�ʂ̍����擾
            
            // ��ʂ���Ȃ���Δ���
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
            // ���˃x�N�g�� (���x�j
            _inDirection = (Vector2)_rb.velocity;
            // �@���x�N�g�� �ڒn�_����擾
            // Vector3 inNormal =  other.contacts[0].normal;
            Vector2 inNormal = collision.contacts[0].normal;
        
            // ���˃x�N�g���i���x�j
            Vector2 result = Vector2.Reflect(_inDirection, inNormal);
            
            // ���ˌ�̃x�N�g����������Ȃ瑬�x�𔽉f
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
        

        //OnCollision�����x���N���Ȃ��悤�ɑҋ@���ԓ���Ă݂�
        private IEnumerator WaitAndReflective()
        {
            _wait = true;
            yield return new WaitForSeconds(0.1f);
            _wait = false;
        }
        
    }
}