using UnityEngine;
using UnityEngine.UI;

namespace GrowGame.Game
{
    /// <summary>
    /// 각 선택지(i)의 시각화 설정:
    /// - target: 해당 선택지의 결과물이 표시될 RawImage
    /// - levelTextures: [0..max] 레벨별 텍스처
    ///   * 0번,4번 선택지는 0~3까지(길이=4)
    ///   * 나머지는 0~5까지(길이=6) 권장
    /// </summary>
    [System.Serializable]
    public class ChoiceVisual
    {
        public RawImage target;
        public Texture[] levelTextures = new Texture[4]; // 기본 4칸(필요 시 에디터에서 늘리기)
    }

    /// <summary>
    /// choiceLevels 배열을 받아 맵(캔버스 RawImage들)을 갱신
    /// </summary>
    public class MapRenderer : MonoBehaviour
    {
        public static MapRenderer Instance;

        [Header("Visuals (index 0~7)")]
        public ChoiceVisual[] visuals = new ChoiceVisual[8];

        void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// 각 선택지의 레벨에 맞게 RawImage.texture 교체
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
                var tex = v.levelTextures[lvl];

                v.target.texture = tex;
                v.target.enabled = (tex != null); // 텍스처가 없으면 비활성화(투명)
            }
        }
    }
}
