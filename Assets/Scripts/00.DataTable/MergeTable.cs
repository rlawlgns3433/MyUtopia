using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public struct MergeData
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int Animal1_ID { get; set; }
    public int Animal2_ID { get; set; }
    public int Result_Animal { get; set; }

    public string GetName()
    {
        return DataTableMgr.GetStringTable().Get(Name);
    }
}

public class MergeTable : DataTable
{
    public static readonly MergeData defaultData = new MergeData();
    private Dictionary<int, MergeData> table = new Dictionary<int, MergeData>();

    public override void Load(string path)
    {
        path = string.Format(FormatPath, path);

        table.Clear();

        Addressables.LoadAssetAsync<TextAsset>(DataTableIds.MergeTable).Completed += (AsyncOperationHandle<TextAsset> handle) =>
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
                    table.Add(record.ID, record);
                }
            }
        };
    }

    public MergeData Get(int id)
    {
        if (!table.ContainsKey(id))
            return defaultData;
        return table[id];
    }
}
