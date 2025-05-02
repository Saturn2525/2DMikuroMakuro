using System;
using DG.Tweening;
using UnityEngine;

namespace CS_Script.Haruo
{
    public class Box : MonoBehaviour
    {
        [SerializeField, Header("スケール係数")] private float scaleFactor = 4f;
        [SerializeField, Header("スケール時間")] private float scaleDuration = 0.5f;
        [SerializeField, Header("スケール基準点")] private Vector2 pivot = new Vector2(0.5f, 0.5f);
        [SerializeField, Header("反発力")] private float bouncePower = 10f;
        [SerializeField, Header("最小段階")] private int minStep = 0;
        [SerializeField, Header("最大段階")] private int maxStep = 3;
        [SerializeField, Header("現在の段階")] protected int Step;

        private Tween currentTween;
        private Vector2 targetScale;
        private Vector2 defaultScale;
        private Vector2 defaultPosition;
        private Vector2 defaultWorldPosition;

        protected float LatestScaleTime = 0f;

        public event Action<bool> OnScale;
        public event Action OnBounce;

        protected virtual void Start()
        {
            targetScale = Vector2.one + Vector2.one * scaleFactor * Step;
            defaultScale = Vector2.one;
            defaultPosition = transform.localPosition;
            defaultWorldPosition = transform.position;
            LatestScaleTime = Time.time;
        }

        private void OnValidate()
        {
            defaultPosition = transform.localPosition;
            defaultWorldPosition = transform.position;
            defaultScale = Vector2.one;
            targetScale = Vector2.one + Vector2.one * scaleFactor * Step;
            ScaleAround(GetLocalPivot(), targetScale);
        }

        public void DoScale(bool isBig)
        {
            // 段階条件を満たしていない場合は何もしない
            if (!isBig && Step <= minStep || isBig && Step >= maxStep)
            {
                return;
            }

            int sign = isBig ? 1 : -1;
            targetScale += Vector2.one * scaleFactor * sign;
            targetScale.x = Mathf.Max(0f, targetScale.x);
            targetScale.y = Mathf.Max(0f, targetScale.y);

            Step += sign;
            LatestScaleTime = Time.time;

            // スケールを変更する
            DoAroundScale(targetScale);

            // プレイヤーをはねさせる
            CheckBounce(targetScale);

            OnScale?.Invoke(isBig);
        }

        public void ResetScale()
        {
            // スケールを元に戻す
            DoAroundScale(defaultScale);
            LatestScaleTime = Time.time;
            targetScale = defaultScale;
            Step = 0;
        }

        private void DoAroundScale(Vector2 targetScale)
        {
            currentTween?.Complete();

            Vector2 scale = transform.localScale;
            currentTween = DOTween.To(() => scale,
                value =>
                {
                    scale = value;

                    Vector2 localPivot = GetLocalPivot();

                    // 左下を(0, 0)として指定したピボットを中心にスケールを変更
                    ScaleAround(localPivot, scale);
                },
                targetScale,
                scaleDuration).SetEase(Ease.OutBack, 3f);
        }

        public void CheckBounce(Vector2 newScale)
        {
            Vector2 localPivot = GetLocalPivot();
            Vector2 localPosition = CalculateScaledPosition(localPivot, newScale);
            
            debugBoxPosition = transform.position;
            debugBoxSize = newScale * 2f;

            // ターゲット位置を中心に検出
            Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, newScale * 2f, 0);
            foreach (Collider2D col in colliders)
            {
                if (col.CompareTag("Player") && col.TryGetComponent(out Rigidbody2D rb))
                {
                    // 上方向に跳ねさせる
                    rb.velocity = new Vector2(rb.velocity.x, bouncePower);
                    OnBounce?.Invoke();
                    Debug.Log("ボックスジャンプ！");
                }
            }
        }

        private Vector2 debugBoxPosition;
        private Vector2 debugBoxSize;
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(debugBoxPosition, debugBoxSize);
        }

        public Vector2 GetLocalPivot()
        {
            Vector2 localPivot = defaultPosition + (pivot - new Vector2(0.5f, 0.5f)) * defaultScale.x;
            return localPivot;
        }

        /// <summary>
        /// 指定のピボットを中心にスケールを変更する
        /// </summary>
        /// <param name="pivot"></param>
        /// <param name="newScale"></param>
        private void ScaleAround(Vector2 pivot, Vector2 newScale)
        {
            Vector2 resultPos = CalculateScaledPosition(pivot, newScale);
            transform.localScale = newScale;
            transform.localPosition = resultPos;
        }

        private Vector2 CalculateScaledPosition(Vector2 pivot, Vector2 newScale)
        {
            Vector2 targetPos = transform.localPosition;
            Vector2 diff = targetPos - pivot;
            float relativeScale = newScale.x / transform.localScale.x;
            return pivot + diff * relativeScale;
        }
    }
}