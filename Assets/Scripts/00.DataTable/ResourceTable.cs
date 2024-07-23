using CsvHelper;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public struct ResourceData
{
    public int Resource_ID { get; set; }
    public string Resource_Name_ID { get; set; }
    public int World_Type { get; set; }
    public int Floor_Type { get; set; }
    public int Resource_Type { get; set; }
    public int Sale_Resource_ID { get; set; }
    public string Sale_Price { get; set; }
    public string Image { get; set; }

    public string GetName()
    {
        return DataTableMgr.GetStringTable().Get(Resource_Name_ID);
    }

    public async UniTask<Sprite> GetImage()
    {
        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(Image);
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

public class ResourceTable : DataTable
{
    public static readonly ResourceData defaultData = new ResourceData();
    private Dictionary<int, ResourceData> table = new Dictionary<int, ResourceData>();
    public override bool IsLoaded { get; protected set; }

    public override void Load(string path)
    {
        path = string.Format(FormatPath, path);

        table.Clear();

        Addressables.LoadAssetAsync<TextAsset>(DataTableIds.Resource).Completed += (AsyncOperationHandle<TextAsset> handle) =>
        {
            using (var reader = new StringReader(handle.Result.text))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csvReader.Read();
                csvReader.ReadHeader();
                csvReader.Read();

                var records = csvReader.GetRecords<ResourceData>();
                foreach (var record in records)
                {
                    table.Add(record.Resource_ID, record);
                }
            }
        };
    }

    public ResourceData Get(int id)
    {
        if (!table.ContainsKey(id))
            return defaultData;
        return table[id];
    }
}
