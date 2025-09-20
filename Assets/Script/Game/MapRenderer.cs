using UnityEngine;
using UnityEngine.UI;

namespace GrowGame.Game
{
    /// <summary>
    /// �� ������(i)�� �ð�ȭ ����:
    /// - target: �ش� �������� ������� ǥ�õ� RawImage
    /// - levelTextures: [0..max] ������ �ؽ�ó
    ///   * 0��,4�� �������� 0~3����(����=4)
    ///   * �������� 0~5����(����=6) ����
    /// </summary>
    [System.Serializable]
    public class ChoiceVisual
    {
        public RawImage target;
        public Texture[] levelTextures = new Texture[4]; // �⺻ 4ĭ(�ʿ� �� �����Ϳ��� �ø���)
        public GameObject upgrade;
    }

    /// <summary>
    /// choiceLevels �迭�� �޾� ��(ĵ���� RawImage��)�� ����
    /// </summary>
    public class MapRenderer : MonoBehaviour
    {
        public static MapRenderer Instance;

        [Header("Visuals (index 0~7)")]
        public ChoiceVisual[] visuals = new ChoiceVisual[8];

        // 이전 프레임의 레벨 기록 (상승 감지용)
        private int[] _lastLevels;

        void Awake()
        {
            Instance = this;
            _lastLevels = new int[visuals.Length];
            for (int i = 0; i < _lastLevels.Length; i++)
                _lastLevels[i] = -1; // 처음 갱신 때 무조건 새로 그림
        }

        /// <summary>
        /// �� �������� ������ �°� RawImage.texture ��ü
        /// </summary>
        public void UpdateMap(int[] choiceLevels)
        {
            if (choiceLevels == null || choiceLevels.Length < 8) return;

            for (int i = 0; i < Mathf.Min(visuals.Length, choiceLevels.Length); i++)
            {
                var v = visuals[i];
                
                if (v == null || v.target == null || v.levelTextures == null || v.levelTextures.Length == 0)
                    continue;

                int lvl = Mathf.Clamp(choiceLevels[i], 0, v.levelTextures.Length - 1);

                if (lvl >= 1)
                {
                    Animator anim = v.target.GetComponent<Animator>();
                    if (anim != null)
                    {
                        anim.SetTrigger("Drop");
                    }
                    StartCoroutine(UpgradeManager.Instance.StopGame());
                }


                

                var tex = v.levelTextures[lvl];

                v.target.texture = tex;
                v.target.enabled = (tex != null); // �ؽ�ó�� ������ ��Ȱ��ȭ(����)

                // ▲ 레벨 상승 감지: (이전보다 커졌을 때만)
                bool leveledUp = (_lastLevels[i] >= 1) && (lvl > _lastLevels[i]);
                if (leveledUp)
                {
                    v.upgrade.SetActive(true);
                    StartCoroutine(UpgradeManager.Instance.StopText(v.upgrade));
                }

                // 마지막에 현재 레벨 저장
                _lastLevels[i] = lvl;
            }
        }
    }
}
