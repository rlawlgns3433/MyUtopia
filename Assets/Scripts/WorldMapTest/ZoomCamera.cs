using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class ZoomCamera : MonoBehaviour
{
    public MultiTouchManager touchManager;
    private CinemachineVirtualCamera vc;
    public float zoomSpeed = 0.1f;
    public float zoomDuration = 0.5f;
    public float dragSpeed = 0.1f;
    private Vector3 targetPosition;
    private float minY = 10f;
    private float maxY = 15f;
    private bool isDragging = false;
    private bool isZooming = false;

    private void Awake()
    {
        vc = GameObject.FindWithTag(Tags.VirtualCamera).GetComponent<CinemachineVirtualCamera>();
        targetPosition = vc.transform.position;
    }

    private void Update()
    {
        if (!touchManager.Tap)
        {
            if (touchManager.Zoom != 0 && !isDragging)
            {
                if (touchManager.Zoom < 0 && vc.transform.position.y < maxY)
                {
                    ZoomOut();
                }
                else if (touchManager.Zoom > 0 && vc.transform.position.y >= minY)
                {
                    ZoomIn();
                }
            }

            if (touchManager.DragX != 0 && !isZooming)
            {
                Drag();
            }
        }
    }

    private void ZoomIn()
    {
        if (vc.transform.position.y >= 10)
        {
            isZooming = true;
            Debug.Log("ZoomIn");
            Vector3 forwardDirection = vc.transform.forward * zoomSpeed;
            targetPosition += forwardDirection;
            targetPosition.y = Mathf.Min(targetPosition.y, minY);
            //targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
            vc.transform.DOMove(targetPosition, zoomDuration).SetEase(Ease.InOutQuad);
            isZooming = false;
        }
        else
        {
            targetPosition.y = Mathf.Min(targetPosition.y, minY);
        }
    }

    private void ZoomOut()
    {
        if (vc.transform.position.y <= 15)
        {
            isZooming = true;
            Debug.Log("ZoomOut");
            Vector3 forwardDirection = vc.transform.forward * zoomSpeed;
            targetPosition -= forwardDirection;
            targetPosition.y = Mathf.Max(targetPosition.y, maxY);
            //targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
            vc.transform.DOMove(targetPosition, zoomDuration).SetEase(Ease.InOutQuad);
            isZooming=false;
        }
        else
        {
            targetPosition.y = Mathf.Max(targetPosition.y, maxY);
        }
    }

    private void Drag()
    {
        isDragging = true;
        Vector3 rightDirection = vc.transform.right * touchManager.DragX * dragSpeed;
        targetPosition += rightDirection;
        vc.transform.DOMove(targetPosition, zoomDuration).SetEase(Ease.InOutQuad);
        isDragging = false;
    }
}
