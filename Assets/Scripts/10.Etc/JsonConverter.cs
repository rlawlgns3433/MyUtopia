using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

public class QuitTimeConverter : JsonConverter<TimeData>
{
    public override TimeData ReadJson(JsonReader reader, Type objectType, TimeData existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        TimeData timeData = new TimeData();
        JObject jObj = JObject.Load(reader);
        timeData.QuitTime = jObj["quitTime"]?.ToString();
        timeData.EnterTime = jObj["enterTime"]?.ToString();

        return timeData;
    }

    public override void WriteJson(JsonWriter writer, TimeData value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("quitTime");
        writer.WriteValue(value.QuitTime);
        writer.WritePropertyName("enterTime");
        writer.WriteValue(value.EnterTime);
        writer.WriteEndObject();
    }
}

public class WorkLoadConverter : JsonConverter<StorageTest>
{
    public override StorageTest ReadJson(JsonReader reader, Type objectType, StorageTest existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        StorageTest result = new StorageTest();
        JObject jObj = JObject.Load(reader);
        result.currBigNum = new BigNumber(jObj["workLoad"]?.ToString());
        return result;
    }

    public override void WriteJson(JsonWriter writer, StorageTest value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("workLoad");
        writer.WriteValue(value.currBigNum.ToSimpleString());
        writer.WriteEndObject();
    }
}