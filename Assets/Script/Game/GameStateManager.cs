using UnityEngine;
using UnityEngine.UI;
using GrowGame.Rules;

namespace GrowGame.Game
{
    /// <summary>
    /// - 라운드/버튼/레벨 상태를 관리
    /// - 버튼 클릭 시: 해당 선택지 +1 → 규칙 적용 → 맵 갱신
    /// - 한 라운드 내 동일 선택지는 1회만 클릭 가능
    /// - 총 2라운드 × 8선택 = 16클릭
    /// </summary>
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance;

        [Header("Buttons (index 0~7)")]
        public Button[] choiceButtons = new Button[8];

        [Header("Max Levels per Choice (0~7)")]
        // 요구사항: [3,5,5,5,3,5,5,5]
        public int[] maxLevels = new int[8] { 3, 5, 5, 5, 3, 5, 5, 5 };

        [Header("State (read-only in Inspector)")]
        public int[] choiceLevels = new int[8];  // 현재 레벨(0부터 시작)
        public bool[] chosenThisRound = new bool[8];
        public int currentRound = 1;
        public int totalRounds = 2;
        public int turnsThisRound = 0; // 0..8

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            // CSV 규칙 로드
            RuleEngine.Init("rules");

            // 버튼 초기 상태(라운드 시작 시 미선택만 활성)
            RefreshButtons();

            // 초기 맵 반영
            if (MapRenderer.Instance != null)
                MapRenderer.Instance.UpdateMap(choiceLevels);
        }

        /// <summary>
        /// UI 버튼 OnClick()에서 index(0~7)를 넘겨 호출
        /// </summary>
        public void OnPressChoice(int index)
        {
            if (currentRound > totalRounds) { Debug.Log("게임 종료"); return; }
            if (index < 0 || index >= 8) return;

            // 이번 라운드에서 이미 눌렀다면 무시
            if (chosenThisRound[index]) return;

            // 1) 해당 선택지 레벨 +1 (개별 최대치로 클램프)
            int cap = (index >= 0 && index < maxLevels.Length) ? maxLevels[index] : 99;
            choiceLevels[index] = Mathf.Min(choiceLevels[index] + 1, cap);

            // 2) 규칙 적용(연쇄 반영이 멈출 때까지)
            RuleEngine.ApplyUntilStable(choiceLevels, maxLevels);

            // 3) 맵 갱신
            if (MapRenderer.Instance != null)
                MapRenderer.Instance.UpdateMap(choiceLevels);

            // 4) 라운드 진행 상태 갱신
            chosenThisRound[index] = true;
            turnsThisRound++;

            RefreshButtons();

            // 5) 라운드 종료 체크(8번 클릭)
            if (turnsThisRound >= 8)
            {
                currentRound++;
                turnsThisRound = 0;

                if (currentRound <= totalRounds)
                {
                    // 다음 라운드 시작: 선택 가능 상태 초기화
                    for (int i = 0; i < chosenThisRound.Length; i++)
                        chosenThisRound[i] = false;

                    RefreshButtons();
                    Debug.Log($"라운드 {currentRound} 시작");
                }
                else
                {
                    Debug.Log("모든 라운드 종료");
                    // TODO: 엔딩/점수/저장 등
                }
            }
        }

        /// <summary>
        /// 이번 라운드에서 아직 선택하지 않은 버튼만 활성화
        /// </summary>
        void RefreshButtons()
        {
            for (int i = 0; i < choiceButtons.Length; i++)
            {
                if (choiceButtons[i] == null) continue;
                bool interactable = (currentRound <= totalRounds) && !chosenThisRound[i];
                choiceButtons[i].interactable = interactable;
            }
        }
    }
}
