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
        // CharacterController 컴포넌트 가져오기
        characterController = GetComponent<CharacterController>();

        // 카메라가 설정되지 않았다면 Main Camera 찾기
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        // 마우스 커서 잠그기 (게임 중에는 커서가 보이지 않음)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();

        // ESC 키로 마우스 커서 해제
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void HandleMouseLook()
    {
        // 마우스 입력 받기
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // 수평 회전 (Y축) - 플레이어 오브젝트 회전
        transform.Rotate(Vector3.up * mouseX);

        // 수직 회전 (X축) - 카메라만 회전
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    void HandleMovement()
    {
        // WASD 입력 받기
        float horizontal = Input.GetAxis("Horizontal"); // A, D 키
        float vertical = Input.GetAxis("Vertical");     // W, S 키

        // 이동 방향 계산 (플레이어가 바라보는 방향 기준)
        Vector3 direction = transform.right * horizontal + transform.forward * vertical;

        // 이동 벡터 계산
        Vector3 move = direction * moveSpeed * Time.deltaTime;

        // CharacterController로 이동
        characterController.Move(move);
    }
}
