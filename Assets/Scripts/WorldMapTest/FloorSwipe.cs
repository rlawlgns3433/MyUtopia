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
            Debug.LogWarning("Screen DPI is 0, setting default DPI to 96.");
            dpi = 96; // 기본 DPI 값
        }
        minSwipeDistancePixels = dpi * minSwipeDistanceInch;
        Debug.Log($"DPI: {dpi}, Min Swipe Distance (pixels): {minSwipeDistancePixels}");

        // Input Actions 초기화
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
        Debug.Log("Touch Pressed");
        if (!isTouching)
        {
            isTouching = true;
            primaryStartTime = Time.time;
            primaryStartPos = touchPositionAction.ReadValue<Vector2>();
            Debug.Log($"Touch started at: {primaryStartPos}");
        }
    }

    private async void HandleTouchRelease(InputAction.CallbackContext context)
    {
        Debug.Log("Touch Released");
        if (isTouching)
        {
            isTouching = false;
            var duration = Time.time - primaryStartTime;
            var endPos = touchPositionAction.ReadValue<Vector2>();
            var diff = endPos - primaryStartPos;
            Debug.Log($"Touch ended at: {endPos}, duration: {duration}, diff: {diff}");

            if (duration < swipeTime)
            {
                if (diff.magnitude > minSwipeDistancePixels)
                {
                    if (Mathf.Abs(diff.x) < Mathf.Abs(diff.y))
                    {
                        Swipe = diff.y > 0 ? Dirs.Up : Dirs.Down;
                        Debug.Log($"Swipe detected: {Swipe}");
                    }
                }
            }

            if (duration < timeTap)
            {
                Tap = true;
                Debug.Log("Tap detected");
            }

            await UniTask.Delay(100);
            Tap = false;
            Swipe = Dirs.None;
        }
    }

    private void Update()
    {
#if UNITY_EDITOR
        SimulateTouchInput();
#endif
    }

    private void SimulateTouchInput()
    {
        if (Mouse.current == null)
        {
            Debug.LogWarning("Mouse.current is null.");
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleTouchPress(new InputAction.CallbackContext());
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            HandleTouchRelease(new InputAction.CallbackContext());
        }
    }
}
