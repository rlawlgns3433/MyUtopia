using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class WorldMapMoveTest : MonoBehaviour
{
    public GameObject titlePanel;
    public GameObject infoPanel;
    public float speed = 5f;
    public float offsetY = 1.5f;
    private Vector2 mouseDelta;
    private bool isDragging = false;
    private InputAction dragAction;
    private InputAction moveAction;
    private InputAction touchAction;
    private WorldMapMove worldMapMove;
    public Transform rayOrigin;
    private Vector3 defaultCameraPosition;
    public TextMeshProUGUI worldName;
    private bool isRotate = false;
    private void Awake()
    {
        worldMapMove = new WorldMapMove();
        dragAction = worldMapMove.WorldMap.Drag;
        moveAction = worldMapMove.WorldMap.Move;
        touchAction = worldMapMove.WorldMap.Touch;
        if(!titlePanel.gameObject.activeSelf)
            titlePanel.gameObject.SetActive(true);
        if(infoPanel.gameObject.activeSelf)
            infoPanel.gameObject.SetActive(false);
        defaultCameraPosition = Camera.main.transform.position;
    }

    private void OnEnable()
    {
        dragAction.started += OnDragStarted;
        dragAction.canceled += OnDragCanceled;
        moveAction.performed += OnMouseMove;
        touchAction.started += OnTouchTitlePanel;

        dragAction.Enable();
        moveAction.Enable();
        touchAction.Enable();
    }

    private void OnDisable()
    {
        dragAction.started -= OnDragStarted;
        dragAction.canceled -= OnDragCanceled;
        moveAction.performed -= OnMouseMove;
        touchAction.started -= OnTouchTitlePanel;

        dragAction.Disable();
        moveAction.Disable();
        touchAction.Disable();
    }

    private void OnTouchTitlePanel(InputAction.CallbackContext context)
    {
        if (titlePanel.gameObject.activeSelf)
        {
            titlePanel.gameObject.SetActive(false);
            infoPanel.gameObject.SetActive(true);
            var camera = Camera.main;
            defaultCameraPosition.y = offsetY;
            camera.transform.position = defaultCameraPosition;
            //SetWorldInfo();
        }
    }

    private void SetWorldInfo()
    {
        //worldName.text = DataTableMgr.GetWorldTable().Get(int.Parse("101")).GetWorldName();
        //그외 정보들 출력
    }

    private void OnDragStarted(InputAction.CallbackContext context)
    {
        if(!titlePanel.gameObject.activeSelf || isRotate)
            isDragging = true;
    }

    private void OnDragCanceled(InputAction.CallbackContext context)
    {
        if (!titlePanel.gameObject.activeSelf)
        {
            isDragging = false;
            MoveToCurrentRotation();
        }
    }

    private void OnMouseMove(InputAction.CallbackContext context)
    {
        if (isDragging)
        {
            mouseDelta = context.ReadValue<Vector2>();
            float angleX = mouseDelta.y * speed * Time.deltaTime;
            float angleY = -mouseDelta.x * speed * Time.deltaTime;
            transform.Rotate(Vector3.up, angleY, Space.Self);
            transform.Rotate(Vector3.right, angleX, Space.Self);
        }
    }

    private void MoveToCurrentRotation()
    {
        isRotate = true;
        Vector3 currentRotation = transform.rotation.eulerAngles;
        float closestX = Mathf.Round(currentRotation.x / 90) * 90;
        float closestY = Mathf.Round(currentRotation.y / 90) * 90;
        float closestZ = Mathf.Round(currentRotation.z / 90) * 90;
        Quaternion targetRotation = Quaternion.Euler(closestX, closestY, closestZ);
        RotateToTargetRotation(targetRotation);
    }

    private void RotateToTargetRotation(Quaternion targetRotation)
    {
        transform.DORotateQuaternion(targetRotation, 1f).OnComplete(() =>
        {
            PerformRaycast();
            isRotate = false;
        });
    }

    private void PerformRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin.position, -Vector3.up, out hit))
        {
            if (hit.collider.CompareTag("Worlds"))
            {
                Debug.Log("Worlds Name: " + hit.collider.gameObject.name);
            }
            else
            {
                Debug.Log("Hit Fail");
            }
        }
        else
        {
            Debug.Log("No object hit.");
        }
    }
    public void OpenWorld()
    {

        SceneManager.LoadScene("SampleScene");
        
    }
}
