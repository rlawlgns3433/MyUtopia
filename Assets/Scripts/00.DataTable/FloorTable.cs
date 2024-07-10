using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// 데이터 테이블 변경에 따른 수정 필요

public struct FloorData
{
    public int ID { get; set; }
    public int World_Type { get; set; }
    public int Floor_Num { get; set; }
    public int Type { get; set; }
    public string Name { get; set; }
    public int Grade { get; set; }
    public int Grade_Max { get; set; }
    public int Unlock_Facility { get; set; }
    public int Unlock_Content { get; set; }
    public int Max_Population { get; set; } 
    public string Level_Up_Coin { get; set; } // string(991c) -> BigInteger
    public int Level_Up_Resource_1 { get; set; }
    public string Resource_1_Value { get; set; }
    public int Level_Up_Resource_2 { get; set; }
    public string Resource_2_Value { get; set; }
    public int Level_Up_Resource_3 { get; set; }
    public string Resource_3_Value { get; set; }


    public string GetFloorName()
    {
        return DataTableMgr.GetStringTable().Get(Name);
    }
}

public class FloorTable : DataTable
{
    public static readonly FloorData defaultData = new FloorData();
    private Dictionary<int, FloorData> table = new Dictionary<int, FloorData>();
    public override void Load(string path)
    {
        path = string.Format(FormatPath, path);

        table.Clear();

        Addressables.LoadAssetAsync<TextAsset>(DataTableIds.Floor).Completed += (AsyncOperationHandle<TextAsset> handle) =>
        {
            using (var reader = new StringReader(handle.Result.text))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csvReader.Read();
                csvReader.ReadHeader();
                csvReader.Read();

                var records = csvReader.GetRecords<FloorData>();
                foreach (var record in records)
                {
                    table.Add(record.ID, record);
                }
            }
        };
    }

    public FloorData Get(int id)
    {
        if (!table.ContainsKey(id))
            return defaultData;
        return table[id];
    }
}
