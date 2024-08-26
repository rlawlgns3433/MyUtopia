using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldMapManager : MonoBehaviour
{
    public GameObject titlePanel;
    public GameObject infoPanel;
    public float speed = 2f;
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
    public Button moveWorldButton;
    public GameObject loadingManager;
    public GameObject optionPanel;

    private async void Awake()
    {
        Application.targetFrameRate = 60;
        worldMapMove = new WorldMapMove();
        dragAction = worldMapMove.WorldMap.Drag;
        moveAction = worldMapMove.WorldMap.Move;
        touchAction = worldMapMove.WorldMap.Touch;
        if(!titlePanel.gameObject.activeSelf)
            titlePanel.gameObject.SetActive(true);
        if(infoPanel.gameObject.activeSelf)
            infoPanel.gameObject.SetActive(false);
        defaultCameraPosition = Camera.main.transform.position;
        if (LoadingManager.Instance == null)
        {
            GameObject loadingManagerInstance = Instantiate(loadingManager);
            DontDestroyOnLoad(loadingManagerInstance);
        }
        else
        {
            WorldMapSoundManager.Instance.SetVolume();
        }
        
        await LoadingManager.Instance.FadeOut(1);
        LoadingManager.Instance.HideLoadingPanel();
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

    //private void SetWorldInfo()
    //{
    //    //worldName.text = DataTableMgr.GetWorldTable().Get(int.Parse("101")).GetWorldName();
    //    //±×¿Ü Á¤º¸µé Ãâ·Â
    //}

    private void OnDragStarted(InputAction.CallbackContext context)
    {
        if(!titlePanel.gameObject.activeSelf)
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
            if (mouseDelta.sqrMagnitude < 0.1f)
                return;

            bool rotateAroundY = Mathf.Abs(mouseDelta.x) > Mathf.Abs(mouseDelta.y);
            if (rotateAroundY)
            {
                float angleY = -mouseDelta.x * speed;
                transform.Rotate(Vector3.up, angleY, Space.World);
            }
            else
            {
                float angleX = mouseDelta.y * speed;
                transform.Rotate(Vector3.right, angleX, Space.World);
            }
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
                if(hit.collider.gameObject.name == "1_Land of Hope")
                {
                    worldName.text = "Èñ¸ÁÀÇ ¶¥";
                    moveWorldButton.interactable = true;
                }
                else
                {
                    worldName.text = "Coming Soon...";
                    moveWorldButton.interactable = false;
                }
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
    public async void OpenWorld()
    {
        await UtilityTime.Instance.CalculateElapsedTime();
        LoadingManager.Instance.ShowLoadingPanel();
        WorldMapSoundManager.Instance.SaveVolume();
        await LoadingManager.Instance.FadeIn(1);
        await SceneManager.LoadSceneAsync("SampleScene CBTJH");
        await WaitForDataLoadComplete();
    }

    private async UniTask WaitForDataLoadComplete()
    {
        var uiManager = UiManager.Instance;
        while (uiManager == null)
        {
            uiManager = UiManager.Instance;
            await UniTask.Yield();
        }
        var tcs = new UniTaskCompletionSource();
        uiManager.OnDataLoadComplete += () =>
        {
            tcs.TrySetResult();
        };
        await tcs.Task;
    }

    public void OnClickOptionUi()
    {
        optionPanel.gameObject.SetActive(true);
        var uiSetting = optionPanel.GetComponent<WorldMapUiSetting>();
        if(uiSetting != null)
        {
            uiSetting.SetSlider();
        }
    }

    public void OnClickQuitOptionUi()
    {
        optionPanel.gameObject.SetActive(false);
    }

    public void OnClickQuitGame()
    {
        Application.Quit();
    }
}
