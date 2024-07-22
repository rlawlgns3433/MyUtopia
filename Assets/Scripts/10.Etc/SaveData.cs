using System;
using System.Collections.Generic;
using System.Linq.Expressions;

public abstract class SaveData
{
    public int Version { get; protected set; }

    public abstract SaveData VersionUp();
}

public class AnimalSaveData
{
    public AnimalStat animalStat;
    public AnimalSaveData() { }

    public AnimalSaveData(AnimalStat animalStat)
    {
        this.animalStat = animalStat;
    }
}

public class BuildingSaveData
{
    public BuildingStat buildingStat;

    public BuildingSaveData() { }
    public BuildingSaveData(BuildingStat buildingStat)
    {
        this.buildingStat = buildingStat;
    }
}

public class FurnitureSaveData
{
    public FurnitureStat furnitureStat;

    public FurnitureSaveData(FurnitureStat furnitureStat)
    {
        this.furnitureStat = furnitureStat;
    }
}

public class FloorSaveData
{
    public FloorStat floorStat;
    public List<AnimalSaveData> animalSaveDatas;
    public List<BuildingSaveData> buildingSaveDatas;
    public List<FurnitureSaveData> furnitureSaveDatas;

    public FloorSaveData() { }

    public FloorSaveData(int floorId) 
    {
        floorStat = new FloorStat(floorId);
        animalSaveDatas = new List<AnimalSaveData>();
        buildingSaveDatas = new List<BuildingSaveData>();
        furnitureSaveDatas = new List<FurnitureSaveData>();
    }
    public FloorSaveData(FloorStat floorStat)
    {
        this.floorStat = floorStat;
        animalSaveDatas = new List<AnimalSaveData>();
        buildingSaveDatas = new List<BuildingSaveData>();
        furnitureSaveDatas = new List<FurnitureSaveData>();
    }
}

public class CurrencySaveData
{
    public CurrencyType currencyType;
    public BigNumber value;

    public CurrencySaveData() { }
    public CurrencySaveData(CurrencyType key, BigNumber value)
    {
        currencyType = key;
        this.value = value;
    }
}

public class SaveDataV1 : SaveData
{
    public List<FloorSaveData> floors;
    public SaveDataV1()
    {
        floors = new List<FloorSaveData>();
        Version = 1;
    }
    public override SaveData VersionUp()
    {
        return null;
    }
}

public class SaveCurrencyDataV1 : SaveData
{
    public List<CurrencySaveData> currencySaveData;
    public SaveCurrencyDataV1()
    {
        currencySaveData = new List<CurrencySaveData>();
        Version = 1;
    }
    public override SaveData VersionUp()
    {
        return null;
    }
}



public class SaveDataV2 : SaveData
{
    public int Gold { get; set; } = 100;
    public string Name { get; set; } = "Empty";

    public  SaveDataV2()
    {
        Version = 2;
    }

    public override SaveData VersionUp()
    {
        return null;
    }
}