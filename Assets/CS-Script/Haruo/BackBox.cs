using UnityEngine;

namespace CS_Script.Haruo
{
    public class BackBox : Box
    {
        [SerializeField, Header("���Ƃɖ߂鎞��")] private float backTime = 1f;

        private void Update()
        {
            if (Time.time - LatestScaleTime > backTime && Step != 0)
            {
                ResetScale();
            }
        }
    }
}