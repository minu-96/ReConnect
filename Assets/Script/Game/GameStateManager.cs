using UnityEngine;
using UnityEngine.UI;
using GrowGame.Rules;
using UnityEngine.SceneManagement;

namespace GrowGame.Game
{
    /// <summary>
    /// - ����/��ư/���� ���¸� ����
    /// - ��ư Ŭ�� ��: �ش� ������ +1 �� ��Ģ ���� �� �� ����
    /// - �� ���� �� ���� �������� 1ȸ�� Ŭ�� ����
    /// - �� 2���� �� 8���� = 16Ŭ��
    /// </summary>
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance;

        [Header("Buttons (index 0~7)")]
        public Button[] choiceButtons = new Button[8];

        [Header("Max Levels per Choice (0~7)")]
        // �䱸����: [3,5,5,5,3,5,5,5]
        public int[] maxLevels = new int[8] { 3, 5, 5, 5, 3, 5, 5, 5 };

        [Header("State (read-only in Inspector)")]
        public int[] choiceLevels = new int[8];  // ���� ����(0���� ����)
        public bool[] chosenThisRound = new bool[8];
        public int currentRound = 1;
        public int totalRounds = 2;
        public int turnsThisRound = 0; // 0..8

        public Text text;
        public Text text1;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            // CSV ��Ģ �ε�
            RuleEngine.Init("rules");

            // ��ư �ʱ� ����(���� ���� �� �̼��ø� Ȱ��)
            RefreshButtons();

            // �ʱ� �� �ݿ�
            if (MapRenderer.Instance != null)
                MapRenderer.Instance.UpdateMap(choiceLevels);
        }

        private void Update()
        {
            if(currentRound == 3)
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene("Ending0");
            }
            
            text.text = currentRound.ToString();
        }

        /// <summary>
        /// UI ��ư OnClick()���� index(0~7)�� �Ѱ� ȣ��
        /// </summary>
        public void OnPressChoice(int index)
        {
            if (currentRound > totalRounds) { Debug.Log("���� ����"); return; }
            if (index < 0 || index >= 8) return;

            // �̹� ���忡�� �̹� �����ٸ� ����
            if (chosenThisRound[index]) return;

            // 1) �ش� ������ ���� +1 (���� �ִ�ġ�� Ŭ����)
            int cap = (index >= 0 && index < maxLevels.Length) ? maxLevels[index] : 99;
            choiceLevels[index] = Mathf.Min(choiceLevels[index] + 1, cap);

            // 2) ��Ģ ����(���� �ݿ��� ���� ������)
            RuleEngine.ApplyUntilStable(choiceLevels, maxLevels);

            // 3) �� ����
            if (MapRenderer.Instance != null)
                MapRenderer.Instance.UpdateMap(choiceLevels);

            // 4) ���� ���� ���� ����
            chosenThisRound[index] = true;
            turnsThisRound++;

            RefreshButtons();

            // 5) ���� ���� üũ(8�� Ŭ��)
            if (turnsThisRound >= 8)
            {
                currentRound++;
                turnsThisRound = 0;

                GameResultData.finalLevels = (int[])choiceLevels.Clone();
                GameResultData.maxLevels = (int[])maxLevels.Clone();

                if (currentRound <= totalRounds)
                {
                    // ���� ���� ����: ���� ���� ���� �ʱ�ȭ
                    for (int i = 0; i < chosenThisRound.Length; i++)
                        chosenThisRound[i] = false;

                    RefreshButtons();
                    Debug.Log($"���� {currentRound} ����");
                }
                else
                {
                    Debug.Log("��� ���� ����");
                    // TODO: ����/����/���� ��
                }
                

            }
            
        }

        /// <summary>
        /// �̹� ���忡�� ���� �������� ���� ��ư�� Ȱ��ȭ
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
