using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StorageTest : MonoBehaviour, IClickable
{
    public TextMeshPro first;
    public TextMeshPro second;
    public TextMeshPro third;

    private int offLineSeconds;
    private UtilityTime utilityTime;
    public int offsetFirst = 1; // ������ ���뿹��
    public int offsetSecond = 2;
    public int offsetThird = 3;

    public event Action clickEvent;
    private bool isClicked;
    public bool IsClicked { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    private void Start()
    {
        utilityTime = FindObjectOfType<UtilityTime>();        //���� �±׷� ���� or �ν����� �Ҵ�
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            offLineSeconds = utilityTime.Seconds;
            Debug.Log(offLineSeconds);
            BigNum bigNumFirst = new BigNum((offLineSeconds * offsetFirst).ToString());
            BigNum bigNumSecond = new BigNum((offLineSeconds * offsetSecond).ToString());
            BigNum bigNumThird = new BigNum((offLineSeconds * offsetThird).ToString());
            first.text = bigNumFirst.ToString();
            second.text = bigNumSecond.ToString();
            third.text = bigNumThird.ToSimpleString();
        }
    }

    public void RegisterClickable()
    {
        throw new NotImplementedException();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }
}
