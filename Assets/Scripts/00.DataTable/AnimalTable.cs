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
    public float Rate { get; set; }
    public int Merge_ID { get; set; }
    public string Sale_Coin { get; set; }
    public string Level_Up_Coin { get; set; }
    // 데이터 테이블에 따른 추가 필요
}

public class AnimalTable : DataTable
{
    private static readonly AnimalData defaultData = new AnimalData();
    private Dictionary<int, AnimalData> table = new Dictionary<int, AnimalData>();

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