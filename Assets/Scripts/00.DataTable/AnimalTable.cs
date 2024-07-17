using CsvHelper;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AnimalData
{
    public int Animal_ID { get; set; }
    public int Animal_Type { get; set; }
    public int Animal_Grade { get; set; }
    public string Animal_Name_ID { get; set; }
    public int Level { get; set; }
    public int Level_Max { get; set; }
    public int Workload { get; set; }
    public float Stamina { get; set; }
    public int Merge_ID { get; set; }
    public string Sale_Coin { get; set; }
    public string Level_Up_Coin_ID { get; set; }
    public string Level_Up_Coin_Value { get; set; }
    public string Prefab { get; set; }
    public string Profile { get; set; }
    // 데이터 테이블에 따른 추가 필요

    public string GetName()
    {
        return DataTableMgr.GetStringTable().Get(Animal_Name_ID);
    }

    public Sprite GetPrefab()
    {
        return Addressables.LoadAssetAsync<Sprite>(Prefab).Result;
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

public class AnimalTable : DataTable
{
    private static readonly AnimalData defaultData = new AnimalData();
    private Dictionary<int, AnimalData> table = new Dictionary<int, AnimalData>();
    public override bool IsLoaded { get; protected set; }

    public Dictionary<int, AnimalData> GetKeyValuePairs
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

        Addressables.LoadAssetAsync<TextAsset>(DataTableIds.Animal).Completed += (AsyncOperationHandle<TextAsset> handle) => 
        {
            if(handle.Result == null)
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

                var records = csvReader.GetRecords<AnimalData>();
                foreach (var record in records)
                {
                    table.Add(record.Animal_ID, record);
                    //Debug.Log(record.Profile);
                }
            }

            IsLoaded = true;
        };
    }

    public AnimalData Get(int id)
    {
        if (!table.ContainsKey(id))
            return defaultData;
        return table[id];
    }
}