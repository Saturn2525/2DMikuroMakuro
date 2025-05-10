using UnityEngine;

namespace CS_Script.Haruo
{
    public class BackBox : ScaleObject
    {
        [SerializeField, Header("‚à‚Æ‚É–ß‚éŽžŠÔ")] private float backTime = 1f;

        private void Update()
        {
            if (Time.time - LatestScaleTime > backTime && CurrentStep != 0)
            {
                ResetScale();
            }
        }
    }
}