using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public enum Dirs
{
    None,
    Up, Down, Left, Right
}

public class MultiTouchManager : MonoBehaviour
{
    public bool Tap { get; private set; }
    public Dirs Swipe { get; set; }
    public float Zoom { get; private set; }
    public float DragX { get; private set; }

    private Finger primaryFinger = null;
    public bool isZooming = false;

    private float timeTap = 0.25f;

    private float primaryStartTime = 0f;
    private Vector2 primaryStartPos;
    private Vector2 previousPos;

    private bool isFirstTap = false;

    public float minSwipeDistanceInch = 0.25f;
    private float minSwipeDistancePixels;

    private float swipeTime = 0.25f;

    public float zoomMaxInch = 1f;
    private float zoomMaxPixel;

    private float dragThreshold = 0.2f;
    private bool isDragging = false;

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
                    if (primaryFinger == null)
                    {
                        primaryFinger = touch.finger;
                        primaryStartTime = Time.time;
                        primaryStartPos = touch.screenPosition;
                        previousPos = touch.screenPosition;
                        isDragging = false;
                    }
                    break;
                case TouchPhase.Moved:
                    if (primaryFinger == touch.finger)
                    {
                        float swipeDistance = (touch.screenPosition - primaryStartPos).magnitude;
                        float swipeDuration = Time.time - primaryStartTime;

                        if (swipeDuration < swipeTime && swipeDistance > minSwipeDistancePixels)
                        {
                            Vector2 direction = touch.screenPosition - primaryStartPos;
                            if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
                            {
                                Swipe = direction.y > 0 ? Dirs.Up : Dirs.Down;
                            }
                            else if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                            {
                                Swipe = direction.x > 0 ? Dirs.Right : Dirs.Left;
                            }
                        }
                        else if (!isDragging && swipeDuration > timeTap && swipeDistance > dragThreshold)
                        {
                            isDragging = true;
                        }

                        if (isDragging)
                        {
                            DragX = touch.screenPosition.x - previousPos.x;
                        }

                        previousPos = touch.screenPosition;
                    }
                    break;
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (primaryFinger == touch.finger)
                    {
                        primaryFinger = null;
                        var duration = Time.time - primaryStartTime;
                        if (duration < timeTap && !isDragging)
                        {
                            Tap = true;
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
