using Coffee.UIExtensions;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Coffee.UIParticleExtensions;
using UnityEngine.UI;

[Serializable]
public class StorageData
{
    private string currWorkLoad;
    public BigNumber CurrentWorkLoad
    {
        get
        {
            return new BigNumber(currWorkLoad);
        }
        set
        {
            currWorkLoad = value.ToSimpleString();
        }
    }
    private string[] currArray;
    public BigNumber[] CurrArray
    {
        get
        {
            return currArray.Select(s => new BigNumber(s)).ToArray();
        }
        set
        {
            currArray = value.Select(bn => bn.ToSimpleString()).ToArray();
        }
    }
    private int totalOfflineTime;
    public int TotalOfflineTime
    {
        get
        {
            return totalOfflineTime;
        }
        set
        {
            totalOfflineTime = value;
        }
    }
}

public class Storage : MonoBehaviour, IClickable
{
    public List<CurrencyType> currencyTypes;
    public BigNumber CurrWorkLoad;
    private Building[] buildings;
    private bool isClick = false;
    private int currentTotalSeconds;
    public int CurrentTotalSeconds
    {
        get
        {
            return currentTotalSeconds;
        }
        private set
        {
            currentTotalSeconds = value;
        }
    }
    private BigNumber[] currArray;
    public BigNumber[] CurrArray
    {
        get
        {
            return currArray;
        }
        set
        {
            currArray = value;
        }
    }

    public Slider currentValue;
    [SerializeField]
    private int facilityId;

    private FacilityData facilityData;
    public FacilityData FacilityData
    {
        get
        {
            if (facilityData.Furniture_ID == 0)
                facilityData = DataTableMgr.Get<FacilityTable>(DataTableIds.Facility).Get(facilityId);
            return facilityData;
           
        }
        set
        {
            facilityData = value;
        }
    }
    public List<ParticleSystem> particleSystems;
    public Canvas canvas;
    private BigNumber[] values;
    public BigNumber[] Values
    {
        get
        {
            return values;
        }
        set
        {
            values = value;
        }
    }
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
    private int maxSeconds;
    public int MaxSeconds
    {
        get
        {
            return maxSeconds;
        }
        private set
        {
            maxSeconds = value;
        }
    }
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
    private StorageValue storageValue;
    private void Awake()
    {
        clickEvent += OpenStorage;
        RegisterClickable();
        Application.quitting += SaveDataOnQuit;
    }

    private async void Start()
    {
        await UniTask.WaitUntil(() => UtilityTime.Seconds > 0);
        buildings = new Building[currencyTypes.Count];
        CurrArray = new BigNumber[currencyTypes.Count];
        values = new BigNumber[currencyTypes.Count];
        LoadDataOnStart();
        MaxSeconds = FacilityData.Effect_Value;
        Debug.Log($"maxSeconds{maxSeconds}");
        Debug.Log($"UtilityTime{UtilityTime.Seconds}");
        currentTotalSeconds += UtilityTime.Seconds;

        if (maxSeconds > currentTotalSeconds)
        {
            offLineSeconds = UtilityTime.Seconds / 3;
        }
        else
        {
            var overSeconds = currentTotalSeconds - maxSeconds;
            var overTime = UtilityTime.Seconds - overSeconds;
            offLineSeconds = overTime / 3;
            if(offLineSeconds <= 0)
            {
                offLineSeconds = 0;
            }
            currentTotalSeconds = maxSeconds;
        }
        Debug.Log($"offLine = {offLineSeconds},utiliy = {UtilityTime.Seconds},totla = {currentTotalSeconds}");
        CheckStorage().Forget();
        Debug.Log($"Storage Load Test{FacilityData.Furniture_Name}");
        if (currentTotalSeconds > 0)
        {
            currentValue.gameObject.SetActive(true);
            storageValue = currentValue.GetComponent<StorageValue>();
            currentValue.value = Mathf.Clamp01(currentTotalSeconds / maxSeconds);
            await UniTask.WaitUntil(() => currentValue.gameObject.activeSelf);
            storageValue.TotalValue = currentTotalSeconds;
            if(storageValue.TotalValue <= 0 || CurrWorkLoad == 0)
            {
                currentValue.gameObject.SetActive(false);
            }
        }
        Debug.Log("StorageChildTest"+storageValue.TotalValue);
    }

    public async UniTaskVoid CheckStorage()
    {
        await UniTask.WaitUntil(() => buildings.Length > 0 && buildings[0] != null);

        for (int i = 0; i < buildings.Length; i++)
        {
            var workRequire = buildings[i].BuildingData.Work_Require;
            values[i] = CurrWorkLoad / workRequire;
            var tempValue = values[i] * offLineSeconds;
            CurrArray[i] += tempValue;
        }
        await UniTask.Yield();
    }

    private void SaveDataOnQuit()
    {
        if(CurrWorkLoad == 0)
        {
            CurrWorkLoad = BigNumber.Zero;
        }
        if(CurrArray == null)
        {
            CurrArray = new BigNumber[currencyTypes.Count];
        }
        if(UtilityTime.Seconds == 0 || CurrWorkLoad == 0)
        {
            currentTotalSeconds = 0;
        }
        string filePath = Path.Combine(Application.persistentDataPath, $"{FacilityData.Furniture_ID}.json");
        StorageData storageData = new StorageData
        {
            CurrentWorkLoad = CurrWorkLoad,
            CurrArray = CurrArray,
            TotalOfflineTime = currentTotalSeconds,
        };
        string json = JsonConvert.SerializeObject(storageData, Formatting.Indented, new WorkLoadConverter());
        File.WriteAllText(filePath, json);
    }

    private void LoadDataOnStart()
    {
        string filePath = Path.Combine(Application.persistentDataPath, $"{FacilityData.Furniture_ID}.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            StorageData data = JsonConvert.DeserializeObject<StorageData>(json, new WorkLoadConverter());
            if(data.CurrentWorkLoad.IsZero)
            {
                CurrWorkLoad = BigNumber.Zero;
            }
            else
            {
                CurrWorkLoad = data.CurrentWorkLoad;
            }
            if (data.CurrArray.Length != 0)
            {
                CurrArray = data.CurrArray;
            }
            if(data.TotalOfflineTime == 0)
            {
                currentTotalSeconds = 0;
            }
            else
            {
                currentTotalSeconds = data.TotalOfflineTime;
            }
        }
    }

    public void OpenStorage()
    {
        currentValue.gameObject.SetActive(false);
        if (!isClick)
        {
            isClick = true;
            if(isClick)
            {
                currentTotalSeconds = default;
            }
            if(CurrArray != null)
            {
                for (int i = 0; i < CurrArray.Length; ++i)
                {
                    if (CurrArray[i] > BigNumber.Zero)
                    {
                        ParticleSystemEmit(particleSystems[i]).Forget();
                    }
                }
            }
        }
    }

    public void RegisterClickable()
    {
        ClickableManager.AddClickable(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        IsClicked = true;
    }

    public async UniTask ParticleSystemEmit(ParticleSystem ps)
    {
        if (ps != null)
        {
            var worldPosition = transform.position;
            var screenPos = Camera.main.WorldToScreenPoint(worldPosition);
            ps.transform.position = screenPos;

            ps.Emit(1);
            await UniTask.WaitUntil(() => !ps.IsAlive(true));

            Debug.Log("Click");
            for (int i = 0; i < currencyTypes.Count; ++i)
            {
                CurrencyManager.currency[(int)currencyTypes[i]] += CurrArray[i];
                CurrArray[i] = BigNumber.Zero;
                isClick = true;
            }
        }
    }
}
