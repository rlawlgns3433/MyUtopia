using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public struct AnimalData
{
    public int ID { get; set; }
    public int Type { get; set; }
    public int Grade { get; set; }
    public string Name { get; set; }
    public int Level { get; set; }
    public int Level_Max { get; set; }
    public int Workload { get; set; }
    public float Stamina { get; set; }
    public int Merge_ID { get; set; }
    public string Sale_Coin { get; set; }
    public string Level_Up_Coin { get; set; }
    // 데이터 테이블에 따른 추가 필요

    public override string ToString()
    {
        return string.Format("ID: {0}, Type: {1}, Grade: {2}, Name: {3}, Level: {4}, Level_Max: {5}, Workload: {6}, Stamina: {7}, Merge_ID: {9}, Sale_Coin: {10}, Level_Up_Coin: {11}",
                       ID, Type, Grade, Name, Level, Level_Max, Workload, Stamina, Merge_ID, Sale_Coin, Level_Up_Coin);
    }
}

public class AnimalTable : DataTable
{
    private static readonly AnimalData defaultData = new AnimalData();
    private Dictionary<int, AnimalData> table = new Dictionary<int, AnimalData>();

    public Dictionary<int, AnimalData> GetKeyValuePairs
    {
        get
        {
            return table;
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
                    table.Add(record.ID, record);
                }
            }
        };

        //var textAsset = Resources.Load<TextAsset>(path);

        //using (var reader = new StringReader(textAsset.text))
        //using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
        //{
        //    csvReader.Read();
        //    csvReader.ReadHeader();
        //    csvReader.Read();

        //    var records = csvReader.GetRecords<AnimalData>();
        //    foreach (var record in records)
        //    {
        //        table.Add(record.ID, record);
        //    }
        //}
    }

    public AnimalData Get(int id)
    {
        if (!table.ContainsKey(id))
            return defaultData;
        return table[id];
    }
}