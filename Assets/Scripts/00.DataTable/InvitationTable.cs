using CsvHelper;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class InvitationData
{
    public int Invite_ID { get; set; }
    public string Invite_Name { get; set; } // 개발 확인용
    public int Floor_Grade { get; set; }
    public string World_Type { get; set; }
    public int Level_Up_Coin_ID { get; set; }
    public int Level_Up_Coin_Value { get; set; }
    public int Get_Animal_ID_1 { get; set; }
    public float Get_Animal1_Rate { get; set; }
    public int Get_Animal_ID_2 { get; set; }
    public float Get_Animal2_Rate { get; set; }
    public int Get_Animal_ID_3 { get; set; }
    public float Get_Animal3_Rate { get; set; }
    public int Get_Animal_ID_4 { get; set; }
    public float Get_Animal4_Rate { get; set; }
    public int Get_Animal_ID_5 { get; set; }
    public float Get_Animal5_Rate { get; set; }
    public int Get_Animal_ID_6 { get; set; }
    public float Get_Animal6_Rate { get; set; }
}

public class InvitationTable : DataTable
{
    private static readonly InvitationData defaultData = new InvitationData();
    private Dictionary<int, InvitationData> table = new Dictionary<int, InvitationData>();
    public override bool IsLoaded { get; protected set; }

    public Dictionary<int, InvitationData> GetKeyValuePairs
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

        Addressables.LoadAssetAsync<TextAsset>(DataTableIds.Invitation).Completed += (AsyncOperationHandle<TextAsset> handle) =>
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

                var records = csvReader.GetRecords<InvitationData>();
                foreach (var record in records)
                {
                    table.Add(record.Invite_ID, record);
                }
            }

            IsLoaded = true;
        };
    }

    public InvitationData Get(int id)
    {
        if (!table.ContainsKey(id))
            return defaultData;
        return table[id];
    }
}