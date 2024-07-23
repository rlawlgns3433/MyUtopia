using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class ZoomCamera : MonoBehaviour
{
    public MultiTouchManager touchManager;
    private CinemachineVirtualCamera vc;
    public float zoomSpeed = 1f;
    public float zoomDuration = 0.5f;
    public float dragSpeed = 0.1f;
    private Vector3 targetPosition;

    private float minY = 10f;
    private float maxY = 15f;

    private void Awake()
    {
        vc = GameObject.FindWithTag(Tags.VirtualCamera).GetComponent<CinemachineVirtualCamera>();
        targetPosition = vc.transform.position;
    }

    private void Update()
    {
        if (!touchManager.Tap && !touchManager.LongTap && !touchManager.DoubleTap)
        {
            if (touchManager.Zoom != 0)
            {
                if (touchManager.Zoom < 0 && vc.transform.position.y > minY)
                {
                    ZoomOut();
                }
                else if (touchManager.Zoom > 0 && vc.transform.position.y <= maxY)
                {
                    ZoomIn();
                }
            }

            if (touchManager.DragX != 0)
            {
                Drag();
            }
        }
    }

    private void ZoomIn()
    {
        if (vc.transform.position.y <= maxY)
        {
            Debug.Log("ZoomIn");
            Vector3 forwardDirection = vc.transform.forward * zoomSpeed;
            targetPosition += forwardDirection;
            targetPosition.y = Mathf.Min(targetPosition.y, maxY);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
            Debug.Log($"New Position: {targetPosition}");
            vc.transform.DOMove(targetPosition, zoomDuration).SetEase(Ease.InOutQuad);
        }
    }

    private void ZoomOut()
    {
        if (vc.transform.position.y > minY)
        {
            Debug.Log("ZoomOut");
            Vector3 forwardDirection = vc.transform.forward * zoomSpeed;
            targetPosition -= forwardDirection;
            targetPosition.y = Mathf.Max(targetPosition.y, minY);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
            Debug.Log($"New Position: {targetPosition}");
            vc.transform.DOMove(targetPosition, zoomDuration).SetEase(Ease.InOutQuad);
        }
    }

    private void Drag()
    {
        Debug.Log("Drag");
        Vector3 rightDirection = vc.transform.right * touchManager.DragX * dragSpeed;
        targetPosition += rightDirection;
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
        Debug.Log($"New Position: {targetPosition}");
        vc.transform.DOMove(targetPosition, zoomDuration).SetEase(Ease.InOutQuad);
    }
}
