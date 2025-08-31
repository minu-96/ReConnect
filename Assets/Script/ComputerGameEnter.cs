using UnityEngine;

public class ComputerGameEnter : MonoBehaviour
{
    [Header("Interaction Settings")]
    public string interactionText = "Press F to interact";
    public GameObject uiPanel; // 열릴 UI 패널

    private bool playerInRange = false;
    private bool uiOpen = false;

    void Update()
    {
        // 플레이어가 범위 안에 있고 F키를 눌렀을 때
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F키가 눌렸습니다!");
            ToggleUI();
            return;
        }
        // UI가 열려있을 때 ESC나 F키로 닫기
        if (uiOpen && (Input.GetKeyDown(KeyCode.Escape) || uiOpen && Input.GetKeyDown(KeyCode.F)))
        {
            CloseUI();
        }
    }

    void ToggleUI()
    {
        if (uiOpen)
        {
            CloseUI();
        }
        else
        {
            OpenUI();
        }
    }

    void OpenUI()
    {
        Debug.Log("OpenUI 호출됨");

        if (uiPanel != null)
        {
            Debug.Log("UI Panel 활성화 시도");
            uiPanel.SetActive(true);
            uiOpen = true;

            // InteractionPrompt 숨기기
            InteractionUI.Instance?.HideInteractionPrompt();

            // 마우스 커서 보이게 하고 잠금 해제
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // 플레이어 움직임 비활성화
            MonoBehaviour playerController = FindFirstObjectByType<CameraMove>();
            if (playerController != null)
            {
                playerController.enabled = false;
                Debug.Log("플레이어 이동 비활성화됨");
            }
            else
            {
                Debug.LogWarning("CameraMove 스크립트를 찾을 수 없습니다!");
            }
        }
        else
        {
            Debug.LogError("UI Panel이 null입니다!");
        }
    }

    void CloseUI()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
            uiOpen = false;

            // 플레이어가 아직 범위 안에 있다면 InteractionPrompt 다시 표시
            if (playerInRange)
            {
                InteractionUI.Instance?.ShowInteractionPrompt(interactionText);
            }

            // 마우스 커서 다시 잠그기
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // 플레이어 움직임 다시 활성화
            MonoBehaviour playerController = FindFirstObjectByType<CameraMove>();
            if (playerController != null)
            {
                playerController.enabled = true;
                Debug.Log("플레이어 이동 활성화됨");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            // 상호작용 UI 표시
            InteractionUI.Instance?.ShowInteractionPrompt(interactionText);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            // 상호작용 UI 숨기기
            InteractionUI.Instance?.HideInteractionPrompt();

            // UI가 열려있다면 닫기
            if (uiOpen)
            {
                CloseUI();
            }
        }
    }
}
