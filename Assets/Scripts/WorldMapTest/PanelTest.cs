using Cinemachine;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public RectTransform button;
    public RectTransform button1;
    public RectTransform button2;
    public Button testbutton;
    public Image test;
    public GameObject target3DObject;
    public CinemachineBrain cinemachineBrain;
    private Vector2 screenPoint;
    private Vector2 screenPoint2;
    private Vector2 screenPoint3;

    void Start()
    {
        Vector3 absolutePosition = testbutton.transform.localPosition;
        Debug.Log($"buttonRectTransform => {button.position}");
        Debug.Log($"buttonLocalPosition => {absolutePosition}");
        Debug.Log($"buttonTransformPosition => {testbutton.transform.position}");
        screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, button.position);
        screenPoint2 = RectTransformUtility.WorldToScreenPoint(Camera.main, testbutton.transform.position);
        screenPoint3 = RectTransformUtility.WorldToScreenPoint(Camera.main, testbutton.transform.localPosition);
        Debug.Log($"screenPointRectPosition =>{screenPoint}");
        Debug.Log($"screenPointbuttonPosition =>{screenPoint2}");
        Debug.Log($"screenPointlocalPosition =>{screenPoint3}");
    }

    public void Set1()
    {
        CastRayAndMoveUI();
    }

    public void MatchButtonSizeAndPosition1()
    {
        RectTransform buttonRectTransform = button.GetComponent<RectTransform>();
        MatchImageToRectTransform(buttonRectTransform);
    }

    public void MatchButtonSizeAndPosition2()
    {
        RectTransform buttonRectTransform = button1.GetComponent<RectTransform>();
        MatchImageToRectTransform(buttonRectTransform);
    }

    public void MatchButtonSizeAndPosition3()
    {
        RectTransform buttonRectTransform = button2.GetComponent<RectTransform>();
        MatchImageToRectTransform(buttonRectTransform);
    }

    public void Set2()
    {
        MatchButtonSizeAndPosition1();
    }

    public void Set3()
    {
        MatchButtonSizeAndPosition2();
    }

    public void Set4()
    {
        MatchButtonSizeAndPosition3();
    }
    private void MatchImageToRectTransform(RectTransform buttonRectTransform)
    {
        test.rectTransform.position = buttonRectTransform.position;
        test.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        test.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        test.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        test.rectTransform.sizeDelta = buttonRectTransform.rect.size;
        test.rectTransform.localScale = buttonRectTransform.localScale;
    }

    private void CastRayAndMoveUI()
    {
        // ���� Ȱ��ȭ�� ī�޶� ��������
        Camera activeCamera = cinemachineBrain.OutputCamera;

        // ���콺 �Ǵ� ��ġ�� ��ũ�� ��ǥ�� ������ (Ȥ�� ���ϴ� ������ ��ũ�� ��ǥ)
        Vector3 screenPosition = Input.mousePosition;

        // ��ũ�� ��ǥ���� Ray ����
        Ray ray = activeCamera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Debug.Log($"Ray Hit: {hitInfo.point}"); // Ray�� ���� ���� ��ǥ

            // Ray�� ���� ���� ��ǥ�� ��ũ�� ��ǥ�� ��ȯ
            Vector3 hitScreenPosition = activeCamera.WorldToScreenPoint(hitInfo.point);

            // ��ũ�� ��ǥ�� UI ĵ������ ���� ��ǥ�� ��ȯ
            RectTransformUtility.ScreenPointToLocalPointInRectangle(test.rectTransform.parent as RectTransform, hitScreenPosition, activeCamera, out Vector2 localPoint);

            // ������� ���� ��ǥ ��ȯ ���� ����
            Debug.Log($"Converted Local Point: {localPoint}");

            // �ʿ� �� �������̳� ������ ����
            Vector2 adjustedPosition = localPoint;

            // UI ��� ��ġ�� ������Ʈ
            test.rectTransform.anchoredPosition = adjustedPosition;

            Debug.Log($"UI Element moved to: {test.rectTransform.anchoredPosition}");
        }
        else
        {
            Debug.Log("Ray did not hit any objects.");
        }
    }
}
