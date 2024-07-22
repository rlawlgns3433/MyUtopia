using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.VisualScripting;

public class QuitTimeConverter : JsonConverter<TimeData>
{
    public override TimeData ReadJson(JsonReader reader, Type objectType, TimeData existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        TimeData timeData = new TimeData();
        JObject jObj = JObject.Load(reader);
        timeData.EnterTime = jObj["enterTime"]?.ToString();
        if (jObj["quitTime"] != null)
        {
            timeData.QuitTime = jObj["quitTime"].ToObject<float>();
        }

        return timeData;
    }

    public override void WriteJson(JsonWriter writer, TimeData value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("enterTime");
        writer.WriteValue(value.EnterTime); // ISO 8601 형식으로 저장
        writer.WritePropertyName("quitTime");
        writer.WriteValue(value.QuitTime);
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
        while (!DataTableMgr.GetFurnitureTable().IsLoaded) continue;

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

            var animals = floorProperties["Animals"];
            foreach (var animal in animals)
            {
                AnimalSaveData animalSaveData;
                var animalStat = new AnimalStat((int)animal["Id"]);
                animalStat.Animal_ID = (int)animal["Id"];
                animalStat.Stamina = (float)animal["Stamina"];
                animalStat.CurrentFloor = $"B{floorData.floorStat.Floor_Num}";
                animalSaveData = new AnimalSaveData(animalStat);

                floorData.animalSaveDatas.Add(animalSaveData);
            }

            var buildings = floorProperties["Buildings"];
            foreach (var building in buildings)
            {
                BuildingSaveData buildingData = new BuildingSaveData
                {
                    buildingStat = new BuildingStat
                    (
                        (int)building["Id"]
                    )
                };
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