using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public struct BuildingData
{
    public int ID { get; set; }
    public int Floor_Type { get; set; }
    public string Name { get; set; }
    public int Level { get; set; }
    public int Level_Max { get; set; }
    public float Work_Require { get; set; }
    public int Recipe_Reward { get; set; } // 레시피 테이블 ID
    public int Resource_Type { get; set; } // 생산 재화 타입
    public int Touch_Produce { get; set; }
    public string Level_Up_Coin { get; set; }
    public int Level_Up_Resource_1 { get; set; }
    public string Resource_1_Value { get; set; }
    public int Level_Up_Resource_2 { get; set; }
    public string Resource_2_Value { get; set; }
    public int Level_Up_Resource_3 { get; set; }
    public string Resource_3_Value { get; set; }

    public string GetName()
    {
        return DataTableMgr.GetStringTable().Get(Name);
    }
}

public class BuildingTable : DataTable
{
    private static readonly BuildingData defaultData = new BuildingData();
    private Dictionary<int, BuildingData> table = new Dictionary<int, BuildingData>();

    public override void Load(string path)
    {
        path = string.Format(FormatPath, path);

        table.Clear();

        Addressables.LoadAssetAsync<TextAsset>(DataTableIds.Building).Completed += (AsyncOperationHandle<TextAsset> handle) =>
        {
            using (var reader = new StringReader(handle.Result.text))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csvReader.Read();
                csvReader.ReadHeader();
                csvReader.Read();

                var records = csvReader.GetRecords<BuildingData>();
                foreach (var record in records)
                {
                    table.Add(record.ID, record);
                }
            }
        };
    }

    public BuildingData Get(int id)
    {
        if (!table.ContainsKey(id))
            return defaultData;
        return table[id];
    }
}