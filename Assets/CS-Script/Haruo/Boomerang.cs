using UnityEngine;

namespace CS_Script.Haruo
{
    public class Boomerang : MonoBehaviour
    {
        private void OnCollisionStay2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out Status status))
            {
                status.Damage(1);
            }
        }
    }
}