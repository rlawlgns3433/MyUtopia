using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using Cysharp.Threading.Tasks;

public enum Dirs
{
    None,
    Up, Down
}

public class MultiTouch : MonoBehaviour
{
    public bool Tap { get; private set; }
    public Dirs Swipe { get; set; }

    private bool isTouching = false;
    private float timeTap = 0.25f;

    private float primaryStartTime = 0f;
    private Vector2 primaryStartPos;

    public float minSwipeDistanceInch = 0.25f;
    private float minSwipeDistancePixels;

    private float swipeTime = 0.25f;

    private void Awake()
    {
        EnhancedTouchSupport.Enable();
        float dpi = Screen.dpi;
        if (dpi == 0)
        {
            dpi = 96;
        }
        minSwipeDistancePixels = dpi * minSwipeDistanceInch;
        Touch.onFingerDown += HandleFingerDown;
        Touch.onFingerUp += HandleFingerUp;
    }

    private void OnDestroy()
    {
        Touch.onFingerDown -= HandleFingerDown;
        Touch.onFingerUp -= HandleFingerUp;
        EnhancedTouchSupport.Disable();
    }

    private void Update()
    {
#if UNITY_EDITOR
        SimulateTouchInput();
#endif
    }

    private void SimulateTouchInput()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleMouseDown();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            HandleMouseUp();
        }
    }

    private void HandleMouseDown()
    {
        if (!isTouching)
        {
            isTouching = true;
            primaryStartTime = Time.time;
            primaryStartPos = Mouse.current.position.ReadValue();
        }
    }

    private async void HandleMouseUp()
    {
        if (isTouching)
        {
            isTouching = false;
            var duration = Time.time - primaryStartTime;
            var endPos = Mouse.current.position.ReadValue();
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

    private void HandleFingerDown(Finger finger)
    {
        if (!isTouching)
        {
            isTouching = true;
            primaryStartTime = Time.time;
            primaryStartPos = finger.screenPosition;
        }
    }

    private async void HandleFingerUp(Finger finger)
    {
        if (isTouching)
        {
            isTouching = false;
            var duration = Time.time - primaryStartTime;
            var diff = finger.screenPosition - primaryStartPos;

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
