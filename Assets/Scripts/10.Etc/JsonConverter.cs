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