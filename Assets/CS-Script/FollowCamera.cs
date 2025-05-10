using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target; // 追従対象（プレイヤーなど）
    private float followSpeed = 5f; // 移動速度
    private float YAbs;

    void FixedUpdate()
    {
        // 現在の位置を取得
        Vector3 newPosition = transform.position;
        // X座標のみを追従
        newPosition.x = Mathf.Lerp(transform.position.x, target.position.x, followSpeed * Time.deltaTime);
        YAbs = Mathf.Abs(newPosition.y - target.position.y);
        if (target.position.y > 0 || YAbs > 3.5f)
        {
            newPosition.y = Mathf.Lerp(transform.position.y, target.position.y, followSpeed * Time.deltaTime);
        }

        // 位置を更新
        transform.position = newPosition;
    }
}