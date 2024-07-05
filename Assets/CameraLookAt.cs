using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    private Camera mainCamera;
    private void Awake()
    {
        mainCamera = Camera.main;
    }
    void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(-mainCamera.transform.forward);
    }
}
