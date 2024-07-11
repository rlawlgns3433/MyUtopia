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
}

public class StorageTest : MonoBehaviour, IClickable
{
    public List<CurrencyType> currencyTypes;
    public List<TextMeshPro> textMeshPros;
    public BigNumber CurrWorkLoad;
    public BigNumber[] CurrArray;
    private Building[] buildings;
    private bool isClick = false;

    //public ParticleSystem ps;
    public List<ParticleSystem> particleSystems;
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
        CurrArray = new BigNumber[textMeshPros.Count];
        LoadDataOnStart();
        CheckStorage().Forget();
    }

    public async UniTaskVoid CheckStorage()
    {
        await UniTask.WaitUntil(() => buildings.Length > 0 && buildings[0] != null);

        for (int i = 0; i < buildings.Length; i++)
        {
            var workRequire = buildings[i].BuildingData.Work_Require;
            var value = CurrWorkLoad / workRequire;
            value *= offLineSeconds;
            CurrArray[i] += value;
            textMeshPros[i].text = CurrArray[i].ToString();
        }
        await UniTask.Yield();
    }

    private void SaveDataOnQuit()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "storageData.json");
        StorageData storageData = new StorageData
        {
            CurrentWorkLoad = CurrWorkLoad,
            CurrArray = CurrArray
        };
        string json = JsonConvert.SerializeObject(storageData, Formatting.Indented, new WorkLoadConverter());
        File.WriteAllText(filePath, json);
    }

    private void LoadDataOnStart()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "storageData.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            StorageData data = JsonConvert.DeserializeObject<StorageData>(json, new WorkLoadConverter());

            CurrWorkLoad = data.CurrentWorkLoad;
            if(data.CurrArray.Length != 0)
            {
                CurrArray = data.CurrArray;
            }

        }
    }

    public void OpenStorage()
    {
        if (!isClick)
        {
            isClick = true;
            for(int i = 0; i < CurrArray.Length; ++i)
            {
                if (CurrArray[i] > BigNumber.Zero)
                {
                    ParticleSystemEmit(particleSystems[i]).Forget();
                    
                }
            }
        }
        else
        {
            Debug.Log("isClick");
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
            ps.Emit(1);
            await UniTask.WaitUntil(() => !ps.IsAlive(true));

            Debug.Log("Click");
            for (int i = 0; i < textMeshPros.Count; ++i)
            {
                CurrencyManager.currency[(int)currencyTypes[i]] += CurrArray[i];
                CurrArray[i] = BigNumber.Zero;
                textMeshPros[i].text = CurrArray[i].ToString();
                isClick = true;
            }
        }
    }
}
