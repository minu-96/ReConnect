using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;

    [Header("Camera Settings")]
    public Camera playerCamera;
    public float maxLookAngle = 80f;

    private float verticalRotation = 0;
    private Vector3 velocity;
    private CharacterController characterController;

    void Start()
    {
        // CharacterController ������Ʈ ��������
        characterController = GetComponent<CharacterController>();

        // ī�޶� �������� �ʾҴٸ� Main Camera ã��
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        // ���콺 Ŀ�� ��ױ� (���� �߿��� Ŀ���� ������ ����)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();

        // ESC Ű�� ���콺 Ŀ�� ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
        }
    }

    void HandleMouseLook()
    {
        // ���콺 �Է� �ޱ�
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // ���� ȸ�� (Y��) - �÷��̾� ������Ʈ ȸ��
        transform.Rotate(Vector3.up * mouseX);

        // ���� ȸ�� (X��) - ī�޶� ȸ��
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    void HandleMovement()
    {
        // WASD �Է� �ޱ�
        float horizontal = Input.GetAxis("Horizontal"); // A, D Ű
        float vertical = Input.GetAxis("Vertical");     // W, S Ű

        // �̵� ���� ��� (�÷��̾ �ٶ󺸴� ���� ����)
        Vector3 direction = transform.right * horizontal + transform.forward * vertical;

        // �̵� ���� ���
        Vector3 move = direction * moveSpeed * Time.deltaTime;

        // CharacterController�� �̵�
        characterController.Move(move);
    }
}
