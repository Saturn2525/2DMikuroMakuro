using System;
using DG.Tweening;
using UnityEngine;

namespace CS_Script.Haruo
{
    public enum State
    {
        InScale,
        MinScale,
        MaxScale
    }

    public class ScaleObject : MonoBehaviour, IScalable
    {
        [SerializeField, Header("スケール係数")] private float scaleFactor = 4f;
        [SerializeField, Header("スケール時間")] private float scaleDuration = 0.5f;

        [SerializeField, Header("基準点からのスケールを有効化する\\n(動かないオブジェクトのみ有効)")]
        private bool enablePivotScaling;

        [SerializeField, Header("スケール基準点")] private Vector2 pivot = new Vector2(0.5f, 0.5f);
        [SerializeField, Header("反発力")] private float bouncePower = 10f;
        [SerializeField, Header("最小段階")] private int minStep = 0;
        [SerializeField, Header("最大段階")] private int maxStep = 3;
        [SerializeField, Header("現在の段階")] private int step;

        /// <summary>
        /// 現在のスケール段階
        /// </summary>
        public int CurrentStep => step;

        /// <summary>
        /// 最近スケールした時間 
        /// </summary>
        public float LatestScaleTime => latestScaleTime;

        /// <summary>
        /// スケールしたときに呼ばれるイベント
        /// </summary>
        public event Action<State> OnScale;

        /// <summary>
        /// 反発したときに呼ばれるイベント
        /// </summary>
        public event Action OnBounce;

        private Tween currentTween;
        private Vector2 targetScale;
        private Vector2 defaultScale;
        private Vector2 defaultPosition;
        private bool isUpScaling = false;
        private float latestScaleTime;
        
        public void DoScale(bool isBig)
        {
            // 段階条件を満たしていない場合は何もしない
            if (!isBig && step <= minStep || isBig && step >= maxStep)
            {
                return;
            }

            // 目標のスケールを計算する
            int sign = isBig ? 1 : -1;
            targetScale += Vector2.one * scaleFactor * sign;
            targetScale.x = Mathf.Max(0f, targetScale.x);
            targetScale.y = Mathf.Max(0f, targetScale.y);

            step += sign;
            latestScaleTime = Time.time;

            // スケールを変更する
            DoAroundScale(targetScale);

            OnScale?.Invoke(GetScaleState());
        }

        protected void ResetScale()
        {
            // スケールを元に戻す
            DoAroundScale(defaultScale);
            latestScaleTime = Time.time;
            targetScale = defaultScale;
            step = defaultStep;
        }

        private int defaultStep;
        protected virtual void Start()
        {
            defaultStep = step;
            SetDefaultParams();
            latestScaleTime = Time.time;
        }

        // private void OnValidate()
        // {
        //     // エディタ側で段階を変更したときに反映する
        //     step = Mathf.Clamp(step, minStep, maxStep);
        //     SetDefaultParams();
        //
        //     Vector2 targetPivot = GetTargetPivot();
        //     ScaleAround(targetPivot, targetScale);
        // }

        private void SetDefaultParams()
        {
            targetScale = (Vector2)transform.localScale + Vector2.one * scaleFactor * step;
            defaultScale = targetScale;
            defaultPosition = transform.localPosition;
        }

        private State GetScaleState()
        {
            if (step == minStep)
            {
                return State.MinScale;
            }

            if (step == maxStep)
            {
                return State.MaxScale;
            }

            return State.InScale;
        }

        private void DoAroundScale(Vector2 targetScale)
        {
            // 現在のTweenを強制的に完了させる
            currentTween?.Complete();

            // targetScaleまで滑らかにスケールする
            Vector2 scale = transform.localScale;
            isUpScaling = targetScale.x > scale.x;
            currentTween = DOTween.To(() => scale,
                    value =>
                    {
                        scale = value;

                        // 左下を(0, 0)として指定したピボットを中心にスケールを変更
                        Vector2 targetPivot = GetTargetPivot();
                        ScaleAround(targetPivot, scale);
                    }, targetScale, scaleDuration)
                .SetEase(Ease.OutBack, 3f)
                .OnComplete(() => isUpScaling = false)
                .SetLink(gameObject);
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (!isUpScaling)
                return;

            GameObject obj = other.gameObject;

            if (obj.CompareTag("Player") && obj.TryGetComponent(out PlayerMovement movement))
            {
                // あたった面の法線の反対方向に力を加える (場合によっては変な方向になる)
                Vector2 bounceDirection = -other.GetContact(0).normal;
                movement.AddExternalForce(bounceDirection * bouncePower);
                OnBounce?.Invoke();
                isUpScaling = false;
            }
        }

        private Vector2 GetTargetPivot()
        {
            if (enablePivotScaling)
            {
                return defaultPosition + GetLocalPivot();
            }

            return transform.localPosition;
        }

        private Vector2 GetLocalPivot()
        {
            return (pivot - new Vector2(0.5f, 0.5f)) * defaultScale.x;
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

        /// <summary>
        /// スケール後の座標を算出する
        /// </summary>
        /// <param name="pivot"></param>
        /// <param name="newScale"></param>
        /// <returns></returns>
        private Vector2 CalculateScaledPosition(Vector2 pivot, Vector2 newScale)
        {
            Vector2 targetPos = transform.localPosition;
            Vector2 diff = targetPos - pivot;
            float relativeScale = newScale.x / transform.localScale.x;
            return pivot + diff * relativeScale;
        }
    }
}