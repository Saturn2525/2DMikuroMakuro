using UnityEngine;

namespace CS_Script.Haruo
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField, Header("追従対象(プレイヤー)")] private Transform target;
        [SerializeField, Header("追従速度")] private float followSpeed = 5f;
        [SerializeField, Header("高さのオフセット")] private float yOffset = 2f;
        private float targetY;

        private void Start()
        {
            targetY = target.position.y;
        }

        private void FixedUpdate()
        {
            // 現在の位置を取得
            Vector3 newPosition = transform.position;
            // X座標のみを追従
            newPosition.x = Mathf.Lerp(transform.position.x, target.position.x, followSpeed * Time.deltaTime);
            // 位置を更新
            transform.position = new Vector3(newPosition.x, targetY + yOffset, newPosition.z);
        }
    }
}