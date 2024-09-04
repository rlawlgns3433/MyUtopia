using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
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
    public WorldMapTutorial worldMapTutorial;
    private Quaternion rotation;
    private bool checkTutorial = false;
    private Vector2 initialMousePosition;
    private const float DragThreshold = 5f;
    private bool loadScene = false;
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
        //else
        //{
        //    WorldMapSoundManager.Instance.SetVolume();
        //}
        loadScene = false;
        await UniTask.WaitUntil(() => LoadingManager.Instance != null);
        await LoadingManager.Instance.FadeOut(1);
        LoadingManager.Instance.HideLoadingPanel();
        rotation = transform.rotation;
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
        if (LoadingManager.Instance.loadingPanel.gameObject.activeSelf)
            return;
        if (!checkTutorial)
        {
            worldMapTutorial.gameObject.SetActive(true);
            worldMapTutorial.CheckTutorial();
            checkTutorial = true;
        }
        if(worldMapTutorial.gameObject.activeSelf)
        {
            if (worldMapTutorial.progress == WorldMapTutorialProgress.None)
                worldMapTutorial.SetTutorialProgress();
        }
        if (titlePanel.gameObject.activeSelf)
        {
            titlePanel.gameObject.SetActive(false);
            infoPanel.gameObject.SetActive(true);
            var camera = Camera.main;
            defaultCameraPosition.y = offsetY;
            camera.transform.position = defaultCameraPosition;
        }
    }

    private void OnDragStarted(InputAction.CallbackContext context)
    {
        if (!titlePanel.gameObject.activeSelf && !worldMapTutorial.stopDrag)
        {
            if (context.control.device is Mouse)
            {
                initialMousePosition = Mouse.current.position.ReadValue();
            }
            else if (context.control.device is Touchscreen)
            {
                initialMousePosition = Touchscreen.current.primaryTouch.position.ReadValue();
            }
            isDragging = false;
        }
    }

    private void OnMouseMove(InputAction.CallbackContext context)
    {
        if (worldMapTutorial != null && worldMapTutorial.gameObject.activeSelf)
        {
            if (worldMapTutorial.stopDrag)
                return;
        }

        if (optionPanel.gameObject.activeSelf || LoadingManager.Instance.loadingPanel.gameObject.activeSelf)
            return;

        if (isRotate)
        {
            DOTween.Kill(transform);
            isRotate = false;
        }

        if (!isDragging)
        {
            Vector2 currentMousePosition = Vector2.zero;
            if (context.control.device is Mouse)
            {
                currentMousePosition = Mouse.current.position.ReadValue();
            }
            else if (context.control.device is Touchscreen)
            {
                currentMousePosition = Touchscreen.current.primaryTouch.position.ReadValue();
            }

            float distance = Vector2.Distance(initialMousePosition, currentMousePosition);

            if (distance > DragThreshold)
            {
                isDragging = true;
            }
        }

        if (isDragging)
        {
            if (context.control.device is Mouse)
            {
                mouseDelta = Mouse.current.delta.ReadValue();
            }
            else if (context.control.device is Touchscreen)
            {
                mouseDelta = context.ReadValue<Vector2>();
            }

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

    private void OnDragCanceled(InputAction.CallbackContext context)
    {
        if (optionPanel.gameObject.activeSelf)
            return;
        if (isDragging && !titlePanel.gameObject.activeSelf && !worldMapTutorial.stopDrag)
        {
            isDragging = false;
            MoveToCurrentRotation();
        }
    }

    public void SetTutorialRotation()
    {
        isRotate = true;
        RotateToTargetRotation(rotation);
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
            if (worldMapTutorial != null && worldMapTutorial.gameObject.activeSelf)
            {
                if (worldMapTutorial.progress == WorldMapTutorialProgress.Drag)
                {
                    worldMapTutorial.SetTutorialProgress();
                }
            }
        });
    }

    private void PerformRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin.position, -Vector3.up, out hit))
        {
            if (hit.collider.CompareTag("Worlds"))
            {
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
        }
    }
    public async void OpenWorld()
    {
        if (loadScene)
            return;
        loadScene = true;
        if (worldMapTutorial != null && worldMapTutorial.gameObject.activeSelf)
        {
            if (worldMapTutorial.progress == WorldMapTutorialProgress.SelectWorld)
            {
                worldMapTutorial.SetTutorialProgress();
            }
        }
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
        WorldMapSoundManager.Instance.SaveVolume();
        optionPanel.gameObject.SetActive(false);
    }

    public void OnClickQuitGame()
    {
        WorldMapSoundManager.Instance.SaveVolume();
        Application.Quit();
    }

    public void TestTutorial()
    {
        PlayerPrefs.SetInt("WorldTutorialCheck", 0);
        PlayerPrefs.SetInt("TutorialCheck", 0);
        Application.Quit();
    }
}
