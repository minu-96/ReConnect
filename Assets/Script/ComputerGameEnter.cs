using UnityEngine;

public class ComputerGameEnter : MonoBehaviour
{
    [Header("Interaction Settings")]
    public string interactionText = "Press F to interact";
    public GameObject uiPanel; // ���� UI �г�
    public GameObject uiPanel1; // ���� UI �г�

    private bool playerInRange = false;
    private bool uiOpen = false;

    void Update()
    {
        // �÷��̾ ���� �ȿ� �ְ� FŰ�� ������ ��
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            
            ToggleUI();
            return;
        }
        // UI�� �������� �� ESC�� FŰ�� �ݱ�
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
        

        if (uiPanel != null)
        {
            
            uiPanel.SetActive(true);
            uiOpen = true;

            // InteractionPrompt �����
            InteractionUI.Instance?.HideInteractionPrompt();

            // ���콺 Ŀ�� ���̰� �ϰ� ��� ����
            Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;

            // �÷��̾� ������ ��Ȱ��ȭ
            MonoBehaviour playerController = FindFirstObjectByType<CameraMove>();
            if (playerController != null)
            {
                playerController.enabled = false;
                Debug.Log("�÷��̾� �̵� ��Ȱ��ȭ��");
            }
            else
            {
                Debug.LogWarning("CameraMove ��ũ��Ʈ�� ã�� �� �����ϴ�!");
            }
        }
        else
        {
            Debug.LogError("UI Panel�� null�Դϴ�!");
        }

        if (uiPanel1 != null)
        {

            uiPanel1.SetActive(true);
            uiOpen = true;

            // InteractionPrompt �����
            InteractionUI.Instance?.HideInteractionPrompt();

            // ���콺 Ŀ�� ���̰� �ϰ� ��� ����
            Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;

            // �÷��̾� ������ ��Ȱ��ȭ
            MonoBehaviour playerController = FindFirstObjectByType<CameraMove>();
            if (playerController != null)
            {
                playerController.enabled = false;
                Debug.Log("�÷��̾� �̵� ��Ȱ��ȭ��");
            }
            else
            {
                Debug.LogWarning("CameraMove ��ũ��Ʈ�� ã�� �� �����ϴ�!");
            }
        }
        else
        {
            Debug.LogError("UI Panel�� null�Դϴ�!");
        }
    }

    void CloseUI()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
            uiOpen = false;

            // �÷��̾ ���� ���� �ȿ� �ִٸ� InteractionPrompt �ٽ� ǥ��
            if (playerInRange)
            {
                InteractionUI.Instance?.ShowInteractionPrompt(interactionText);
            }

            // ���콺 Ŀ�� �ٽ� ��ױ�
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // �÷��̾� ������ �ٽ� Ȱ��ȭ
            MonoBehaviour playerController = FindFirstObjectByType<CameraMove>();
            if (playerController != null)
            {
                playerController.enabled = true;
                Debug.Log("�÷��̾� �̵� Ȱ��ȭ��");
            }
        }

        if (uiPanel1 != null)
        {
            uiPanel1.SetActive(false);
            uiOpen = false;

            // �÷��̾ ���� ���� �ȿ� �ִٸ� InteractionPrompt �ٽ� ǥ��
            if (playerInRange)
            {
                InteractionUI.Instance?.ShowInteractionPrompt(interactionText);
            }

            // ���콺 Ŀ�� �ٽ� ��ױ�
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // �÷��̾� ������ �ٽ� Ȱ��ȭ
            MonoBehaviour playerController = FindFirstObjectByType<CameraMove>();
            if (playerController != null)
            {
                playerController.enabled = true;
                Debug.Log("�÷��̾� �̵� Ȱ��ȭ��");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            // ��ȣ�ۿ� UI ǥ��
            InteractionUI.Instance?.ShowInteractionPrompt(interactionText);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            // ��ȣ�ۿ� UI �����
            InteractionUI.Instance?.HideInteractionPrompt();

            // UI�� �����ִٸ� �ݱ�
            if (uiOpen)
            {
                CloseUI();
            }
        }
    }
}
