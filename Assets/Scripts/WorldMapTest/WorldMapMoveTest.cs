using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CubeRotator : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 mouseDelta;
    private bool isDragging = false;
    private InputAction dragAction;
    private InputAction moveAction;
    private WorldMapMove playerInput;
    public Transform rayOrigin;

    private void Awake()
    {
        playerInput = new WorldMapMove();
        dragAction = playerInput.WorldMap.Drag;
        moveAction = playerInput.WorldMap.Move;
    }

    private void OnEnable()
    {
        dragAction.started += OnDragStarted;
        dragAction.canceled += OnDragCanceled;
        moveAction.performed += OnMouseMove;

        dragAction.Enable();
        moveAction.Enable();
    }

    private void OnDisable()
    {
        dragAction.started -= OnDragStarted;
        dragAction.canceled -= OnDragCanceled;
        moveAction.performed -= OnMouseMove;

        dragAction.Disable();
        moveAction.Disable();
    }

    private void OnDragStarted(InputAction.CallbackContext context)
    {
        isDragging = true;
    }

    private void OnDragCanceled(InputAction.CallbackContext context)
    {
        isDragging = false;
        MoveToCurrentRotation();
    }

    private void OnMouseMove(InputAction.CallbackContext context)
    {
        if (isDragging)
        {
            mouseDelta = context.ReadValue<Vector2>();
            float angleX = mouseDelta.y * speed * Time.deltaTime;
            float angleY = -mouseDelta.x * speed * Time.deltaTime;
            transform.Rotate(Vector3.up, angleY, Space.World);
            transform.Rotate(Vector3.right, angleX, Space.World);
        }
    }

    private void MoveToCurrentRotation()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        float closestX = Mathf.Round(currentRotation.x / 90) * 90;
        float closestY = Mathf.Round(currentRotation.y / 90) * 90;
        float closestZ = Mathf.Round(currentRotation.z / 90) * 90;
        Quaternion targetRotation = Quaternion.Euler(closestX, closestY, closestZ);
        StartCoroutine(RotateToTargetRotation(targetRotation));
    }

    private IEnumerator RotateToTargetRotation(Quaternion targetRotation)
    {
        float time = 0f;
        Quaternion startRotation = transform.rotation;
        while (time < 1f)
        {
            time += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, time);
            yield return null;
        }
        transform.rotation = targetRotation;
    }
}
