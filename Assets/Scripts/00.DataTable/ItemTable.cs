using CsvHelper;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ItemData
{
    public int Item_ID { get; set; }
    public string Item_Name_ID { get; set; }
    public string Item_Desc_ID { get; set; }
    public int Item_Type { get; set; }
    public int World_Type { get; set; }
    public int Item_Stack { get; set; }
    public int Buy_Resource_ID { get; set; }
    public string Buy_Price { get; set; }
    public int Sell_Resource_ID { get; set; }
    public string Sell_Price { get; set; }
    public int Effect_ID { get; set; }
    public string Image { get; set; }
    public string Item_prefab { get; set; }

    public string GetName()
    {
        return DataTableMgr.GetStringTable().Get(Item_Name_ID);
    }

    public string GetDescription()
    {
        return DataTableMgr.GetStringTable().Get(Item_Desc_ID);
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

public class ItemTable : DataTable
{
    private static readonly ItemData defaultData = new ItemData();
    private Dictionary<int, ItemData> table = new Dictionary<int, ItemData>();
    public override bool IsLoaded { get; protected set; }

    public Dictionary<int, ItemData> GetKeyValuePairs
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

        Addressables.LoadAssetAsync<TextAsset>(DataTableIds.Item).Completed += (AsyncOperationHandle<TextAsset> handle) =>
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

                var records = csvReader.GetRecords<ItemData>();
                foreach (var record in records)
                {
                    table.Add(record.Item_ID, record);
                }
            }

            IsLoaded = true;
        };
    }

    public ItemData Get(int id)
    {
        if (!table.ContainsKey(id))
            return defaultData;
        return table[id];
    }
}