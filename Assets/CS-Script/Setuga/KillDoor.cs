using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillDoor : MonoBehaviour
{
    public GameObject Door;
    // Start is called before the first frame update
    
    void OnCollisionEnter2D(Collision2D collision)
    { 
        Door.SetActive(false);
        Debug.Log("Doorè¡Ç¶Ç‹Ç∑ÅI");
    }
}
