using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiFocusOut : MonoBehaviour, IPointerClickHandler
{
    private Vector3 focusOutPosition = new Vector3(0, 15, -15);
    private Vector3 focusOutRotation = new Vector3(40, 0, 0);

    public CinemachineVirtualCamera vc;
    private CinemachineTransposer transposer;

    private void Awake()
    {
        vc = GameObject.FindWithTag(Tags.VirtualCamera).GetComponent<CinemachineVirtualCamera>();
    }

    private void UnFollow()
    {
        vc.Follow = null;
        vc.LookAt = null;
    }
    private void FocusOut()
    {
        if (transposer == null)
        {
            transposer = vc.GetCinemachineComponent<CinemachineTransposer>();
        }

        transposer.m_FollowOffset = new Vector3(0, 3, -2);
        vc.transform.position = focusOutPosition;
        vc.transform.rotation = Quaternion.Euler(focusOutRotation);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        FocusOut();
        UnFollow();
        UiManager.Instance.ShowMainUi();
    }
}
