using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public struct MergeData
{
    public int Merge_ID { get; set; }
    public string Merge_Name { get; set; }
    public int Animal1_ID { get; set; }
    public int Animal2_ID { get; set; }
    public int Result_Animal { get; set; }

    public string GetName()
    {
        return DataTableMgr.GetStringTable().Get(Merge_Name);
    }
}

public class MergeTable : DataTable
{
    public static readonly MergeData defaultData = new MergeData();
    private Dictionary<int, MergeData> table = new Dictionary<int, MergeData>();
    public override bool IsLoaded { get; protected set; }

    public override void Load(string path)
    {
        path = string.Format(FormatPath, path);

        table.Clear();

        Addressables.LoadAssetAsync<TextAsset>(DataTableIds.Merge).Completed += (AsyncOperationHandle<TextAsset> handle) =>
        {
            using (var reader = new StringReader(handle.Result.text))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csvReader.Read();
                csvReader.ReadHeader();
                csvReader.Read();

                var records = csvReader.GetRecords<MergeData>();
                foreach (var record in records)
                {
                    table.Add(record.Merge_ID, record);
                }
            }
            IsLoaded = true;
        };
    }

    public MergeData Get(int id)
    {
        if (!table.ContainsKey(id))
            return defaultData;
        return table[id];
    }
}
