using UnityEngine;
using UnityEngine.SceneManagement;

public class TestPanel : MonoBehaviour
{
    public void ShowMeTheMoney()
    {
        CurrencyManager.currency[CurrencyType.Coin] += 10000;
        CurrencyManager.currency[CurrencyType.CopperStone] += 10000;
        CurrencyManager.currency[CurrencyType.SilverStone] += 10000;
        CurrencyManager.currency[CurrencyType.GoldStone] += 10000;
        CurrencyManager.currency[CurrencyType.CopperIngot] += 10000;
        CurrencyManager.currency[CurrencyType.SilverIngot] += 10000;
        CurrencyManager.currency[CurrencyType.GoldIngot] += 10000;
    }

    // 현재 세이브 파일 삭제
    // 씬 다시 로드
    public void ResetSaveData()
    {
        GameManager.Instance.SetPlayerData();
        SetEmptyData();
    }

    // 중간 플레이 세이브 파일 로드
    public void SetPlayingData()
    {
        GameManager.Instance.SetPlayerData();

        var playingWorld = SaveLoadSystem.Load(4) as SaveDataV1;
        var playingCurrency = SaveLoadSystem.Load(5) as SaveCurrencyDataV1;

        // 현재 월드에 적용된 시스템 초기화
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
            var saveFurnitures = playingWorld.floors[i].furnitureSaveDatas;

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

            for (int j = 0; j < saveFurnitures.Count; ++j)
            {
                floor.furnitures[j].FurnitureStat = saveFurnitures[j].furnitureStat;
            }
        }


        for (int i = 0; i < playingCurrency.currencySaveData.Count; ++i)
        {
            CurrencyManager.currency[CurrencyManager.currencyTypes[i]] = playingCurrency.currencySaveData[i].value;
        }
    }


    public void SetEmptyData()
    {
        var emptyWorld = SaveLoadSystem.Load(2) as SaveDataV1;
        var emptyCurrency = SaveLoadSystem.Load(3) as SaveCurrencyDataV1;

        // 현재 월드에 적용된 시스템 초기화
        var floors = FloorManager.Instance.floors;

        foreach(var floor in floors.Values)
        {
            floor.RemoveAllAnimals();
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

            for(int j = 0; j < saveFurnitures.Count; ++j)
            {
                floor.furnitures[j].FurnitureStat = saveFurnitures[j].furnitureStat;
            }
        }


        for(int i = 0; i < emptyCurrency.currencySaveData.Count; ++i)
        {
            CurrencyManager.currency[CurrencyManager.currencyTypes[i]] = emptyCurrency.currencySaveData[i].value;
        }
    }

    public void OnClickApplicationQuit()
    {
        Application.Quit();
    }
}