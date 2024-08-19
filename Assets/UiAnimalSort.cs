using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum AnimalSortType
{
    Default,
    Acquire,
    Workload,
    Type
}

public class UiAnimalSort : MonoBehaviour
{
    private readonly string[] sortingOptions =
    {
        "�⺻",
        "ȹ���",
        "���������",
        "Ÿ�Ժ�"
    };

    public UiAnimalList uiAnimalList;
    public TMP_Dropdown sorting;
    public AnimalSortType sortType = AnimalSortType.Acquire;

    public void Awake()
    {
        uiAnimalList = GetComponent<UiAnimalList>();

        sorting.options.Clear();
        foreach (var option in sortingOptions)
        {
            sorting.options.Add(new TMP_Dropdown.OptionData(option));
        }
        sorting.value = 0;
        sorting.RefreshShownValue();
    }

    public void OnValueChangeSorting(int value)
    {
        uiAnimalList.Clear();
        sortType = (AnimalSortType)value;
        uiAnimalList.SortAnimal(sortType);
    }
}
