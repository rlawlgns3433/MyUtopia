using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class StorageUi : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform slotParent;
    private Building[] buildings;
    private int b5Count = 0;
    private int b4Count = 0;
    private Floor b5Floor;
    private Floor b4Floor;
    public StorageConduct b5FloorStorage;
    public StorageConduct b4FloorStorage;

    public Button openButton;
    private BigNumber[] b5currencyArray;
    private BigNumber[] b4currencyArray;

    private async void Start()
    {
        await UniTask.WaitUntil(() => GameManager.Instance.isLoadedWorld);
        openButton.interactable = false;
        await UniTask.WaitUntil(() => FloorManager.Instance.GetFloor("B5") != null);
        await UniTask.WaitUntil(() => FloorManager.Instance.GetFloor("B4") != null);
        b5Floor = FloorManager.Instance.GetFloor("B5");
        b4Floor = FloorManager.Instance.GetFloor("B4");
        await UniTask.WaitUntil(() => (b5Floor as BuildingFloor).storageConduct != null);
        await UniTask.WaitUntil(() => (b4Floor as BuildingFloor).storageConduct != null);
        b5FloorStorage = (b5Floor as BuildingFloor).storageConduct;
        b4FloorStorage = (b4Floor as BuildingFloor).storageConduct;
        await WaitLoadCompleteStorage(b5FloorStorage, b4FloorStorage);
        buildings = new Building[b5Floor.buildings.Count + b4Floor.buildings.Count];
        foreach(var building in b5Floor.buildings)
        {
            buildings[b5Count] = building;
            b5Count++;

        }
        foreach (var building in b4Floor.buildings)
        {
            buildings[b5Count + b4Count] = building;
            b4Count++;
        }
        b5currencyArray = new BigNumber[b5FloorStorage.CurrArray.Length];
        b4currencyArray = new BigNumber[b4FloorStorage.CurrArray.Length];
        b5currencyArray = b5FloorStorage.CurrArray;
        b4currencyArray = b4FloorStorage.CurrArray;
        for (int i = 0; i < b5currencyArray.Length; i++)
        {
            if (!b5currencyArray[i].IsZero)
            {
                var slot = Instantiate(slotPrefab, slotParent);
                var slotUi = slot.GetComponent<StorageSlotUi>();
                slotUi.SetText($"{b5currencyArray[i].ToString()}");
                slotUi.SetSprite(b5FloorStorage.currencyTypes[i]).Forget();
            }
        }
        for (int i = 0; i < b4currencyArray.Length; i++)
        {
            if (!b4currencyArray[i].IsZero)
            {
                var slot = Instantiate(slotPrefab, slotParent);
                var slotUi = slot.GetComponent<StorageSlotUi>();
                slotUi.SetText($"{b4currencyArray[i].ToString()}");
                slotUi.SetSprite(b4FloorStorage.currencyTypes[i]).Forget();
            }
        }

        openButton.interactable = true;
        await UniTask.Delay(1000);
        UiManager.Instance.SetEvent();
    }

    private async UniTask WaitLoadCompleteStorage(StorageConduct b5Storage, StorageConduct b4Storage)
    {
        b5Storage.CheckStorage().Forget();
        b4Storage.CheckStorage().Forget();
        await UniTask.WaitUntil(() => b5Storage.isLoadComplete);
        await UniTask.WaitUntil(() => b4Storage.isLoadComplete);
    }

    public void OnClickAddCurrency()
    {
        b5FloorStorage.OpenStorage(openButton.transform.position);
        b4FloorStorage.OpenStorage(openButton.transform.position);
        SoundManager.Instance.OnClickButton(SoundType.GetAnimal);
        UiManager.Instance.ShowMainUi();
    }

    public void SaveStorageData()
    {
        b5FloorStorage.SaveDataOnQuit();
        b4FloorStorage.SaveDataOnQuit();
    }
}
