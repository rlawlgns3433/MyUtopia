using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiFocusOut : MonoBehaviour, IPointerClickHandler
{
    private Vector3 defaultPosition = new Vector3(0, 15, -15);
    private Vector3 focusOutRotation = new Vector3(40, 0, 0);
    private int offSet = 15;
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
    public void FocusOut()
    {
        if (transposer == null)
        {
            transposer = vc.GetCinemachineComponent<CinemachineTransposer>();
        }

        transposer.m_FollowOffset = new Vector3(0, 3, -2);
        var currentFloorIndex = FloorManager.Instance.CurrentFloorIndex - 1;
        var movePosition = new Vector3(0, defaultPosition.y - currentFloorIndex * offSet, -15);
        vc.transform.position = movePosition;
        vc.transform.rotation = Quaternion.Euler(focusOutRotation);
        //var floorMove = FloorManager.Instance.FloorMove;
        //await floorMove.MoveToCurrentFloor();

    }

    public void CloseAnimalFocus()
    {
        FocusOut();
        UnFollow();
        UiManager.Instance.ShowMainUi();
        UiManager.Instance.mainUi.animalInventoryUi.UpdateInventory(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        FocusOut();
        UnFollow();
        UiManager.Instance.ShowMainUi();
        UiManager.Instance.mainUi.animalInventoryUi.UpdateInventory(false);
    }
}
