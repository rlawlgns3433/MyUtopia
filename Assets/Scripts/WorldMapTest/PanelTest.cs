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
        // 현재 활성화된 카메라 가져오기
        Camera activeCamera = cinemachineBrain.OutputCamera;

        // 마우스 또는 터치의 스크린 좌표를 가져옴 (혹은 원하는 지점의 스크린 좌표)
        Vector3 screenPosition = Input.mousePosition;

        // 스크린 좌표에서 Ray 생성
        Ray ray = activeCamera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Debug.Log($"Ray Hit: {hitInfo.point}"); // Ray가 맞은 월드 좌표

            // Ray가 맞은 월드 좌표를 스크린 좌표로 변환
            Vector3 hitScreenPosition = activeCamera.WorldToScreenPoint(hitInfo.point);

            // 스크린 좌표를 UI 캔버스의 로컬 좌표로 변환
            RectTransformUtility.ScreenPointToLocalPointInRectangle(test.rectTransform.parent as RectTransform, hitScreenPosition, activeCamera, out Vector2 localPoint);

            // 디버깅을 통해 좌표 변환 과정 점검
            Debug.Log($"Converted Local Point: {localPoint}");

            // 필요 시 오프셋이나 스케일 보정
            Vector2 adjustedPosition = localPoint;

            // UI 요소 위치를 업데이트
            test.rectTransform.anchoredPosition = adjustedPosition;

            Debug.Log($"UI Element moved to: {test.rectTransform.anchoredPosition}");
        }
        else
        {
            Debug.Log("Ray did not hit any objects.");
        }
    }
}
