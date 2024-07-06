using Cinemachine;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimalClick : MonoBehaviour, IClickable
{
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
                clickEvent?.Invoke();
                ClickableManager.OnClicked(this);
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
    }

    private void Awake()
    {
        clickEvent += Bump;
        clickEvent += Follow;
        RegisterClickable();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        IsClicked = true;
    }

    public void Follow()
    {
        VirtualCamera.Follow = transform;
        VirtualCamera.LookAt = transform;
    }

    public void FocusIn()
    {
        var transposer = VirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        if (transposer != null)
        {
            transposer.m_FollowOffset = new Vector3(0, 2, -2);
        }
    }

    private void Bump()
    {
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
        FloorManager.MoveAnimal(animalWork.currentFloor, toFloor, animalWork.myAnimalData);
        gameObject.SetActive(false);
        gameObject.transform.position = FloorManager.GetFloor(toFloor).transform.position;
        gameObject.SetActive(true); 
    }
}
