using CsvHelper;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BuildingData
{
    public int Building_ID { get; set; }
    public int Floor_Type { get; set; }
    public string Building_Name_ID { get; set; }
    public string Building_Desc_ID { get; set; }
    public int Level { get; set; }
    public int Level_Max { get; set; }
    public int Materials_Type { get; set; }
    public int Conversion_rate { get; set; }
    public int Work_Require { get; set; }
    public int Effect_Type { get; set; }
    public int Effect_Value { get; set; }
    public int Resource_Type { get; set; } // 생산 재화 타입
    public string Touch_Produce { get; set; }
    public int Level_Up_Time { get; set; }
    public string Level_Up_Resource_ID { get; set; }
    public string Level_Up_Coin_Value { get; set; }
    public int Level_Up_Resource_1 { get; set; }
    public string Resource_1_Value { get; set; }
    public int Level_Up_Resource_2 { get; set; }
    public string Resource_2_Value { get; set; }
    public int Level_Up_Resource_3 { get; set; }
    public string Resource_3_Value { get; set; }
    public string Prefab { get; set; }
    public string Profile { get; set; }

    public string GetName()
    {
        return DataTableMgr.GetStringTable().Get(Building_Name_ID);
    }

    public string GetDescription()
    {
        return DataTableMgr.GetStringTable().Get(Building_Desc_ID);
    }

    public async UniTask<GameObject> GetPrefab()
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(Prefab);
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

public class BuildingTable : DataTable
{
    private static readonly BuildingData defaultData = new BuildingData();
    private Dictionary<int, BuildingData> table = new Dictionary<int, BuildingData>();
    public override bool IsLoaded { get; protected set; }

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
                    table.Add(record.Building_ID, record);
                }
            }
            IsLoaded = true;
        };
    }

    public BuildingData Get(int id)
    {
        if (!table.ContainsKey(id))
            return defaultData;
        return table[id];
    }
}