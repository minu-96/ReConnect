using UnityEngine;
using TMPro;

public class EndingUIManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text summaryText;   // 결과 보여줄 Text (TMP)
    public GameObject panelRoot;   // 엔딩 루트 패널(없어도 무방)

    [Header("보기 좋은 이름(선택)")]
    public string[] displayNames = new string[8]
    {
        "선택지0","선택지1","선택지2","선택지3",
        "선택지4","선택지5","선택지6","선택지7"
    };

    void Start()
    {
        // 🔹 Static에서 결과 꺼내서 바로 표시
        if (GameResultData.finalLevels == null || GameResultData.maxLevels == null)
        {
            if (summaryText) summaryText.text = "결과 데이터가 없습니다.";
            return;
        }

        ShowEnding(GameResultData.finalLevels, GameResultData.maxLevels);

        // 필요하면 여기서 정리
        GameResultData.Clear();
    }

    public void ShowEnding(int[] finalLevels, int[] maxLevels)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        int count = finalLevels.Length;

        for (int i = 0; i < count; i++)
        {
            string name = (displayNames != null && i < displayNames.Length && !string.IsNullOrEmpty(displayNames[i]))
                          ? displayNames[i] : $"선택지{i}";
            int cur = finalLevels[i];
            int cap = (maxLevels != null && i < maxLevels.Length) ? maxLevels[i] : cur;

            sb.AppendLine($"{name} : Lv.{cur}/{cap}");
        }

        if (summaryText) summaryText.text = sb.ToString();
        if (panelRoot) panelRoot.SetActive(true);

        // 엔딩 화면에서는 커서 보이게 하고 싶으면:
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // 버튼용: 다시하기 / 메인으로
    public void OnClickRetry()
    {
        // 재시작 전에 정리
        GameResultData.Clear();
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene"); // 실제 게임 씬 이름
    }

    public void OnClickToMenu()
    {
        GameResultData.Clear();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu"); // 메인 메뉴 씬 이름
    }
}
