using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace CS_Script.Naebo
{
    public class BallSimulator : MonoBehaviour
    {
       [SerializeField] private GameObject ballSimPrefab; // 何でもOK。予測位置を表示するオブジェクト

        private const int SIMULATE_COUNT = 20; // いくつ先までシュミレートするか
        private const float　GRAVITY_SCALE = 5f; // Rigidbody2DのGravityScale

        [SerializeField] private GameObject startPosition; // 発射開始位置
       
        private List<GameObject> _simuratePointList; 

        void Start()
        {
            Init();
        }

        void Update()
        {
            // デバッグ用に線を出してみる。必要無いなら無くても問題なし。
            if (_simuratePointList != null && _simuratePointList.Count > 0)
            {
                for (int i = 0; i < SIMULATE_COUNT; i++)
                {
                    if (i == 0)
                    {
                        Debug.DrawLine(startPosition.transform.position, _simuratePointList[i].transform.position);
                    }
                    else if (i < SIMULATE_COUNT)
                    {
                        Debug.DrawLine(_simuratePointList[i - 1].transform.position,
                            _simuratePointList[i].transform.position);
                    }
                }
            }

        }
        
        private void Init()
        {
            if (_simuratePointList != null && _simuratePointList.Count > 0)
            {
                foreach (var obj in _simuratePointList)
                {
                    Destroy(obj.gameObject);
                }
            }

            // 位置を表示するオブジェクトを予め作っておく
            if (ballSimPrefab != null)
            {
                _simuratePointList = new List<GameObject>();
                for (int i = 0; i < SIMULATE_COUNT; i++)
                {
                    var obj = Instantiate(ballSimPrefab);
                    obj.transform.SetParent(transform);
                    obj.transform.position = Vector3.zero;
                    _simuratePointList.Add(obj);
                }
            }
        }

        /*
          弾道を予測計算する。オブジェクトを再生成せず、位置だけ動かす。
          targetにはRigidbodyが必須
         */
        public void Simulate(Vector2 velocity)
        {
            if (!isSimulate)
                return;
            
            if (_simuratePointList != null && _simuratePointList.Count > 0)
            {
                Vector2 force = velocity;

                //弾道予測の位置に点を移動
                for (int i = 0; i < SIMULATE_COUNT; i++)
                {
                    var time = (i * 0.05f); // 〇秒ごとの位置を予測。
                    var x = time * force.x;
                    var y = (force.y * time) - 0.5f * (-Physics2D.gravity.y * GRAVITY_SCALE) * Mathf.Pow(time, 2.0f);
                    
                    _simuratePointList[i].transform.position = startPosition.transform.position + new Vector3(x, y, 0f);
                }
            }
        }

        private void RendererSwitch(bool value)
        {
            if (_simuratePointList != null && _simuratePointList.Count > 0)
            {
                foreach (var obj in _simuratePointList)
                {
                    obj.gameObject.GetComponent<Renderer>().enabled = value;
                }
            }
        }
        private bool isSimulate;
        public bool SimulateSwitch
        {
            set
            {
                isSimulate = value;
                if (value == false)
                {
                    RendererSwitch(false);
                }
                else
                {
                    RendererSwitch(true);
                }
            }
            get { return isSimulate; }
        }
    }
}