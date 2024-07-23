using UnityEngine;
using Cysharp.Threading.Tasks;
using Cinemachine;
using DG.Tweening;

public class FloorMove : MonoBehaviour
{
    //public MultiTouchManager touchManager;
    //private CinemachineVirtualCamera vc;
    //public float zoomSpeed = 5f;
    //public float dragSpeed = 0.15f;
    //private float maxZoomOut = 15f;
    //private float maxZoomIn = 10f;
    //public float zoomOutMaxValueZ = -9f;
    //private bool isDragging = false;
    //private bool isZooming = false;
    //public int moveDistance = 10;
    //public float moveDuration = 0.5f;
    //public int floorCount = 6;
    //private int upCount = 1;
    //public int UpCount
    //{
    //    get
    //    {
    //        return upCount;
    //    }
    //    set
    //    {
    //        upCount = value;
    //    }
    //}
    //private bool isMoving = false;
    //private Vector3 targetPosition;
    //private Vector3 defaultPosition;
    //private Vector3 zoomPosition;
    //public UiAnimalInventory uiAnimalInventory;

    //private void Awake()
    //{
    //    vc = GameObject.FindWithTag(Tags.VirtualCamera).GetComponent<CinemachineVirtualCamera>();
    //}

    //private void Start()
    //{
    //    if (touchManager == null)
    //    {
    //        touchManager = FindObjectOfType<MultiTouchManager>();
    //    }

    //    zoomPosition = defaultPosition = targetPosition = vc.transform.position;
    //    FloorManager.Instance.CurrentFloorIndex = upCount;
    //}

    //private void Update()
    //{
    //    if (!isMoving && !touchManager.Tap)
    //    {
    //        if (touchManager.Zoom != 0 && !isDragging)
    //        {
    //            if (touchManager.Zoom < 0 && vc.transform.position.y <= maxZoomOut)
    //            {
    //                ZoomOut();
    //            }
    //            else if (touchManager.Zoom > 0 && vc.transform.position.y >= maxZoomIn)
    //            {
    //                ZoomIn();
    //            }
    //        }

    //        if (touchManager.DragX != 0 && !touchManager.isZooming && touchManager.Swipe == Dirs.None)
    //        {
    //            Drag();
    //        }
    //        else if (touchManager.DragX == 0)
    //        {
    //            isDragging = false;
    //        }
    //    }

    //    if (!isMoving && touchManager.Swipe != Dirs.None && !touchManager.isZooming && !isDragging)
    //    {
    //        if (touchManager.Swipe == Dirs.Up)
    //        {
    //            MoveUp();
    //        }
    //        else if (touchManager.Swipe == Dirs.Down)
    //        {
    //            MoveDown();
    //        }
    //        touchManager.Swipe = Dirs.None;
    //    }
    //}

    //private async UniTask MoveFloor(Vector3 moveVector)
    //{
    //    targetPosition += moveVector;
    //    zoomPosition = targetPosition;
    //    maxZoomOut = targetPosition.y;
    //    maxZoomIn = targetPosition.y - 5;
    //    await vc.transform.DOMove(targetPosition, moveDuration).SetEase(Ease.InOutQuad).AsyncWaitForCompletion();
    //    Debug.Log($"CurrentFloor-MoveFloor{FloorManager.Instance.CurrentFloorIndex}/{upCount}->{targetPosition}");
    //}

    //public async UniTask MoveToCurrentFloor()
    //{
    //    var distance = FloorManager.Instance.CurrentFloorIndex - 1;
    //    distance *= moveDistance;
    //    var movePosition = new Vector3(0, defaultPosition.y - distance, defaultPosition.z);
    //    maxZoomOut = movePosition.y;
    //    maxZoomIn = movePosition.y - 5;
    //    upCount = FloorManager.Instance.CurrentFloorIndex;
    //    await vc.transform.DOMove(movePosition, moveDuration).SetEase(Ease.InOutQuad).AsyncWaitForCompletion();
    //    targetPosition = vc.transform.position;
    //    zoomPosition = targetPosition;
    //}

    //public async void MoveUp()
    //{
    //    if (isMoving || upCount >= floorCount)
    //        return;
    //    Debug.Log($"Swipe Up: {upCount}/{FloorManager.Instance.CurrentFloorIndex}");
    //    isMoving = true;
    //    upCount++;
    //    FloorManager.Instance.CurrentFloorIndex = upCount;
    //    uiAnimalInventory.UpdateInventory(false);
    //    await MoveFloor(new Vector3(0, -moveDistance, 0));
    //    isMoving = false;
    //}

    //public async void MoveDown()
    //{
    //    if (isMoving || upCount <= 1)
    //        return;
    //    Debug.Log($"Swipe Down: {upCount}/{FloorManager.Instance.CurrentFloorIndex}");
    //    isMoving = true;
    //    upCount--;
    //    FloorManager.Instance.CurrentFloorIndex = upCount;
    //    uiAnimalInventory.UpdateInventory(false);
    //    await MoveFloor(new Vector3(0, moveDistance, 0));
    //    isMoving = false;
    //}

    //private void ZoomIn()
    //{
    //    if (vc.transform.position.y >= maxZoomIn || vc.transform.position.z >= zoomOutMaxValueZ)
    //    {
    //        isZooming = true;
    //        Debug.Log($"ZoomIn{FloorManager.Instance.CurrentFloorIndex}-->/{maxZoomIn}//{maxZoomOut}");
    //        var forwardDirection = vc.transform.forward * zoomSpeed * Time.deltaTime;
    //        zoomPosition += forwardDirection;
    //        var zoomTargetPosition = new Vector3(zoomPosition.x, maxZoomIn, zoomOutMaxValueZ);
    //        vc.transform.position = Vector3.MoveTowards(vc.transform.position, zoomTargetPosition, zoomSpeed * Time.deltaTime);
    //        if (Vector3.Distance(vc.transform.position, zoomTargetPosition) < 0.01f)
    //        {
    //            isZooming = false;
    //            zoomPosition = zoomTargetPosition;
    //        }
    //    }
    //}

    //private void ZoomOut()
    //{
    //    if (vc.transform.position.y <= maxZoomOut || vc.transform.position.z >= defaultPosition.z)
    //    {
    //        isZooming = true;
    //        Debug.Log($"ZoomOut{FloorManager.Instance.CurrentFloorIndex}-->/{maxZoomIn}//{maxZoomOut}");
    //        var backwardDirection = -vc.transform.forward * zoomSpeed * Time.deltaTime;
    //        zoomPosition += backwardDirection;
    //        var zoomTargetPosition = targetPosition;
    //        vc.transform.position = Vector3.MoveTowards(vc.transform.position, zoomTargetPosition, zoomSpeed * Time.deltaTime);
    //        if (Vector3.Distance(vc.transform.position, zoomTargetPosition) < 0.01f)
    //        {
    //            isZooming = false;
    //            zoomPosition = targetPosition;
    //        }
    //    }
    //}

    //private void Drag()
    //{
    //    switch(FloorManager.Instance.CurrentFloorIndex)
    //    {
    //        case 1:
    //            dragSpeed = 0.3f;
    //            if(Mathf.Abs(vc.transform.position.x) <= 10)
    //            {
    //                isDragging = true;
    //                zoomPosition.x -= touchManager.DragX * dragSpeed * Time.deltaTime;
    //                vc.transform.position = zoomPosition;
    //            }
    //            else
    //            {
    //                if(vc.transform.position.x < 0)
    //                {
    //                    zoomPosition.x = -10;
    //                    vc.transform.position = zoomPosition;
    //                }
    //                else
    //                {
    //                    zoomPosition.x = 10;
    //                    vc.transform.position = zoomPosition;
    //                }
    //            }
    //            break;
    //        default:
    //            dragSpeed = 0.15f;
    //            if (Mathf.Abs(vc.transform.position.x) <= 5)
    //            {
    //                isDragging = true;
    //                zoomPosition.x -= touchManager.DragX * dragSpeed * Time.deltaTime;
    //                vc.transform.position = zoomPosition;
    //            }
    //            else
    //            {
    //                if (vc.transform.position.x < 0)
    //                {
    //                    zoomPosition.x = -5;
    //                    vc.transform.position = zoomPosition;
    //                }
    //                else
    //                {
    //                    zoomPosition.x = 5;
    //                    vc.transform.position = zoomPosition;
    //                }
    //            }
    //            break;
    //    }

    //}
}
