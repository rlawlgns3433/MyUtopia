using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public RectTransform targetButton; // 타겟 버튼
    public RectTransform panelLeft;    // 좌측 패널
    public RectTransform panelRight;   // 우측 패널
    public RectTransform panelUp;      // 상단 패널
    public RectTransform panelDown;    // 하단 패널
    public Canvas canvas;              // 기준이 되는 캔버스

    void Start()
    {
        UpdatePanels();
    }

    void UpdatePanels()
    {
        // 타겟 버튼의 월드 좌표를 가져오기
        Vector3[] worldCorners = new Vector3[4];
        targetButton.GetWorldCorners(worldCorners);

        // 월드 좌표를 캔버스 로컬 좌표로 변환
        Vector2 canvasLocalPosMin;
        Vector2 canvasLocalPosMax;

        RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, RectTransformUtility.WorldToScreenPoint(Camera.main, worldCorners[0]), canvas.worldCamera, out canvasLocalPosMin);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, RectTransformUtility.WorldToScreenPoint(Camera.main, worldCorners[2]), canvas.worldCamera, out canvasLocalPosMax);

        // 캔버스의 크기와 위치
        float canvasWidth = canvasRectTransform.rect.width;
        float canvasHeight = canvasRectTransform.rect.height;

        // 타겟 버튼의 크기와 위치 계산
        float left = canvasLocalPosMin.x;
        float right = canvasWidth - canvasLocalPosMax.x;
        float bottom = canvasLocalPosMin.y;
        float top = canvasHeight - canvasLocalPosMax.y;

        // 패널 위치 및 크기 설정
        panelLeft.sizeDelta = new Vector2(left, canvasHeight);
        panelLeft.anchoredPosition = new Vector2(left / 2, 0);

        panelRight.sizeDelta = new Vector2(right, canvasHeight);
        panelRight.anchoredPosition = new Vector2(-(right / 2), 0);

        panelUp.sizeDelta = new Vector2(canvasLocalPosMax.x - canvasLocalPosMin.x, top);
        panelUp.anchoredPosition = new Vector2((canvasLocalPosMin.x + canvasLocalPosMax.x) / 2 - canvasWidth / 2, -top / 2);

        panelDown.sizeDelta = new Vector2(canvasLocalPosMax.x - canvasLocalPosMin.x, bottom);
        panelDown.anchoredPosition = new Vector2((canvasLocalPosMin.x + canvasLocalPosMax.x) / 2 - canvasWidth / 2, bottom / 2);
    }
}
