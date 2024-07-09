using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class StorageTest : MonoBehaviour, IClickable
{
    public TextMeshPro first;
    public TextMeshPro second;
    public TextMeshPro third;
    public TextMeshPro fourth;

    private int offLineSeconds;
    private UtilityTime utilityTime;
    public int offsetFirst = 1; // ������ ���뿹��
    public int offsetSecond = 2;
    public int offsetThird = 3;

    public event Action clickEvent;
    [SerializeField]
    private bool isClicked;
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
    private void Awake()
    {
        clickEvent += OpenStorage;
        RegisterClickable();
    }
    private void Start()
    {
        utilityTime = FindObjectOfType<UtilityTime>();        //���� �±׷� ���� or �ν����� �Ҵ�
        offLineSeconds = utilityTime.Seconds;
        Debug.Log(offLineSeconds);
        BigNum bigNumFirst = new BigNum((offLineSeconds * offsetFirst).ToString());
        BigNum bigNumSecond = new BigNum((offLineSeconds * offsetSecond).ToString());
        BigNum bigNumThird = new BigNum((offLineSeconds * offsetThird).ToString());
        first.text = bigNumFirst.ToString();
        second.text = bigNumSecond.ToString();
        third.text = bigNumThird.ToSimpleString();
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
           
        }
    }
    public void OpenStorage()
    {
        Debug.Log("Click");
    }
    public void RegisterClickable()
    {
        ClickableManager.AddClickable(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        IsClicked = true;
    }
}
