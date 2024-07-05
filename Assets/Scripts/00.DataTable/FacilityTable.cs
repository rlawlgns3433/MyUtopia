using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public struct FacilityData
{
    public int ID { get; set; }
    public int Floor_Type { get; set; }
    public string Name { get; set; }
    public int Level { get; set; }
    public int Level_Max { get; set; }
    public int Effect_Type { get; set; }
    public float Effect_Value { get; set; }
    public string Level_Up_Coin { get; set; }

    public string GetName()
    {
        return DataTableMgr.GetStringTable().Get(Name);
    }
}


public class FacilityTable : DataTable
{
    private static readonly FacilityData defaultData = new FacilityData();
    private Dictionary<int, FacilityData> table = new Dictionary<int, FacilityData>();
    public override void Load(string path)
    {
        path = string.Format(FormatPath, path);

        table.Clear();
        Addressables.LoadAssetAsync<TextAsset>(DataTableIds.Facility).Completed += (AsyncOperationHandle<TextAsset> handle) =>
        {
            using (var reader = new StringReader(handle.Result.text))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csvReader.Read();
                csvReader.ReadHeader();
                csvReader.Read();

                var records = csvReader.GetRecords<FacilityData>();
                foreach (var record in records)
                {
                    table.Add(record.ID, record);
                }
            }
        };
    }

    public FacilityData Get(int id)
    {
        if (table.ContainsKey(id))
        {
            return table[id];
        }
        return defaultData;
    }
}
