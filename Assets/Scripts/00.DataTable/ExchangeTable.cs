using CsvHelper;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ExchangeData
{
    public int Exchange_ID { get; set; }
    public int World_Type { get; set; }
    public int Exchange_Num { get; set; }
    public int Furniture_Type { get; set; }
    public int Exchange_Level { get; set; }
    public int Require_Resource1_Type { get; set; }
    public int Require_Resource1_ID { get; set; }
    public string Require_Resource1_Value { get; set; }
    public int Require_Resource2_Type { get; set; }
    public int Require_Resource2_ID { get; set; }
    public string Require_Resource2_Value { get; set; }
    public int Require_Resource3_Type { get; set; }
    public int Require_Resource3_ID { get; set; }
    public string Require_Resource3_Value { get; set; }
    public int Reward_ID { get; set; }
}

public class ExchangeTable : DataTable
{
    private static readonly ExchangeData defaultData = new ExchangeData();
    private Dictionary<int, ExchangeData> table = new Dictionary<int, ExchangeData>();
    public override bool IsLoaded { get; protected set; }

    public Dictionary<int, ExchangeData> GetKeyValuePairs
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

        Addressables.LoadAssetAsync<TextAsset>(DataTableIds.Exchange).Completed += (AsyncOperationHandle<TextAsset> handle) =>
        {
            if (handle.Result == null)
            {
                return;
            }

            using (var reader = new StringReader(handle.Result.text))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csvReader.Read();
                csvReader.ReadHeader();
                csvReader.Read();

                var records = csvReader.GetRecords<ExchangeData>();
                foreach (var record in records)
                {
                    table.Add(record.Exchange_ID, record);
                }
            }

            IsLoaded = true;
        };
    }

    public ExchangeData Get(int id)
    {
        if (!table.ContainsKey(id))
            return defaultData;
        return table[id];
    }
}