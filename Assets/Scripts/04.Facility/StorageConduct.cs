using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class StorageConduct : MonoBehaviour
{
    public List<CurrencyProductType> currencyTypes;
    public BigNumber CurrWorkLoad;
    public int floorIndex;
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
    //private int maxSeconds;
    //public int MaxSeconds
    //{
    //    get
    //    {
    //        return maxSeconds;
    //    }
    //    private set
    //    {
    //        maxSeconds = value;
    //    }
    //}
    private int offLineSeconds;

    private bool isAddQuitEvent = false;
    private BigNumber workLoadValue;
    private Floor floor;
    private float offLineWorkLoad;
    public bool isLoadComplete = false;
    private StorageValue storageValue;
    private int count = 0;
    private Sprite[] sprites;
    public float OffLineWorkLoad
    {
        get
        {
            return offLineWorkLoad;
        }
        set
        {
            offLineWorkLoad = value;
        }
    }
    private void Awake()
    {
        if (!isAddQuitEvent)
        {
            Application.quitting -= SaveDataOnQuit;
            Application.quitting += SaveDataOnQuit;
            isAddQuitEvent = true;
        }
    }
    public void OnDisable()
    {
        SaveData();
    }

    public async UniTask CheckStorage()
    {
        await UniWaitFurnitureTable();
        await UniTask.WaitUntil(() => UtilityTime.Seconds > 0);
        //MaxSeconds = FurnitureStat.Effect_Value;
        CurrArray = new BigNumber[currencyTypes.Count];
        values = new BigNumber[currencyTypes.Count];
        LoadDataOnStart();
        //if (MaxSeconds == 0)
        //{
        //    return;
        //}
        currentTotalSeconds += UtilityTime.Seconds;
        //if (maxSeconds > currentTotalSeconds)
        //{
        //    offLineSeconds = UtilityTime.Seconds / 3;
        //}
        //else
        //{
        //    var overSeconds = currentTotalSeconds - maxSeconds;
        //    var overTime = UtilityTime.Seconds - overSeconds;
        //    offLineSeconds = overTime / 3;
        //    if (offLineSeconds <= 0)
        //    {
        //        offLineSeconds = 0;
        //    }
        //    currentTotalSeconds = maxSeconds;
        //}
        offLineSeconds = UtilityTime.Seconds / 3;
        floor = FloorManager.Instance.GetFloor($"B{floorIndex}");
        await UniTask.WaitUntil(() => floor.buildings.Count > 0 && floor.buildings[0] != null);
        //bool isEmpty = true;
        var offLineTime = (int)OffLineWorkLoad / 3;//int
        if(offLineTime >= offLineSeconds)
        {
            offLineTime = offLineSeconds;
        }
        for (int i = 0; i < floor.buildings.Count; i++)
        {
            if (!floor.buildings[i].BuildingStat.IsLock)
            {
                var workRequire = floor.buildings[i].BuildingStat.Work_Require;
                values[i] = workLoadValue / workRequire;
                var tempValue = values[i] * offLineSeconds;
                var tempOffLineValue = tempValue;
                if (offLineTime >= 0)
                {
                    tempOffLineValue = values[i] * offLineTime;
                }
                if (tempOffLineValue == 0)
                {
                    CurrArray[i] += tempValue;
                }
                else
                {
                    if(tempValue - tempOffLineValue >= 0)
                    {
                        CurrArray[i] += tempValue - tempOffLineValue;
                    }
                }
                //if (CurrArray[i] != 0)
                //{
                //    isEmpty = false;
                //}
            }
            else
            {
                CurrArray[i] += BigNumber.Zero;
            }
        }
        //currentValue.gameObject.SetActive(true);
        //if (currentTotalSeconds > 0)
        //{
        //    if (currentValue == null)
        //    {
        //        return;
        //    }

        //    storageValue = currentValue.GetComponent<StorageValue>();
        //    if (storageValue == null)
        //    {
        //        return;
        //    }
        //    currentValue.value = Mathf.Clamp01((float)currentTotalSeconds / maxSeconds);
        //    await UniTask.WaitUntil(() => currentValue.gameObject.activeSelf);
        //    storageValue.TotalValue = currentTotalSeconds;
        //    if (storageValue.TotalValue <= 0 || workLoadValue == 0)
        //    {
        //        currentValue.gameObject.SetActive(false);
        //    }
        //}
        //if (isEmpty)
        //{
        //    currentValue.gameObject.SetActive(false);
        //    //활동량 못채울시 시간초기화 or 누적
        //}
        //sprites = new Sprite[particleSystems.Count];
        isLoadComplete = true;
        await UniTask.Yield();
    }

    public void SaveDataOnQuit()
    {
        SaveData();
    }

    private void LoadDataOnStart()
    {
        string filePath = Path.Combine(Application.persistentDataPath, $"B{floorIndex}.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            StorageData data = JsonConvert.DeserializeObject<StorageData>(json, new WorkLoadConverter());
            if (data.CurrentWorkLoad.IsZero)
            {
                workLoadValue = BigNumber.Zero;
            }
            else
            {
                workLoadValue = data.CurrentWorkLoad;
            }
            if (data.CurrArray.Length != 0)
            {
                CurrArray = data.CurrArray;
            }
            if (data.TotalOfflineTime == 0)
            {
                currentTotalSeconds = 0;
            }
            else
            {
                currentTotalSeconds = data.TotalOfflineTime;
            }
        }
        else
        {
            Debug.Log("No existing data file found.");
        }
    }

    public void OpenStorage(Vector2 pos)
    {
        currentValue.gameObject.SetActive(false);
        if (!isClick)
        {
            isClick = true;
            if (CurrArray != null)
            {
                for (int i = 0; i < CurrArray.Length; ++i)
                {
                    if (CurrArray[i] > BigNumber.Zero)
                    {
                        Debug.Log($"Emitting particle for currency index {i}, value {CurrArray[i]}");
                        ParticleSystemEmit(particleSystems[i], i, pos).Forget();
                        Debug.Log($"{particleSystems[i].name}/{i}");
                    }
                }
            }
        }
    }

    //private async UniTask SetParticleImage()
    //{
    //    for(int i = 0; i < particleSystems.Count; ++i)
    //    {
    //        sprites[i] = await DataTableMgr.GetResourceTable().Get((int)currencyTypes[i]).GetImage();
    //        Debug.Log($"{particleSystems[i].name}/{sprites[i].name}");
    //    }

    //}

    public async UniTask ParticleSystemEmit(ParticleSystem ps, int index , Vector2 pos)
    {
        if (ps != null)
        {
            //var worldPosition = transform.position;
            //var screenPos = Camera.main.WorldToScreenPoint(worldPosition);
            ps.transform.position = pos;
            //var psSetTexture = particleSystems[index].textureSheetAnimation;
            //if (psSetTexture.mode == ParticleSystemAnimationMode.Sprites && sprites[index] != null)
            //{
            //    psSetTexture.SetSprite(0, sprites[index]);
            //    Debug.Log($"{particleSystems[index].name}/{sprites[index].name}");
            //}
            ps.Emit(1);
            await UniTask.WaitUntil(() => !ps.IsAlive(true));
            if (isClick)
            {
                currentTotalSeconds = default;
            }
            Debug.Log("Click");
            for (int i = 0; i < currencyTypes.Count; ++i)
            {
                CurrencyManager.product[currencyTypes[i]] += CurrArray[i];
                CurrArray[i] = BigNumber.Zero;

                isClick = true;
            }
        }
    }

    public void SaveData()
    {
        if(floor != null)
        {
            CurrWorkLoad = floor.autoWorkload;
        }

        Debug.Log("Saving data...");
        if (CurrWorkLoad == 0 || CurrWorkLoad == default)
        {
            CurrWorkLoad = new BigNumber(0);
        }
        if (CurrArray == null)
        {
            CurrArray = new BigNumber[currencyTypes.Count];
        }
        if (UtilityTime.Seconds == 0 || CurrWorkLoad == 0)
        {
            currentTotalSeconds = 0;
        }
        string filePath = Path.Combine(Application.persistentDataPath, $"B{floorIndex}.json");
        StorageData storageData = new StorageData
        {
            CurrentWorkLoad = CurrWorkLoad,
            CurrArray = CurrArray,
            TotalOfflineTime = currentTotalSeconds,
        };
        string json = JsonConvert.SerializeObject(storageData, Formatting.Indented, new WorkLoadConverter());
        Debug.Log("Serialized JSON: " + json);
        File.WriteAllText(filePath, json);
        Debug.Log("Data saved: " + json);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveData();
        }
    }
    public async UniTask UniWaitFurnitureTable()
    {
        while (!DataTableMgr.GetBuildingTable().IsLoaded)
        {
            await UniTask.Yield();
        }
        return;
    }

    public void ResetStorageConduct()
    {
        if(CurrArray != null)
        {
            for (int i = 0; i < CurrArray.Length; i++)
            {
                CurrArray[i] = BigNumber.Zero;
            }
            CurrentTotalSeconds = 0;
            currentValue.gameObject.SetActive(false);
            CurrWorkLoad = BigNumber.Zero;
        }
    }
}
