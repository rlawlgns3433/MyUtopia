using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class StorageTest : MonoBehaviour, IClickable
{
    public List<CurrencyType> currencyTypes;
    public List<TextMeshPro> textMeshPros;
    public BigNumber currBigNum;
    private Building[] buildings;
    public Building[] Buildings
    {
        get
        {
            return buildings;
        }
        set
        {
            buildings = value;
        }
    }
    private int count;
    private int offLineSeconds;

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
        Application.quitting += SaveDataOnQuit;
    }

    private async void Start()
    {
        await UniTask.WaitUntil(() => UtilityTime.Seconds > 0);
        offLineSeconds = UtilityTime.Seconds / 3;
        Debug.Log(offLineSeconds);
        buildings = new Building[textMeshPros.Count];
        LoadDataOnStart();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Test().Forget();
        }
    }

    public async UniTaskVoid Test()
    {
        for (int i = 0; i < buildings.Length; i++)
        {
            if (buildings[i] == null)
            {
                Debug.LogError($"Building at index {i} is null");
                continue;
            }

            if (buildings[i].BuildingData.Name == null)
            {
                Debug.LogError($"BuildingData for building at index {i} is null");
                continue;
            }

            var workRequire = buildings[i].BuildingData.Work_Require;
            if (workRequire == 0)
            {
                Debug.LogError($"Work_Require for building at index {i} is zero");
                continue;
            }

            var value = currBigNum / workRequire;
            Debug.Log($"Building ID: {buildings[i].buildingId}");
            Debug.Log($"Value before multiplication: {value.ToSimpleString()}");

            value *= offLineSeconds;
            Debug.Log($"Value after multiplication: {value.ToSimpleString()}");

            textMeshPros[i].text = value.ToString();
        }
        await UniTask.Yield();
    }


    private void SaveDataOnQuit()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "storageData.json");
        string json = JsonConvert.SerializeObject(this, Formatting.Indented, new WorkLoadConverter());
        File.WriteAllText(filePath, json);
        Debug.Log($"Data saved to {filePath}");
    }

    private void LoadDataOnStart()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "storageData.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            StorageTest data = JsonConvert.DeserializeObject<StorageTest>(json, new WorkLoadConverter());

            currBigNum = data.currBigNum;
            Debug.Log("testst"+currBigNum);
            Debug.Log($"Data loaded from {filePath}");
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
