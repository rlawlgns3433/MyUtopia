using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class RecipeData
{
    public int Recipe_ID { get; set; }
    public string Recipe_Name_ID { get; set; }
    public int World_Type { get; set; }
    public int Unlock_Lv { get; set; }
    public int Resource_1 { get; set; }
    public string Resource_1_Value { get; set; }
    public int Resource_2 { get; set; }
    public string Resource_2_Value { get; set; }
    public int Resource_3 { get; set; }
    public string Resource_3_Value { get; set; }
    public int Workload { get; set; }
    public int Product_ID { get; set; }

    public string GetName()
    {
        return DataTableMgr.GetStringTable().Get(Recipe_Name_ID);
    }

    public ItemData GetProduct()
    {
        return DataTableMgr.GetItemTable().Get(Product_ID);
    }
}

public class RecipeTable : DataTable
{
    private static readonly RecipeData defaultData = new RecipeData();
    private Dictionary<int, RecipeData> table = new Dictionary<int, RecipeData>();
    public override bool IsLoaded { get; protected set; }

    public Dictionary<int, RecipeData> GetKeyValuePairs
    {
        get
        {
            return table;
        }
    }

    public int Count
    {
        get
        {
            return table.Count;
        }
    }

    public override void Load(string path)
    {
        path = string.Format(FormatPath, path);

        table.Clear();

        Addressables.LoadAssetAsync<TextAsset>(DataTableIds.Recipe).Completed += (AsyncOperationHandle<TextAsset> handle) =>
        {
            if (handle.Result == null)
            {
                Debug.LogError("Failed to load table");
                return;
            }

            using (var reader = new StringReader(handle.Result.text))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csvReader.Read();
                csvReader.ReadHeader();
                csvReader.Read();

                var records = csvReader.GetRecords<RecipeData>();
                foreach (var record in records)
                {
                    table.Add(record.Recipe_ID, record);
                }
            }

            IsLoaded = true;
        };
    }

    public RecipeData Get(int id)
    {
        if (!table.ContainsKey(id))
            return defaultData;
        return table[id];
    }
}