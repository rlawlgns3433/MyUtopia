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
    public BuildingStat buildingStat;

    public FurnitureSaveData(BuildingStat buildingStat)
    {
        this.buildingStat = buildingStat;
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

public class CurrencyProductSaveData
{
    public CurrencyProductType currencyProductType;
    public BigNumber value;

    public CurrencyProductSaveData() { }
    public CurrencyProductSaveData(CurrencyProductType key, BigNumber value)
    {
        currencyProductType = key;
        this.value = value;
    }
}

public class ProductSaveData
{
    public int productId;
    public int productValue;

    public ProductSaveData() { }
    public ProductSaveData(int productId, int productValue)
    {
        this.productId = productId;
        this.productValue = productValue;
    }
}

public class PatronBoardSaveData
{
    public int id;
    public bool isCompleted;

    public PatronBoardSaveData() { }
    public PatronBoardSaveData(int id, bool isCompleted)
    {
        this.id = id;
        this.isCompleted = isCompleted;
    }
}

public class SaveDataV1 : SaveData
{
    public List<FloorSaveData> floors;
    public List<MissionSaveData> dailyMissions;
    public List<MissionSaveData> weeklyMissions;
    public List<MissionSaveData> monthlyMissions;
    public float dailyPoint;
    public float weeklyPoint;
    public float monthlyPoint;

    public SaveDataV1()
    {
        floors = new List<FloorSaveData>();
        dailyMissions = new List<MissionSaveData>();
        weeklyMissions = new List<MissionSaveData>();
        monthlyMissions = new List<MissionSaveData>();
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

public class SaveCurrencyProductDataV1 : SaveData
{
    public List<CurrencyProductSaveData> currencySaveData;
    public SaveCurrencyProductDataV1()
    {
        currencySaveData = new List<CurrencyProductSaveData>();
        Version = 1;
    }
    public override SaveData VersionUp()
    {
        return null;
    }
}

public class SaveProductDataV1 : SaveData
{
    public List<ProductSaveData> productSaveData;
    public SaveProductDataV1()
    {
        productSaveData = new List<ProductSaveData>();
        Version = 1;
    }
    public override SaveData VersionUp()
    {
        return null;
    }
}

public class SavePatronDataV1 : SaveData
{
    public List<PatronBoardSaveData> patronboardSaveData;
    public DateTime dateTime;
    public SavePatronDataV1()
    {
        patronboardSaveData = new List<PatronBoardSaveData>();
        dateTime = DateTime.UtcNow;
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

    public SaveDataV2()
    {
        Version = 2;
    }

    public override SaveData VersionUp()
    {
        return null;
    }
}