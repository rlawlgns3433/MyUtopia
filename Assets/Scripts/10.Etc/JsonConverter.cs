using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

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


public class WorldConverter : JsonConverter<SaveDataV1>
{
    public override SaveDataV1 ReadJson(JsonReader reader, Type objectType, SaveDataV1 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        SaveDataV1 saveDataV1 = new SaveDataV1();

        saveDataV1.floors = new List<FloorSaveData>();

        JObject jObj = JObject.Load(reader);

        for (int i = 0; i < existingValue.floors.Count; ++i)
        {
            saveDataV1.floors[i].floor = jObj[$"B{i + 1}"].ToObject<Floor>(); // 각 계층

            for (int j = 0; j < existingValue.floors[i].animalSaveDatas.Count; ++j)
            {
                saveDataV1.floors[i].animalSaveDatas[j].animalStat = jObj[$"B{i + 1}"]["Animal{j + 1}"].ToObject<AnimalStat>(); // 각 동물
            }

            for (int j = 0; j < existingValue.floors[i].buildingSaveDatas.Count; ++j)
            {
                saveDataV1.floors[i].buildingSaveDatas[j].buildingStat = jObj[$"B{i + 1}"]["Building{j + 1}"].ToObject<BuildingStat>(); // 각 건물
            }

            for (int j = 0; j < existingValue.floors[i].furnitureSaveDatas.Count; ++j)
            {
                saveDataV1.floors[i].furnitureSaveDatas[j].furnitureStat = jObj[$"B{i + 1}"]["Furniture{j + 1}"].ToObject<FurnitureStat>(); // 각 가구
            }
        }

        return saveDataV1;
    }

    public override void WriteJson(JsonWriter writer, SaveDataV1 value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        for (int i = 0; i < value.floors.Count; ++i)
        {
            writer.WritePropertyName($"B{i + 1}");
            writer.WriteStartObject();

            // Write animal data
            writer.WritePropertyName("Animals");
            writer.WriteStartArray();
            for (int j = 0; j < value.floors[i].animalSaveDatas.Count; ++j)
            {
                writer.WriteStartObject();
                writer.WritePropertyName($"Animal{j + 1} Id");
                writer.WriteValue(value.floors[i].animalSaveDatas[j].animalStat.Animal_ID);
                writer.WritePropertyName($"Animal{j + 1} Stamina");
                writer.WriteValue(value.floors[i].animalSaveDatas[j].animalStat.Stamina);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            // Write building data
            writer.WritePropertyName("Buildings");
            writer.WriteStartArray();
            for (int j = 0; j < value.floors[i].buildingSaveDatas.Count; ++j)
            {
                writer.WriteStartObject();
                writer.WritePropertyName($"Building{j + 1} Id");
                writer.WriteValue(value.floors[i].buildingSaveDatas[j].buildingStat.Building_ID);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.WriteEndObject();
        }

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