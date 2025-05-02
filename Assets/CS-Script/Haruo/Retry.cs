using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Retry : MonoBehaviour
{
    void Update()
    {
        if (Gamepad.current.buttonWest.wasPressedThisFrame || Keyboard.current.rKey.wasPressedThisFrame)
        {
            Debug.Log("Retrying the game...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
           
    }
}
