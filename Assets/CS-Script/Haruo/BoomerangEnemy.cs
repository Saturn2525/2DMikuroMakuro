using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace CS_Script.Haruo
{
    public class BoomerangEnemy : MonoBehaviour
    {
        [SerializeField] private ScaleObject boomerang;
        [SerializeField] private float boomerangSpeed = 5f;
        [SerializeField] private int boomerangRotateCount = 5;
        [SerializeField] private float boomerangRadius = 5f;
        [SerializeField] private float boomerangInterval = 1f;
        [SerializeField] private bool isRight;

        private void Start()
        {
            DoBoomerang().Forget();
        }

        private async UniTaskVoid DoBoomerang()
        {
            float direction = isRight ? 1f : -1f;
            float duration = boomerangRadius / boomerangSpeed;
            Vector3 targetPosition = boomerang.transform.localPosition + transform.right * direction * boomerangRadius;
            Vector3 targetRotation = new Vector3(0f, 0f, boomerangRotateCount * 360f * -direction);

            while (!destroyCancellationToken.IsCancellationRequested)
            {
                // ブーメランの回転
                boomerang.transform
                    .DOLocalRotate(targetRotation, duration * 2f, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear);

                // ブーメランの前後移動
                await boomerang.transform
                    .DOLocalMove(targetPosition, duration)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(2, LoopType.Yoyo);

                if (boomerang.CurrentStep == 1)
                {
                    Death().Forget();
                    return;
                }

                await UniTask.Delay(TimeSpan.FromSeconds(boomerangInterval), cancellationToken: destroyCancellationToken);
            }
        }

        private async UniTaskVoid Death()
        {
            await transform.DOShakePosition(1f, 1f, 30);
            Destroy(gameObject);
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out Status status))
            {
                status.Damage(1);
            }
        }
    }
}