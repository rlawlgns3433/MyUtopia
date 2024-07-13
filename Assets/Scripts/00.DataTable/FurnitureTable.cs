using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// 데이터 테이블 변경에 따른 수정 필요

public struct FurnitureData
{
    public int Furniture_ID { get; set; }
    public int Floor_Type { get; set; }
    public string Furniture_Name_ID { get; set; }
    public string Furniture_Desc_ID { get; set; }
    public int Level { get; set; }
    public int Level_Max { get; set; }
    public int Effect_Type { get; set; }
    public int Effect_Value { get; set; }
    public int Level_Up_Coin_ID { get; set; }
    public string Level_Up_Coin_Value { get; set; }
    public string Prefab { get; set; }

    public string GetFurnitureName()
    {
        return DataTableMgr.GetStringTable().Get(Furniture_Name_ID);
    }

    public string GetFurnitureDesc()
    {
        return DataTableMgr.GetStringTable().Get(Furniture_Desc_ID);
    }

    public Sprite GetPrefab()
    {
        return Addressables.LoadAssetAsync<Sprite>(Prefab).Result;
    }
}

public class FurnitureTable : DataTable
{
    public static readonly FurnitureData defaultData = new FurnitureData();
    private Dictionary<int, FurnitureData> table = new Dictionary<int, FurnitureData>();
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

                var records = csvReader.GetRecords<FurnitureData>();
                foreach (var record in records)
                {
                    table.Add(record.Furniture_ID, record);
                }
            }
            IsLoaded = true;
        };
    }

    public FurnitureData Get(int id)
    {
        if (!table.ContainsKey(id))
            return defaultData;
        return table[id];
    }
}
