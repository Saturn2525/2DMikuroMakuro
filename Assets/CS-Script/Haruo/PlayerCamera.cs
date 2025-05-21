using UnityEngine;

namespace CS_Script.Haruo
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField, Header("追従対象(プレイヤー)")] private Transform target;
        [SerializeField, Header("追従速度")] private float followSpeed = 5f;
        [SerializeField, Header("上部のカメラ制限のしきい値")] private float upSideDeadZone = 0.75f;
        [SerializeField, Header("下部のカメラ制限のしきい値")] private float downSideDeadZone = 0.25f;
        private float targetY;

        private void Start()
        {
            targetY = transform.position.y;
        }

        private void FixedUpdate()
        {
            Vector2 screenPoint = Camera.main.WorldToViewportPoint(target.position);

            if (screenPoint.y > upSideDeadZone)
            {
                // 上部のカメラ制限のしきい値を超えた場合
                targetY += (screenPoint.y - upSideDeadZone) * Camera.main.orthographicSize * 0.5f;
            }
            else if (screenPoint.y < downSideDeadZone)
            {
                // 下部のカメラ制限のしきい値を超えた場合
                targetY -= (downSideDeadZone - screenPoint.y) * Camera.main.orthographicSize * 0.5f;
            }

            // 現在の位置を取得
            Vector3 newPosition = transform.position;
            // X座標のみを追従
            newPosition.x = Mathf.Lerp(transform.position.x, target.position.x, followSpeed);
            // 位置を更新
            transform.position = new Vector3(newPosition.x, targetY, newPosition.z);
        }
    }
}