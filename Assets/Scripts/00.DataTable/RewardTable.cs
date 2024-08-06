using CsvHelper;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class RewardData
{
    public int Reward_ID { get; set; }
    public int World_Type { get; set; }
    public int Reward1_Type { get; set; }
    public int Reward1_ID { get; set; }
    public string Reward1_Value { get; set; }
    public int Reward2_Type { get; set; }
    public int Reward2_ID { get; set; }
    public string Reward2_Value { get; set; }
    public int Reward3_Type { get; set; }
    public int Reward3_ID { get; set; }
    public string Reward3_Value { get; set; }
}

public class RewardTable : DataTable
{
    public static readonly RewardData defaultData = new RewardData();
    private Dictionary<int, RewardData> table = new Dictionary<int, RewardData>();
    public override bool IsLoaded { get; protected set; }

    public override void Load(string path)
    {
        path = string.Format(FormatPath, path);

        table.Clear();

        Addressables.LoadAssetAsync<TextAsset>(DataTableIds.Reward).Completed += (AsyncOperationHandle<TextAsset> handle) =>
        {
            using (var reader = new StringReader(handle.Result.text))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csvReader.Read();
                csvReader.ReadHeader();
                csvReader.Read();

                var records = csvReader.GetRecords<RewardData>();
                foreach (var record in records)
                {
                    table.Add(record.Reward_ID, record);
                }
            }
            IsLoaded = true;
        };
    }

    public RewardData Get(int id)
    {
        if (!table.ContainsKey(id))
            return defaultData;
        return table[id];
    }
}
