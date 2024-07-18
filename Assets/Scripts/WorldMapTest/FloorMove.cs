using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class FloorMove : MonoBehaviour
{
    public FloorSwipe touchManager;
    public int moveDistance = 10;
    public float moveDuration = 0.5f;
    public int floorCount = 6;
    private int upCount = 1;
    public int UpCount
    {
        get
        {
            return upCount;
        }
        set
        {
            upCount = value;
        }
    }
    private bool isMoving = false;
    private Vector3 targetPosition;
    private Vector3 defaultPosition;
    public UiAnimalInventory uiAnimalInventory;

    private void Start()
    {
        defaultPosition = targetPosition = transform.position;
        FloorManager.Instance.CurrentFloorIndex = upCount;
    }

    private void Update()
    {
        if (touchManager.Swipe == Dirs.Up)
        {
            MoveUp();
        }
        else if (touchManager.Swipe == Dirs.Down)
        {
            MoveDown();
        }
    }

    private async UniTask MoveFloor(Vector3 moveVector)
    {
        targetPosition += moveVector;
        await transform.DOMove(targetPosition, moveDuration).SetEase(Ease.InOutQuad).AsyncWaitForCompletion();
    }

    public async UniTask MoveToCurrentFloor()
    {

        var distance = FloorManager.Instance.CurrentFloorIndex - 1;
        distance *= moveDistance;
        var movePosition = new Vector3(0, defaultPosition.y + distance, 0);
        upCount = FloorManager.Instance.CurrentFloorIndex;
        await transform.DOMove(movePosition, moveDuration).SetEase(Ease.InOutQuad).AsyncWaitForCompletion();
        targetPosition = transform.position;
    }

    public async void MoveUp()
    {
        if (isMoving || upCount >= floorCount)
            return;
        Debug.Log($"1Swipe{upCount}/{FloorManager.Instance.CurrentFloorIndex}");
        isMoving = true;
        touchManager.Swipe = Dirs.None;
        upCount++;
        FloorManager.Instance.CurrentFloorIndex = upCount;
        uiAnimalInventory.SetFloor(FloorManager.Instance.GetCurrentFloor());
        uiAnimalInventory.UpdateInventory(false);
        await MoveFloor(new Vector3(0, moveDistance, 0));
        isMoving = false;
    }

    public async void MoveDown()
    {
        if (isMoving || upCount <= 1)
            return;
        Debug.Log($"2Swipe{upCount}/{FloorManager.Instance.CurrentFloorIndex}");
        isMoving = true;
        touchManager.Swipe = Dirs.None;
        upCount--;
        FloorManager.Instance.CurrentFloorIndex = upCount;
        uiAnimalInventory.SetFloor(FloorManager.Instance.GetCurrentFloor());
        uiAnimalInventory.UpdateInventory(false);
        await MoveFloor(new Vector3(0, -moveDistance, 0));
        isMoving = false;
    }
}
