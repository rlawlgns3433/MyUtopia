using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

public class QuitTimeConverter : JsonConverter<TimeData>
{
    public override TimeData ReadJson(JsonReader reader, Type objectType, TimeData existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        TimeData timeData = new TimeData();
        JObject jObj = JObject.Load(reader);
        timeData.EnterTime = jObj["enterTime"]?.ToString();
        if (jObj["quitTime"] != null)
        {
            timeData.QuitTime = jObj["quitTime"].ToString();
        }
        timeData.LastDaily = jObj["lastDaily"]?.ToString();
        timeData.LastWeekly = jObj["lastWeekly"]?.ToString();
        timeData.LastMonthly = jObj["lastMonthly"]?.ToString();
        timeData.FirstLogInDaily = jObj["FirstLogInDaily"]?.ToString();
        return timeData;
    }

    public override void WriteJson(JsonWriter writer, TimeData value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("enterTime");
        writer.WriteValue(value.EnterTime);
        writer.WritePropertyName("quitTime");
        writer.WriteValue(value.QuitTime);
        writer.WritePropertyName("lastDaily");
        writer.WriteValue(value.LastDaily);
        writer.WritePropertyName("lastWeekly");
        writer.WriteValue(value.LastWeekly);
        writer.WritePropertyName("lastMonthly");
        writer.WriteValue(value.LastMonthly);
        writer.WritePropertyName("FirstLogInDaily");
        writer.WriteValue(value.FirstLogInDaily);
        writer.WriteEndObject();
    }
}
public class MissionDataConverter : JsonConverter<SaveMissionData>
{
    public override SaveMissionData ReadJson(JsonReader reader, Type objectType, SaveMissionData existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject jObj = JObject.Load(reader);
        SaveMissionData gameData = new SaveMissionData
        {
            dailyPoint = jObj["dailyPoint"].ToObject<float>(),
            weeklyPoint = jObj["weeklyPoint"].ToObject<float>(),
            monthlyPoint = jObj["monthlyPoint"].ToObject<float>(),
            dailyMissions = jObj["dailyMissions"].ToObject<List<MissionSaveData>>(),
            weeklyMissions = jObj["weeklyMissions"].ToObject<List<MissionSaveData>>(),
            monthlyMissions = jObj["monthlyMissions"].ToObject<List<MissionSaveData>>(),
            preMissions = jObj["preMissions"].ToObject<List<PreMissionData>>(),
            completeDailyMission = jObj["completeDailyMission"].ToObject<List<bool>>(),
        };
        return gameData;
    }

    public override void WriteJson(JsonWriter writer, SaveMissionData value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("dailyPoint");
        writer.WriteValue(value.dailyPoint);
        writer.WritePropertyName("weeklyPoint");
        writer.WriteValue(value.weeklyPoint);
        writer.WritePropertyName("monthlyPoint");
        writer.WriteValue(value.monthlyPoint);
        writer.WritePropertyName("dailyMissions");
        serializer.Serialize(writer, value.dailyMissions);
        writer.WritePropertyName("weeklyMissions");
        serializer.Serialize(writer, value.weeklyMissions);
        writer.WritePropertyName("monthlyMissions");
        serializer.Serialize(writer, value.monthlyMissions);
        writer.WritePropertyName("preMissions");
        serializer.Serialize(writer, value.preMissions);
        writer.WritePropertyName("completeDailyMission");
        serializer.Serialize(writer, value.completeDailyMission);
        writer.WriteEndObject();
    }
}

public class CatalougeDataConverter : JsonConverter<SaveCatalogueData>
{
    public override SaveCatalogueData ReadJson(JsonReader reader, Type objectType, SaveCatalogueData existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject jObj = JObject.Load(reader);
        SaveCatalogueData gameData = new SaveCatalogueData
        {
            catalogueDatas = jObj["catalogueDatas"].ToObject<List<CatalogueData>>(),
            isGetFirstAnimal = jObj["firstGetAnimal"].ToObject<bool>(),
        };
        return gameData;
    }

    public override void WriteJson(JsonWriter writer, SaveCatalogueData value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("catalogueDatas");
        serializer.Serialize(writer, value.catalogueDatas);
        writer.WritePropertyName("firstGetAnimal");
        writer.WriteValue(value.isGetFirstAnimal);
        writer.WriteEndObject();
    }
}

public class WorkLoadConverter : JsonConverter<StorageData>
{
    public override StorageData ReadJson(JsonReader reader, Type objectType, StorageData existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        StorageData result = new StorageData();
        JObject jObj = JObject.Load(reader);
        result.CurrentWorkLoad = new BigNumber(jObj["workLoad"]?.ToString());
        var currArray = new List<BigNumber>();
        int i = 0;
        while (jObj.TryGetValue($"Currency{i}", out JToken token))
        {
            currArray.Add(new BigNumber(token.ToString()));
            i++;
        }
        result.CurrArray = currArray.ToArray();
        result.TotalOfflineTime = jObj["totalOfflineTime"].ToObject<int>();
        return result;
    }

    public override void WriteJson(JsonWriter writer, StorageData value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("workLoad");
        writer.WriteValue(value.CurrentWorkLoad.ToSimpleString());
        for (int i = 0; i < value.CurrArray.Length; i++)
        {
            writer.WritePropertyName($"Currency{i}");
            writer.WriteValue(value.CurrArray[i].ToSimpleString());
        }
        writer.WritePropertyName("totalOfflineTime");
        writer.WriteValue(value.TotalOfflineTime.ToString());
        writer.WriteEndObject();
    }
}
public class WorldConverter : JsonConverter<List<FloorSaveData>>
{
    public override List<FloorSaveData> ReadJson(JsonReader reader, Type objectType, List<FloorSaveData> existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        while (!DataTableMgr.GetAnimalTable().IsLoaded) continue;
        while (!DataTableMgr.GetFloorTable().IsLoaded) continue;
        while (!DataTableMgr.GetBuildingTable().IsLoaded) continue;
        //while (!DataTableMgr.GetFurnitureTable().IsLoaded) continue;

        JObject jsonObject = JObject.Load(reader);
        var floorsToken = jsonObject["floors"];

        if (floorsToken == null)
        {
            return new List<FloorSaveData>();
        }

        List<FloorSaveData> floorSaveDataList = new List<FloorSaveData>();

        foreach (var floor in floorsToken.Children<JProperty>())
        {
            var floorProperties = floor.Value;
            FloorSaveData floorData = new FloorSaveData((int)floorProperties["Id"]);
            floorData.floorStat.IsLock = (bool)floorProperties["IsLock"];
            floorData.floorStat.IsUpgrading = (bool)floorProperties["IsUpgrading"];
            floorData.floorStat.UpgradeTimeLeft = (double)floorProperties["UpgradeTimeLeft"];

            var animals = floorProperties["Animals"];
            foreach (var animal in animals)
            {
                AnimalSaveData animalSaveData;
                var animalStat = new AnimalStat((int)animal["Id"]);
                animalStat.Animal_ID = (int)animal["Id"];
                animalStat.Stamina = (float)animal["Stamina"];
                animalStat.AcquireTime = (DateTime)animal["AcquireTime"];
                animalStat.CurrentFloor = $"B{floorData.floorStat.Floor_Num}";
                animalSaveData = new AnimalSaveData(animalStat);

                floorData.animalSaveDatas.Add(animalSaveData);
            }

            var buildings = floorProperties["Buildings"];
            foreach (var building in buildings)
            {
                BuildingSaveData buildingData;
                var builidingStat = new BuildingStat((int)building["Id"]);
                builidingStat.IsLock = (bool)building["IsLock"];
                builidingStat.IsUpgrading = (bool)building["IsUpgrading"];
                builidingStat.UpgradeTimeLeft = (double)building["UpgradeTimeLeft"];
                buildingData = new BuildingSaveData(builidingStat);
                floorData.buildingSaveDatas.Add(buildingData);
            }
            floorSaveDataList.Add(floorData);
        }

        return floorSaveDataList;
    }


    public override void WriteJson(JsonWriter writer, List<FloorSaveData> value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("floors");
        writer.WriteStartObject();

        for (int i = 0; i < value.Count; ++i)
        {
            writer.WritePropertyName($"B{i + 1}");
            writer.WriteStartObject();

            writer.WritePropertyName("Id");
            writer.WriteValue(value[i].floorStat.Floor_ID);

            writer.WritePropertyName("IsLock");
            writer.WriteValue(value[i].floorStat.IsLock);

            writer.WritePropertyName("IsUpgrading");
            writer.WriteValue(value[i].floorStat.IsUpgrading);

            writer.WritePropertyName("UpgradeTimeLeft");
            writer.WriteValue(value[i].floorStat.UpgradeTimeLeft);

            // Write animal data
            writer.WritePropertyName("Animals");
            writer.WriteStartArray();
            for (int j = 0; j < value[i].animalSaveDatas.Count; ++j)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Id");
                writer.WriteValue(value[i].animalSaveDatas[j].animalStat.Animal_ID);
                writer.WritePropertyName("Stamina");
                writer.WriteValue(value[i].animalSaveDatas[j].animalStat.Stamina);
                writer.WritePropertyName("AcquireTime");
                writer.WriteValue(value[i].animalSaveDatas[j].animalStat.AcquireTime);
                writer.WritePropertyName("CurrentFloor");
                writer.WriteValue(value[i].animalSaveDatas[j].animalStat.CurrentFloor);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            // Write building data
            writer.WritePropertyName("Buildings");
            writer.WriteStartArray();
            for (int j = 0; j < value[i].buildingSaveDatas.Count; ++j)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Id");
                writer.WriteValue(value[i].buildingSaveDatas[j].buildingStat.Building_ID);                
                writer.WritePropertyName("IsLock");
                writer.WriteValue(value[i].buildingSaveDatas[j].buildingStat.IsLock);
                writer.WritePropertyName("IsUpgrading");
                writer.WriteValue(value[i].buildingSaveDatas[j].buildingStat.IsUpgrading);
                writer.WritePropertyName("UpgradeTimeLeft");
                writer.WriteValue(value[i].buildingSaveDatas[j].buildingStat.UpgradeTimeLeft);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.WriteEndObject();
        }

        writer.WriteEndObject();
        writer.WriteEndObject();
    }
}


public class CurrencyConverter : JsonConverter<List<CurrencySaveData>>
{
    public override List<CurrencySaveData> ReadJson(JsonReader reader, Type objectType, List<CurrencySaveData> existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        List<CurrencySaveData> currencyData = new List<CurrencySaveData>();
        JObject jObj = JObject.Load(reader);

        // "Currency" 속성의 배열을 가져옴
        JArray currencyArray = (JArray)jObj["Currency"];

        // 각 배열 요소를 CurrencySaveData 객체로 변환
        foreach (var item in currencyArray)
        {
            var currencySaveData = new CurrencySaveData();
            foreach (var property in item)
            {
                if (property is JProperty prop)
                {
                    if (Enum.TryParse(prop.Name, out CurrencyType currencyType))
                    {
                        currencySaveData.currencyType = currencyType;
                        currencySaveData.value = prop.Value.ToString().ToBigNumber();
                    }
                }
            }
            currencyData.Add(currencySaveData);
        }

        return currencyData;
    }


    public override void WriteJson(JsonWriter writer, List<CurrencySaveData> value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("Currency");
        writer.WriteStartArray();
        for (int i = 0; i < value.Count; ++i)
        {
            writer.WriteStartObject();
            writer.WritePropertyName($"{value[i].currencyType}");
            writer.WriteValue(value[i].value.ToFullString());
            writer.WriteEndObject();
        }
        writer.WriteEndArray();
        writer.WriteEndObject();
    }
}


public class CurrencyProductConverter : JsonConverter<List<CurrencyProductSaveData>>
{
    public override List<CurrencyProductSaveData> ReadJson(JsonReader reader, Type objectType, List<CurrencyProductSaveData> existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        List<CurrencyProductSaveData> currencyData = new List<CurrencyProductSaveData>();
        JObject jObj = JObject.Load(reader);

        // "Currency" 속성의 배열을 가져옴
        JArray currencyArray = (JArray)jObj["Currency"];

        // 각 배열 요소를 CurrencySaveData 객체로 변환
        foreach (var item in currencyArray)
        {
            var currencySaveData = new CurrencyProductSaveData();
            foreach (var property in item)
            {
                if (property is JProperty prop)
                {
                    if (Enum.TryParse(prop.Name, out CurrencyProductType currencyType))
                    {
                        currencySaveData.currencyProductType = currencyType;
                        currencySaveData.value = prop.Value.ToString().ToBigNumber();
                    }
                }
            }
            currencyData.Add(currencySaveData);
        }

        return currencyData;
    }


    public override void WriteJson(JsonWriter writer, List<CurrencyProductSaveData> value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("Currency");
        writer.WriteStartArray();
        for (int i = 0; i < value.Count; ++i)
        {
            writer.WriteStartObject();
            writer.WritePropertyName($"{value[i].currencyProductType}");
            writer.WriteValue(value[i].value.ToFullString());
            writer.WriteEndObject();
        }
        writer.WriteEndArray();
        writer.WriteEndObject();
    }
}

public class ProductConverter : JsonConverter<List<ProductSaveData>>
{
    public override List<ProductSaveData> ReadJson(JsonReader reader, Type objectType, List<ProductSaveData> existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject jObj = JObject.Load(reader);
        JArray productArray = (JArray)jObj["Products"];
        return productArray.ToObject<List<ProductSaveData>>();
    }


    public override void WriteJson(JsonWriter writer, List<ProductSaveData> value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("Products");
        writer.WriteStartArray();
        for (int i = 0; i < value.Count; ++i)
        {
            writer.WriteStartObject();
            writer.WritePropertyName($"{value[i].productId}");
            writer.WriteValue(value[i].productValue);
            writer.WriteEndObject();
        }
        writer.WriteEndArray();
        writer.WriteEndObject();
    }
}

public class PatronBoardConverter : JsonConverter<List<PatronBoardSaveData>>
{
    public override List<PatronBoardSaveData> ReadJson(JsonReader reader, Type objectType, List<PatronBoardSaveData> existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject jObj = JObject.Load(reader);
        JArray productArray = (JArray)jObj["Products"];
        return productArray.ToObject<List<PatronBoardSaveData>>();
    }


    public override void WriteJson(JsonWriter writer, List<PatronBoardSaveData> value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("Products");
        writer.WriteStartArray();
        for (int i = 0; i < value.Count; ++i)
        {
            writer.WriteStartObject();
            writer.WritePropertyName($"Id");
            writer.WriteValue(value[i].id);
            writer.WritePropertyName($"IsCompleted");
            writer.WriteValue(value[i].isCompleted);
            Debug.Log($"{value[i].id} : {value[i].isCompleted}");
            writer.WriteEndObject();
        }
        writer.WriteEndArray();
        writer.WriteEndObject();
    }
}