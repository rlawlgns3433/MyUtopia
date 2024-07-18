using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimalClick : MonoBehaviour, IClickable
{
    [SerializeField]
    private FloorMove floorMoveGo;
    public FloorMove FloorMoveGo
    {
        get
        {
            if (floorMoveGo == null)
            {
                floorMoveGo = GameObject.Find("Floors").GetComponent<FloorMove>();
            }

            return floorMoveGo;
        }
    }
    [SerializeField]
    private AnimalWork animalWork;
    public AnimalWork AnimalWork
    {
        get
        {
            if(animalWork == null)
            {
                animalWork = GetComponent<AnimalWork>();
            }

            return animalWork;
        }
    }

    [SerializeField]
    private bool isClicked = false;
    public bool IsClicked
    {
        get
        {
            return isClicked;
        }

        set
        {
            isClicked = value;
            if (isClicked)
            {
                ClickableManager.OnClicked(this);
                clickEvent?.Invoke();
            }
        }
    }

    [SerializeField]
    private float duration = 0f;
    [SerializeField]
    private Vector3 initialScale;
    [SerializeField]
    private Vector3 clickedScale;
    [SerializeField]
    private Vector3 followOffset;

    public event Action clickEvent;

    private CinemachineTransposer transposer;
    private CinemachineTransposer Transposer
    {
        get
        {
            if (transposer == null)
            {
                transposer = VirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            }

            return transposer;
        }
        set
        {
            transposer = value;
        }
    }

    private CinemachineVirtualCamera virtualCamera;

    public CinemachineVirtualCamera VirtualCamera
    {
        get
        {
            if(virtualCamera == null)
            {
                virtualCamera = GameObject.FindWithTag(Tags.VirtualCamera).GetComponent<CinemachineVirtualCamera>();
            }

            return virtualCamera;
        }
        set
        {
            virtualCamera = value;
        }
    }

    private void Awake()
    {
        VirtualCamera = GameObject.FindWithTag(Tags.VirtualCamera).GetComponent<CinemachineVirtualCamera>();
        Transposer = VirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        floorMoveGo = GameObject.Find("Floors").GetComponent<FloorMove>();
        clickEvent += Bump;
        clickEvent += UiManager.Instance.ShowAnimalFocusUi;
        clickEvent += UiManager.Instance.animalFocusUi.Set;
        clickEvent += Follow;

        RegisterClickable();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        IsClicked = true;
    }

    public void Follow()
    {
        var animalClick = ClickableManager.CurrentClicked as AnimalClick;

        if (animalClick == null)
            return;
        Debug.Log($"moveTest{animalClick.AnimalWork.Animal.animalStat.CurrentFloor}");
        VirtualCamera.Follow = transform;
        VirtualCamera.LookAt = transform;
        Transposer.m_FollowOffset = followOffset;
        FloorManager.Instance.SetFloor(animalClick.AnimalWork.Animal.animalStat.CurrentFloor);
        floorMoveGo.MoveToCurrentFloor();
    }

    public void FocusIn()
    {
        if (transposer != null)
        {
            transposer.m_FollowOffset = new Vector3(0, 2, -2);
        }
    }

    private void Bump()
    {
        if (IsClicked)
            return;
        transform.DOScale(clickedScale, duration).OnComplete(() =>
        {
            transform.DOScale(initialScale, duration);
        });
    }

    public void RegisterClickable()
    {
        ClickableManager.AddClickable(this);
    }

    public void MoveAnimal(string toFloor)
    {
        var animalClick = ClickableManager.CurrentClicked as AnimalClick;

        if (animalClick == null)
            return;
        Debug.Log("Move"+animalWork.Animal.animalStat.CurrentFloor);
        FloorManager.Instance.MoveAnimal(animalClick.AnimalWork.Animal.animalStat.CurrentFloor, toFloor, animalWork.Animal);
        gameObject.SetActive(false);
        gameObject.transform.position = FloorManager.Instance.GetFloor(toFloor).transform.position;
        gameObject.SetActive(true); 
    }
}
