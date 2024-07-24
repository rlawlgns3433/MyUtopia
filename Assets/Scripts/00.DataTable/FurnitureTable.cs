using CsvHelper;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class FunitureData
{
    public int Furniture_ID { get; set; }
    public int Floor_Type { get; set; }
    public string Furniture_Name_ID { get; set; }
    public string Furniture_Desc_ID { get; set; }
    public int Level { get; set; }
    public int Level_Max { get; set; }
    public int Effect_Type { get; set; }
    public int Effect_Value { get; set; }
    public int Level_Up_Coin_ID{  get; set; }
    public string Level_Up_Coin_Value { get; set; }
    public string Prefab {  get; set; }
    public string Profile { get; set; }

    public string GetName()
    {
        return DataTableMgr.GetStringTable().Get(Furniture_Name_ID);
    }

    public string GetDescription()
    {
        return DataTableMgr.GetStringTable().Get(Furniture_Desc_ID);
    }

    public async UniTask<Sprite> GetProfile()
    {
        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(Profile);
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            return handle.Result;
        }
        else
        {
            Debug.LogError("Failed to load image.");
            return null;
        }
    }
}


public class FurnitureTable : DataTable
{
    private static readonly FunitureData defaultData = new FunitureData();
    private Dictionary<int, FunitureData> table = new Dictionary<int, FunitureData>();
    public override bool IsLoaded { get; protected set; }
    public override void Load(string path)
    {
        path = string.Format(FormatPath, path);

        table.Clear();
        Addressables.LoadAssetAsync<TextAsset>(DataTableIds.Furniture).Completed += (AsyncOperationHandle<TextAsset> handle) =>
        {
            using (var reader = new StringReader(handle.Result.text))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csvReader.Read();
                csvReader.ReadHeader();
                csvReader.Read();

                var records = csvReader.GetRecords<FunitureData>();
                foreach (var record in records)
                {
                    table.Add(record.Furniture_ID, record);
                }
            }
            IsLoaded = true;
        };
    }

    public FunitureData Get(int id)
    {
        if (table.ContainsKey(id))
        {
            return table[id];
        }
        return defaultData;
    }
}
