using UnityEngine;
using System.Collections;

namespace CS_Script.Naebo
{
    public class Reflection : MonoBehaviour
    {
        [Header("���ˎ��͗͂��Z�{����")]
        [SerializeField] private Vector3 scalePower;
        private Rigidbody _rb;
        private bool _wait;
        private int _bounceCou;
        private Vector3 _inDirection; 
        private void OnCollisionEnter(Collision other)
        {
            if (_wait)
                return;

            _bounceCou++;
            if (_bounceCou >= 5)
                Destroy(this);
            
            if (!other.gameObject.CompareTag("Wall"))
                return;
            
            if (_rb == null)
            {
                _rb = gameObject.GetComponent<Rigidbody>();
            }
            
            // ���˃x�N�g�� (���x�j
            _inDirection = _rb.velocity;
            // �@���x�N�g�� �ڒn�_����擾
            // Vector3 inNormal =  other.contacts[0].normal;
            Vector3 inNormal = other.contacts[0].normal;
        
            // ���˃x�N�g���i���x�j
            Vector3 result = Vector3.Reflect(_inDirection, inNormal);
        
            _rb.velocity = Vector3.zero;
        
            // ���ˌ�̑��x�𔽉f
            _rb.velocity = Vector3.Scale(result, scalePower);
          //  GameManager.instance.Directon = result.normalized;
          
            StartCoroutine("WaitAndReflective");
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