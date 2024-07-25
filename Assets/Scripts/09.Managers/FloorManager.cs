using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : Singleton<FloorManager>
{
    public Dictionary<string, Floor> floors = new Dictionary<string, Floor>();
    private int currentFloorIndex = 1;
    public int CurrentFloorIndex
    {
        get
        {
            return currentFloorIndex;
        }
        set
        {
            currentFloorIndex = value;
        }
    }
    public MultiTouchManager touchManager;
    private CinemachineVirtualCamera vc;
    public float zoomSpeed = 5f;
    public float dragSpeed = 0.15f;
    private float maxZoomOut = 15f;
    private float maxZoomIn = 10f;
    public float zoomOutMaxValueZ = -9f;
    private bool isDragging = false;
    public int moveDistance = 10;
    public float moveDuration = 0.5f;
    public int floorCount = 5;
    public bool isMoving = false;
    private bool isZoomIn = false;
    public bool multiTouchOff = false;
    private Vector3 targetPosition;
    private Vector3 defaultPosition;
    private Vector3 zoomPosition;
    public UiAnimalInventory uiAnimalInventory;
    private void Awake()
    {
        vc = GameObject.FindWithTag(Tags.VirtualCamera).GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        if (touchManager == null)
        {
            touchManager = FindObjectOfType<MultiTouchManager>();
        }

        zoomPosition = defaultPosition = targetPosition = vc.transform.position;
    }

    private void Update()
    {
        if (!multiTouchOff)
        {
            if (!isMoving && !touchManager.Tap)
            {
                if (touchManager.Zoom != 0 && !isDragging)
                {
                    if (touchManager.Zoom < 0 && vc.transform.position.y <= maxZoomOut)
                    {
                        ZoomOut();
                    }
                    else if (touchManager.Zoom > 0 && vc.transform.position.y >= maxZoomIn)
                    {
                        ZoomIn();
                    }
                }

                if (touchManager.DragX != 0 && !touchManager.isZooming && touchManager.Swipe == Dirs.None)
                {
                    Drag().Forget();
                }
                else if (touchManager.DragX == 0)
                {
                    isDragging = false;
                }
            }

            if (!isMoving && touchManager.Swipe != Dirs.None && !touchManager.isZooming && !isDragging)
            {
                if (touchManager.Swipe == Dirs.Up)
                {
                    MoveUp();
                }
                else if (touchManager.Swipe == Dirs.Down)
                {
                    MoveDown();
                }
                touchManager.Swipe = Dirs.None;
            }
        }
    }

    private async UniTask MoveFloor(Vector3 moveVector)
    {
        targetPosition += moveVector;
        zoomPosition = targetPosition;
        maxZoomOut = targetPosition.y;
        maxZoomIn = targetPosition.y - 5;
        await vc.transform.DOMove(targetPosition, moveDuration).SetEase(Ease.InOutQuad).AsyncWaitForCompletion();
        Debug.Log($"CurrentFloor-MoveFloor{CurrentFloorIndex}/{targetPosition}");
    }

    public async UniTask MoveToCurrentFloor()
    {
        var distance = CurrentFloorIndex - 1;
        distance *= moveDistance;
        var movePosition = new Vector3(0, defaultPosition.y - distance, defaultPosition.z);
        maxZoomOut = movePosition.y;
        maxZoomIn = movePosition.y - 5;
        await vc.transform.DOMove(movePosition, moveDuration).SetEase(Ease.InOutQuad).AsyncWaitForCompletion();
        targetPosition = vc.transform.position;
        zoomPosition = targetPosition;
    }

    public async void MoveUp()
    {
        if (isMoving || CurrentFloorIndex == floorCount)
            return;
        else if (CurrentFloorIndex < floorCount)
        {
            Debug.Log($"Swipe Up:{CurrentFloorIndex}");
            isMoving = true;
            CurrentFloorIndex++;
            uiAnimalInventory.UpdateInventory(false);
            await MoveFloor(new Vector3(0, -moveDistance, 0));
            isMoving = false;
        }

    }

    public async void MoveDown()
    {
        if (isMoving || CurrentFloorIndex == 1)
            return;
        else if (CurrentFloorIndex > 1)
        {
            Debug.Log($"Swipe Down:/{CurrentFloorIndex}");
            isMoving = true;
            CurrentFloorIndex--;
            uiAnimalInventory.UpdateInventory(false);
            await MoveFloor(new Vector3(0, moveDistance, 0));
            isMoving = false;
        }
    }

    private void ZoomIn()
    {
        if (vc.transform.position.y >= maxZoomIn || vc.transform.position.z >= zoomOutMaxValueZ)
        {
            isZoomIn = true;
            Debug.Log($"ZoomIn{CurrentFloorIndex}-->/{maxZoomIn}//{maxZoomOut}");
            var forwardDirection = vc.transform.forward * zoomSpeed * Time.deltaTime;
            zoomPosition += forwardDirection;
            var zoomTargetPosition = new Vector3(zoomPosition.x, maxZoomIn, zoomOutMaxValueZ);
            vc.transform.position = Vector3.MoveTowards(vc.transform.position, zoomTargetPosition, zoomSpeed * Time.deltaTime);
            if (Vector3.Distance(vc.transform.position, zoomTargetPosition) < 0.01f)
            {
                zoomPosition = zoomTargetPosition;
            }
        }
    }

    private void ZoomOut()
    {
        if (vc.transform.position.y <= maxZoomOut || vc.transform.position.z >= defaultPosition.z)
        {
            Debug.Log($"ZoomOut{CurrentFloorIndex}-->/{maxZoomIn}//{maxZoomOut}");
            var backwardDirection = -vc.transform.forward * zoomSpeed * Time.deltaTime;
            zoomPosition += backwardDirection;
            var zoomTargetPosition = targetPosition;
            vc.transform.position = Vector3.MoveTowards(vc.transform.position, zoomTargetPosition, zoomSpeed * Time.deltaTime);
            if (Vector3.Distance(vc.transform.position, zoomTargetPosition) < 0.01f)
            {
                zoomPosition = targetPosition;
                isZoomIn = false;
            }
        }
    }

    private async UniTask Drag()
    {
        float dragAmount = touchManager.DragX < 0 ? dragSpeed : -dragSpeed;

        switch (CurrentFloorIndex)
        {
            case 1:
                dragSpeed = isZoomIn ? 0.5f : 1;
                zoomPosition.x = Mathf.Clamp(zoomPosition.x + dragAmount, -10f, 10f);
                break;
            default:
                dragSpeed = isZoomIn ? 0.25f : 0.5f;
                zoomPosition.x = Mathf.Clamp(zoomPosition.x + dragAmount, -5f, 5f);
                break;
        }

        if (!isDragging)
        {
            isDragging = true;
            await vc.transform.DOMove(new Vector3(zoomPosition.x, zoomPosition.y, zoomPosition.z), 0.1f).SetEase(Ease.InOutQuad).AsyncWaitForCompletion();
            isDragging = false;
        }
    }

    public void AddFloor(string floorId, Floor floor)
    {
        if (floors.ContainsKey(floorId))
            return;

        floors[floorId] = floor;
    }

    public bool MoveAnimal(string fromFloor, string toFloor, Animal animal)
    {
        if (!floors.ContainsKey(fromFloor))
            return false;

        if (!floors.ContainsKey(toFloor))
            return false;

        var currentFloor = GetCurrentFloor();
        if (currentFloor.animals.Count >= currentFloor.FloorStat.Max_Population)
            return false;

        animal.animalWork.Animal.animalStat.CurrentFloor = toFloor;
        floors[toFloor].animals.Add(animal);
        animal.animalWork.MoveFloor();
        floors[fromFloor].animals.Remove(animal);
        UiManager.Instance.animalFocusUi.Set();
        return true;
    }

    public Floor GetFloor(string floorId)
    {
        if (!floors.ContainsKey(floorId))
            return null;
        return floors[floorId];
    }

    public void SetFloor(string floorId)
    {
        CurrentFloorIndex = int.Parse(floorId[1].ToString());
    }

    public Floor GetCurrentFloor()
    {
        var floorId = $"B{CurrentFloorIndex}";
        if (!floors.ContainsKey(floorId))
            return null;
        return floors[floorId];
    }

    public void LevelUp(string floorId)
    {
        if (!floors.ContainsKey(floorId))
            return;
        floors[floorId].LevelUp();
    }

    public void LevelUp()
    {
        if (GetCurrentFloor() != null)
        {
            floors[$"B{CurrentFloorIndex}"].LevelUp();
        }
    }
}
