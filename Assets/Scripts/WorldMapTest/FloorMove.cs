using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using Cinemachine;

public class FloorMove : MonoBehaviour
{
    public MultiTouchManager touchManager;
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
    public CinemachineVirtualCamera vc;

    private void Awake()
    {
        vc = GameObject.FindWithTag(Tags.VirtualCamera).GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        if (touchManager == null)
        {
            touchManager = FindObjectOfType<MultiTouchManager>();
            if (touchManager == null)
            {
                Debug.LogError("MultiTouchManager is not assigned and could not be found in the scene.");
                return;
            }
        }

        defaultPosition = targetPosition = vc.transform.position;
        FloorManager.Instance.CurrentFloorIndex = upCount;
    }

    private void Update()
    {
        if (touchManager == null)
        {
            Debug.LogError("MultiTouchManager is not assigned.");
            return;
        }

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
        await vc.transform.DOMove(targetPosition, moveDuration).SetEase(Ease.InOutQuad).AsyncWaitForCompletion();
    }

    public async UniTask MoveToCurrentFloor()
    {
        var distance = FloorManager.Instance.CurrentFloorIndex - 1;
        distance *= moveDistance;
        var movePosition = new Vector3(0, defaultPosition.y + distance, defaultPosition.z);
        upCount = FloorManager.Instance.CurrentFloorIndex;
        await vc.transform.DOMove(movePosition, moveDuration).SetEase(Ease.InOutQuad).AsyncWaitForCompletion();
        targetPosition = vc.transform.position;
    }

    public async void MoveUp()
    {
        if (isMoving || upCount >= floorCount)
            return;
        Debug.Log($"Swipe Up: {upCount}/{FloorManager.Instance.CurrentFloorIndex}");
        isMoving = true;
        touchManager.Swipe = Dirs.None;
        upCount++;
        FloorManager.Instance.CurrentFloorIndex = upCount;
        uiAnimalInventory.UpdateInventory(false);
        await MoveFloor(new Vector3(0, -moveDistance, 0));
        isMoving = false;
    }

    public async void MoveDown()
    {
        if (isMoving || upCount <= 1)
            return;
        Debug.Log($"Swipe Down: {upCount}/{FloorManager.Instance.CurrentFloorIndex}");
        isMoving = true;
        touchManager.Swipe = Dirs.None;
        upCount--;
        FloorManager.Instance.CurrentFloorIndex = upCount;
        uiAnimalInventory.UpdateInventory(false);
        await MoveFloor(new Vector3(0, moveDistance, 0));
        isMoving = false;
    }
}
