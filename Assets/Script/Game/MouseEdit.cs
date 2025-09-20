using UnityEngine;
using UnityEngine.UI;

public class MouseEdit : MonoBehaviour
{
    public RectTransform cursorImage; // UI 이미지 (커서 모양)
    public Rect limitRect = new Rect(0, 0, 800, 600); // 스크린 좌표 기준

    //public Text tt;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            Debug.Log(Input.mousePosition);
        Vector3 pos = Input.mousePosition;

        // 범위 체크
        if (limitRect.Contains(new Vector2(pos.x, pos.y)))
        {
            if (!cursorImage.gameObject.activeSelf)
                cursorImage.gameObject.SetActive(true);

            // UI 좌표 = 스크린 좌표 그대로
            cursorImage.position = pos;
            //tt.text = pos.ToString();
        }
        else
        {
        }
    }
}
