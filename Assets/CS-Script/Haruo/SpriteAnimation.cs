using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimation : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    public void SetSprite(int index)
    {
        if (0 > index || index >= sprites.Length)
            return;
        
        spriteRenderer.sprite = sprites[index];
    }
}
