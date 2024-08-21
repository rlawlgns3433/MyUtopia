using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestPanel : MonoBehaviour
{
    public void ShowMeTheMoney()
    {
        CurrencyManager.currency[CurrencyType.Coin] += 10000;
        CurrencyManager.product[CurrencyProductType.CopperStone] += 10000;
        CurrencyManager.product[CurrencyProductType.SilverStone] += 10000;
        CurrencyManager.product[CurrencyProductType.GoldStone] += 10000;
        CurrencyManager.product[CurrencyProductType.CopperIngot] += 10000;
        CurrencyManager.product[CurrencyProductType.SilverIngot] += 10000;
        CurrencyManager.product[CurrencyProductType.GoldIngot] += 10000;
    }

    // ���� ���̺� ���� ����
    // �� �ٽ� �ε�
    public void ResetSaveData()
    {
        UiManager.Instance.ShowMainUi();
        SaveLoadSystem.Delete((int)SaveLoadSystem.SaveType.Catalouge);
        PlayerPrefs.SetInt("TutorialCheck", 0);
        FloorManager.Instance.MoveToSelectFloor("B1");
        SetEmptyData();
        GameManager.Instance.SetPlayerData();
        UiManager.Instance.ShowTutorial();
    }

    // �߰� �÷��� ���̺� ���� �ε�
    public void SetPlayingData()
    {
        GameManager.Instance.SetPlayerData();

        var playingWorld = SaveLoadSystem.Load(SaveLoadSystem.SaveType.PlayingWorld) as SaveDataV1;
        var playingCurrency = SaveLoadSystem.Load(SaveLoadSystem.SaveType.PlayingCurrency) as SaveCurrencyDataV1;

        // ���� ���忡 ����� �ý��� �ʱ�ȭ
        var floors = FloorManager.Instance.floors;

        foreach (var floor in floors.Values)
        {
            floor.RemoveAllAnimals();
        }

        for (int i = 0; i < playingWorld.floors.Count; ++i)
        {
            var floorSaveData = playingWorld.floors[i];
            var savedAnimals = playingWorld.floors[i].animalSaveDatas;
            var savedBuildings = playingWorld.floors[i].buildingSaveDatas;

            var floor = FloorManager.Instance.GetFloor($"B{floorSaveData.floorStat.Floor_Num}");
            floor.FloorStat = floorSaveData.floorStat;

            foreach (var animal in savedAnimals)
            {
                var pos = floor.transform.position;
                pos.z -= 5;
                GameManager.Instance.GetAnimalManager().Create(pos, floor, animal.animalStat.Animal_ID, 0, animal.animalStat);
            }

            for (int j = 0; j < savedBuildings.Count; ++j)
            {
                floor.buildings[j].BuildingStat = savedBuildings[j].buildingStat;
                if (floor.buildings[j].BuildingStat.IsLock)
                {
                    floor.buildings[j].gameObject.SetActive(false);
                }
                else
                {
                    floor.buildings[j].gameObject.SetActive(true);
                }
            }
        }


        for (int i = 0; i < playingCurrency.currencySaveData.Count; ++i)
        {
            CurrencyManager.currency[CurrencyManager.currencyTypes[i]] = playingCurrency.currencySaveData[i].value;
        }
    }


    public void SetEmptyData()
    {
        var emptyWorld = SaveLoadSystem.Load(SaveLoadSystem.SaveType.EmptyWorld) as SaveDataV1;
        var emptyCurrency = SaveLoadSystem.Load(SaveLoadSystem.SaveType.EmptyCurrency) as SaveCurrencyDataV1;
        var emptyCurrencyProduct = SaveLoadSystem.Load(SaveLoadSystem.SaveType.EmptyCurrencyProduct) as SaveCurrencyProductDataV1;

        if (emptyWorld == null)
            return;
        if (emptyCurrency == null)
            return;
        if (emptyCurrencyProduct == null)
            return;

        // ���� ���忡 ����� �ý��� �ʱ�ȭ
        var floors = FloorManager.Instance.floors;

        foreach(var floor in floors.Values)
        {
            floor.RemoveAllAnimals();
            if(floor.FloorStat.Floor_Num > 3)
            {
                var storageConduct = (floor as BuildingFloor).storageConduct;
                storageConduct.ResetStorageConduct();
            }
        }


        for(int i = 0; i < emptyWorld.floors.Count; ++i)
        {
            var floorSaveData = emptyWorld.floors[i];
            var savedBuildings = emptyWorld.floors[i].buildingSaveDatas;
            var saveFurnitures = emptyWorld.floors[i].furnitureSaveDatas;

            var floor = FloorManager.Instance.GetFloor($"B{floorSaveData.floorStat.Floor_Num}");
            floor.FloorStat = floorSaveData.floorStat;

            for (int j = 0; j < savedBuildings.Count; ++j)
            {
                floor.buildings[j].BuildingStat = savedBuildings[j].buildingStat;
                if(floor.buildings[j].BuildingStat.IsLock)
                {
                    floor.buildings[j].gameObject.SetActive(false);
                }
                else
                {
                    floor.buildings[j].gameObject.SetActive(true);
                }
            }
        }


        for(int i = 0; i < emptyCurrency.currencySaveData.Count; ++i)
        {
            CurrencyManager.currency[CurrencyManager.currencyTypes[i]] = emptyCurrency.currencySaveData[i].value;
        }

        for(int i = 0; i < emptyCurrencyProduct.currencySaveData.Count; ++i)
        {
            CurrencyManager.product[CurrencyManager.productTypes[i]] = emptyCurrencyProduct.currencySaveData[i].value;
        }

        var storageProduct = (FloorManager.Instance.floors["B3"].storage as StorageProduct);
        storageProduct.SetEmpty();
    }

    public void OnClickApplicationQuit()
    {
        Application.Quit();
    }
}