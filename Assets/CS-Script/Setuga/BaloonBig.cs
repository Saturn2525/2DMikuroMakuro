using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaloonBig : MonoBehaviour
{
    public float scaleFactor = 4f;
    public Vector2 boxSize = new Vector2(1f, 1f);
    // Start is called before the first frame update
    public void BigBloon()
    {
        if (Input.GetMouseButtonDown(0))
        {
            transform.localScale *= scaleFactor;
            boxSize *= scaleFactor;
        }
    }
        
}
