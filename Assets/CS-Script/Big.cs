using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big : MonoBehaviour
{
    // Start is called before the first frame update
    public float scaleFactor = 4f;
    public float bouncePower = 10f; // ���˂��
    public Vector2 boxSize = new Vector2(2f, 1f);
    public int BigPass = 0;

    public void debag()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            if (BigPass == 0 || BigPass == -1)
            {// this.transform.localScale = new Vector3(4, 4, 1);
                transform.localScale *= scaleFactor;
                boxSize *= scaleFactor;
                BigPass++;
                CheckBounce();
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (BigPass == 0 || BigPass == 1)
            {//this.transform.localScale = new Vector3(1, 1, 1);
                transform.localScale /= scaleFactor;
                boxSize /= scaleFactor;
                BigPass--;
            }
        }
        Debug.Log("�^�b�`�I");
    }
    public void CheckBounce()
    {
        // �^�[�Q�b�g�ʒu�𒆐S�Ɍ��o
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, boxSize, 0);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    // ������ɒ��˂�����
                    rb.velocity = new Vector2(rb.velocity.x, bouncePower);
                    Debug.Log("�{�b�N�X�W�����v�I");
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}
