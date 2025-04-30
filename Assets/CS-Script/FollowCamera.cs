using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target; // �Ǐ]�Ώہi�v���C���[�Ȃǁj
    private float followSpeed = 5f; // �ړ����x
    private float YAbs;

    void FixedUpdate()
    {
        // ���݂̈ʒu���擾
        Vector3 newPosition = transform.position;
        // X���W�݂̂�Ǐ]
        newPosition.x = Mathf.Lerp(transform.position.x, target.position.x, followSpeed * Time.deltaTime);
        YAbs=Mathf.Abs(newPosition.y - target.position.y);
        Debug.Log($"Y����={YAbs}");
        if (target.position.y > 0 || YAbs > 3.5f)
        {
            newPosition.y = Mathf.Lerp(transform.position.y, target.position.y, followSpeed * Time.deltaTime);
        }
        // �ʒu���X�V
        transform.position = newPosition;
    }
}
