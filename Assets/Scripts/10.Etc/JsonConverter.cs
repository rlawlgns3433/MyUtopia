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
        return result;
    }

    public override void WriteJson(JsonWriter writer, StorageData value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("workLoad");
        writer.WriteValue(value.CurrentWorkLoad.ToSimpleString());
        for(int i = 0; i < value.CurrArray.Length; i++)
        {
            writer.WritePropertyName($"Currency{i}");
            writer.WriteValue(value.CurrArray[i].ToSimpleString());
        }
        writer.WriteEndObject();
    }
}