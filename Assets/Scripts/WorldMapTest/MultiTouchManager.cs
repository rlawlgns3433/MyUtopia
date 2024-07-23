using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public enum Dirs
{
    None,
    Up, Down
}

public class MultiTouchManager : MonoBehaviour
{
    public bool Tap { get; private set; }
    public bool LongTap { get; private set; }
    public bool DoubleTap { get; private set; }

    public Dirs Swipe { get; set; }
    public float Zoom { get; private set; }
    public float DragX { get; private set; }

    private Finger primayFinger = null;
    private bool isZooming = false;

    private float timeTap = 0.25f;
    private float timeLongTap = 0.5f;
    private float timeDoubleTap = 0.25f;

    private float primayStartTime = 0f;
    private Vector2 primayStartPos;
    private Vector2 previousPos;

    private bool isFirstTap = false;
    private float firstTapTime = 0f;

    public float minSwipeDistanceInch = 0.25f; // Inch
    private float minSwipeDistancePixels;

    private float swipeTime = 0.25f;

    public float zoomMaxInch = 1f; // Inch
    private float zoomMaxPixel;

    private void Awake()
    {
        EnhancedTouchSupport.Enable();

        minSwipeDistancePixels = Screen.dpi * minSwipeDistanceInch;
        zoomMaxPixel = Screen.dpi * zoomMaxInch;
    }

    private void OnDestroy()
    {
        EnhancedTouchSupport.Disable();
    }

    private void Update()
    {
        Tap = false;
        LongTap = false;
        DoubleTap = false;
        Swipe = Dirs.None;
        Zoom = 0f;
        DragX = 0f;

        if (Touch.activeTouches.Count == 2)
        {
            isZooming = true;
            HandleZoom();
        }
        else
        {
            isZooming = false;
            HandleSingleTouch();
        }
    }

    private void HandleSingleTouch()
    {
        foreach (var touch in Touch.activeTouches)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (primayFinger == null)
                    {
                        primayFinger = touch.finger;
                        primayStartTime = Time.time;
                        primayStartPos = touch.screenPosition;
                        previousPos = touch.screenPosition;
                    }
                    break;
                case TouchPhase.Moved:
                    if (primayFinger == touch.finger)
                    {
                        DragX = touch.screenPosition.x - previousPos.x;
                        previousPos = touch.screenPosition;
                    }
                    break;
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (primayFinger == touch.finger)
                    {
                        primayFinger = null;
                        var duration = Time.time - primayStartTime;

                        if (duration < timeTap)
                        {
                            Tap = true;

                            if (isFirstTap && Time.time - firstTapTime > timeDoubleTap)
                            {
                                isFirstTap = false;
                            }

                            if (!isFirstTap)
                            {
                                isFirstTap = true;
                                firstTapTime = Time.time;
                            }
                            else
                            {
                                DoubleTap = Time.time - firstTapTime < timeDoubleTap;
                                isFirstTap = false;
                            }
                        }

                        if (duration > timeLongTap)
                        {
                            LongTap = true;
                        }
                    }
                    break;
            }
        }
    }

    private void HandleZoom()
    {
        var first = Touch.activeTouches[0];
        var second = Touch.activeTouches[1];

        if ((first.phase == TouchPhase.Moved || first.phase == TouchPhase.Stationary) &&
            (second.phase == TouchPhase.Moved || second.phase == TouchPhase.Stationary))
        {
            var firstPrevPos = first.screenPosition - first.delta;
            var secondPrevPos = second.screenPosition - second.delta;

            var prevDiff = (firstPrevPos - secondPrevPos).magnitude;
            var diff = (first.screenPosition - second.screenPosition).magnitude;

            Zoom = diff - prevDiff;
            Zoom /= zoomMaxPixel;
            Zoom = Mathf.Clamp(Zoom, -10, 1f);
        }
    }
}
