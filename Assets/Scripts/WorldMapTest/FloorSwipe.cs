using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

public enum Dirs
{
    None,
    Up,
    Down
}

public class FloorSwipe : MonoBehaviour
{
    public InputActionAsset inputActions;
    public bool Tap { get; set; }
    public Dirs Swipe { get; set; }

    private bool isTouching = false;
    private float timeTap = 0.2f;

    private float primaryStartTime = 0f;
    private Vector2 primaryStartPos;

    public float minSwipeDistanceInch = 0.25f;
    private float minSwipeDistancePixels;

    private float swipeTime = 0.2f;

    private InputAction touchPressAction;
    private InputAction touchPositionAction;

    private void Awake()
    {
        float dpi = Screen.dpi;
        if (dpi == 0)
        {
            dpi = 96; // ±âº» DPI °ª
        }
        minSwipeDistancePixels = dpi * minSwipeDistanceInch;
        Debug.Log($"DPI: {dpi}, Min Swipe Distance (pixels): {minSwipeDistancePixels}");

        var swipeActionMap = inputActions.FindActionMap("Swipe");
        touchPressAction = swipeActionMap.FindAction("Press");
        touchPositionAction = swipeActionMap.FindAction("Position");

        touchPressAction.performed += HandleTouchPress;
        touchPressAction.canceled += HandleTouchRelease;

        touchPressAction.Enable();
        touchPositionAction.Enable();
        Tap = false;
        isTouching = false;
        Swipe = Dirs.None;
        primaryStartTime = 0f;
        primaryStartPos = Vector2.zero;
    }

    private void OnDestroy()
    {
        touchPressAction.performed -= HandleTouchPress;
        touchPressAction.canceled -= HandleTouchRelease;

        touchPressAction.Disable();
        touchPositionAction.Disable();
    }

    private void HandleTouchPress(InputAction.CallbackContext context)
    {
        if (!isTouching)
        {
            isTouching = true;
            primaryStartTime = Time.time;
            primaryStartPos = touchPositionAction.ReadValue<Vector2>();
            Debug.Log($"Touch started at {primaryStartPos} at time {primaryStartTime}");
        }
    }

    private async void HandleTouchRelease(InputAction.CallbackContext context)
    {
        if (isTouching)
        {
            isTouching = false;
            var duration = Time.time - primaryStartTime;
            var endPos = touchPositionAction.ReadValue<Vector2>();
            var diff = endPos - primaryStartPos;

            bool isSwipe = duration < swipeTime && diff.magnitude > minSwipeDistancePixels;
            bool isTap = duration < timeTap && diff.magnitude < minSwipeDistancePixels;

            if (isSwipe)
            {
                if (Mathf.Abs(diff.x) < Mathf.Abs(diff.y))
                {
                    Swipe = diff.y > 0 ? Dirs.Up : Dirs.Down;
                }
            }
            else if (isTap)
            {
                Tap = true;
            }

            await UniTask.Delay(100);
            Tap = false;
            Swipe = Dirs.None;
        }
    }
}
