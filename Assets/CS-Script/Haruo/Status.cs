using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CS_Script.Haruo
{
    public class Status : MonoBehaviour
    {
        [SerializeField] private Image[] hpIcons;
        [SerializeField] private float damageInterval = 0.5f;
        private List<Image> hpIconsList = new List<Image>();
        private float lastDamageTime;

        private void Start()
        {
            hpIconsList = hpIcons.ToList();
        }

        public async void Damage(int damage)
        {
            if (Time.timeScale == 0f || Time.time - lastDamageTime < damageInterval)
                return;

            for (int i = 0; i < damage; i++)
            {
                if (hpIconsList.Count == 0)
                    break;

                hpIconsList[0].DOFade(0f, 0.3f).SetEase(Ease.Flash, 5, 0).SetUpdate(true);
                hpIconsList.RemoveAt(0);
            }

            lastDamageTime = Time.time;

            Time.timeScale = 0f;
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), ignoreTimeScale: true, cancellationToken: destroyCancellationToken);
            Time.timeScale = 1f;
        }
    }
}