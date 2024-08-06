using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public struct MissionData
{
    public int Mission_ID { get; set; }
    public string Mission_Desc_ID { get; set; }
    public int World_Type { get; set; }
    public int Mission_Type { get; set; }
    public bool Available { get; set; }
    public bool Repeat { get; set; }
    public int Difficulty { get; set; }
    public int Check_type { get; set; }
    public int Target_ID { get; set; }
    public int Count { get; set; }
    public int Pre_Event_Type { get; set; }
    public int Pre_Event { get; set; }
    public string Pre_Event_Value { get; set; }
    public int Pre_Mission_ID { get; set; }
    public int Reward_ID { get; set; }
    public int Today_Mission_Point { get; set; }

    public string GetDesc()
    {
        return DataTableMgr.GetStringTable().Get(Mission_Desc_ID);
    }
}

public class MissionTable : DataTable
{
    public static readonly MissionData defaultData = new MissionData();
    private Dictionary<int, MissionData> table = new Dictionary<int, MissionData>();
    public override bool IsLoaded { get; protected set; }

    public override void Load(string path)
    {
        path = string.Format(FormatPath, path);

        table.Clear();

        Addressables.LoadAssetAsync<TextAsset>(DataTableIds.Mission).Completed += (AsyncOperationHandle<TextAsset> handle) =>
        {
            using (var reader = new StringReader(handle.Result.text))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csvReader.Read();
                csvReader.ReadHeader();
                csvReader.Read();

                var records = csvReader.GetRecords<MissionData>();
                foreach (var record in records)
                {
                    table.Add(record.Mission_ID, record);
                }
            }
            IsLoaded = true;
        };
    }

    public MissionData Get(int id)
    {
        if (!table.ContainsKey(id))
            return defaultData;
        return table[id];
    }

    public List<MissionData> GetAllMissions()
    {
        return new List<MissionData>(table.Values);
    }
}
