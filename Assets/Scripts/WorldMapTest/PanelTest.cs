using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public RectTransform targetButton; // Ÿ�� ��ư
    public RectTransform panelLeft;    // ���� �г�
    public RectTransform panelRight;   // ���� �г�
    public RectTransform panelUp;      // ��� �г�
    public RectTransform panelDown;    // �ϴ� �г�
    public Canvas canvas;              // ������ �Ǵ� ĵ����

    void Start()
    {
        UpdatePanels();
    }

    void UpdatePanels()
    {
        // Ÿ�� ��ư�� ���� ��ǥ�� ��������
        Vector3[] worldCorners = new Vector3[4];
        targetButton.GetWorldCorners(worldCorners);

        // ���� ��ǥ�� ĵ���� ���� ��ǥ�� ��ȯ
        Vector2 canvasLocalPosMin;
        Vector2 canvasLocalPosMax;

        RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, RectTransformUtility.WorldToScreenPoint(Camera.main, worldCorners[0]), canvas.worldCamera, out canvasLocalPosMin);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, RectTransformUtility.WorldToScreenPoint(Camera.main, worldCorners[2]), canvas.worldCamera, out canvasLocalPosMax);

        // ĵ������ ũ��� ��ġ
        float canvasWidth = canvasRectTransform.rect.width;
        float canvasHeight = canvasRectTransform.rect.height;

        // Ÿ�� ��ư�� ũ��� ��ġ ���
        float left = canvasLocalPosMin.x;
        float right = canvasWidth - canvasLocalPosMax.x;
        float bottom = canvasLocalPosMin.y;
        float top = canvasHeight - canvasLocalPosMax.y;

        // �г� ��ġ �� ũ�� ����
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
