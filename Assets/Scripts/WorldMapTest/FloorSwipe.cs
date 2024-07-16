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
    public bool Tap { get; private set; }
    public Dirs Swipe { get; set; }

    private bool isTouching = false;
    private float timeTap = 0.25f;

    private float primaryStartTime = 0f;
    private Vector2 primaryStartPos;

    public float minSwipeDistanceInch = 0.25f;
    private float minSwipeDistancePixels;

    private float swipeTime = 0.25f;

    private InputAction touchPressAction;
    private InputAction touchPositionAction;

    private void Awake()
    {
        float dpi = Screen.dpi;
        if (dpi == 0)
        {
            dpi = 96;
        }

        minSwipeDistancePixels = dpi * minSwipeDistanceInch;
        touchPressAction = inputActions.FindActionMap("Swipe").FindAction("Press");
        touchPositionAction = inputActions.FindActionMap("Swipe").FindAction("Position");

        touchPressAction.performed += HandleTouchPress;
        touchPressAction.canceled += HandleTouchRelease;
        touchPressAction.Enable();
        touchPositionAction.Enable();
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
            if (duration < swipeTime)
            {
                if (diff.magnitude > minSwipeDistancePixels)
                {
                    if (Mathf.Abs(diff.x) < Mathf.Abs(diff.y))
                    {
                        Swipe = diff.y > 0 ? Dirs.Up : Dirs.Down;
                    }
                }
            }
            if (duration < timeTap)
            {
                Tap = true;
            }
            await UniTask.Delay(100);
            Tap = false;
            Swipe = Dirs.None;
        }
    }
}
