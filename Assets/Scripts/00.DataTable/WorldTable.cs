using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class WorldData
{
    public int World_ID { get; set; }
    public string World_Name { get; set; }
    public int World_Type { get; set; }
    public int Resource_1 { get; set; }
    public string Resource_1_Value { get; set; }
    public int Resource_2 { get; set; }
    public string Resource_2_Value { get; set; }
    public int Resource_3 { get; set; }
    public string Resource_3_Value { get; set; }
    public int Resource_4 { get; set; }
    public string Resource_4_Value { get; set; }
    public int Product_1 { get; set; }
    public int Product_1_Value { get; set; }
    public int Product_2 { get; set; }
    public int Product_2_Value { get; set; }
    public int Product_3 { get; set; }
    public int Product_3_Value { get; set; }
    public string Prefab { get; set; }


    public string GetWorldName()
    {
        return DataTableMgr.GetStringTable().Get(World_Name);
    }
}

public class WorldTable : DataTable
{
    public static readonly WorldData defaultData = new WorldData();
    private Dictionary<int, WorldData> table = new Dictionary<int, WorldData>();
    public override bool IsLoaded { get; protected set; }
    public override void Load(string path)
    {
        path = string.Format(FormatPath, path);

        table.Clear();

        Addressables.LoadAssetAsync<TextAsset>(DataTableIds.World).Completed += (AsyncOperationHandle<TextAsset> handle) =>
        {
            using (var reader = new StringReader(handle.Result.text))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csvReader.Read();
                csvReader.ReadHeader();
                csvReader.Read();

                var records = csvReader.GetRecords<WorldData>();
                foreach (var record in records)
                {
                    table.Add(record.World_ID, record);
                }
            }
            IsLoaded = true;
        };
    }

    public WorldData Get(int id)
    {
        if (!table.ContainsKey(id))
            return defaultData;
        return table[id];
    }
}
