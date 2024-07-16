using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class FloorMove : MonoBehaviour
{
    public FloorSwipe touchManager;
    public float moveDistance = 10.0f;
    public float moveDuration = 0.5f;
    public int floorCount = 5;
    private int upCount = 0;
    private bool isMoving = false;
    private Vector3 targetPosition;

    private void Start()
    {
        targetPosition = transform.position;
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

    public async void MoveUp()
    {
        if (isMoving || upCount >= floorCount - 1)
            return;

        isMoving = true;
        touchManager.Swipe = Dirs.None;
        upCount++;
        await MoveFloor(new Vector3(0, moveDistance, 0));
        isMoving = false;
    }

    public async void MoveDown()
    {
        if (isMoving || upCount <= 0)
            return;

        isMoving = true;
        touchManager.Swipe = Dirs.None;
        upCount--;
        await MoveFloor(new Vector3(0, -moveDistance, 0));
        isMoving = false;
    }
}
