using System;
using UnityEngine;

namespace CS_Script.Naebo
{
    public class Overlap : MonoBehaviour
    {
        [SerializeField] private Renderer renderer;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Ball"))
                renderer.enabled = false;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Ball"))
                renderer.enabled = true;
        }
    }
}