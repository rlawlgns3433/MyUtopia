using CsvHelper;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SynergyData
{
    public int Synergy_ID { get; set; }
    public string Synergy_Name_ID { get; set; }
    public string Synergy_Desc_ID { get; set; }
    public int World_Type { get; set; }
    public int Animal1_Type { get; set; }
    public int Animal2_Type { get; set; }
    public int Animal3_Type { get; set; }
    public int Animal4_Type { get; set; }
    public int Animal5_Type { get; set; }
    public int Animal1_Grade { get; set; }
    public int Animal2_Grade { get; set; }
    public int Animal3_Grade { get; set; }
    public int Animal4_Grade { get; set; }
    public int Animal5_Grade { get; set; }
    public int Synergy_Type { get; set; }
    public float Synergy_Value { get; set; }

    public string GetName()
    {
        return DataTableMgr.GetStringTable().Get(Synergy_Name_ID);
    }
    public string GetDesc()
    {
        return DataTableMgr.GetStringTable().Get(Synergy_Desc_ID);
    }
}

public class SynergyTable : DataTable
{
    private static readonly SynergyData defaultData = new SynergyData();
    private Dictionary<int, SynergyData> table = new Dictionary<int, SynergyData>();
    private Dictionary<int, List<Tuple<int, int>>> synergyAnimalDatas = new Dictionary<int, List<Tuple<int, int>>>();

    public override bool IsLoaded { get; protected set; }

    public Dictionary<int, SynergyData> GetKeyValuePairs
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

    public Dictionary<int, List<Tuple<int,int>>> GetAllSynergyAnimalData()
    {
        if(synergyAnimalDatas.Count == 0)
        {
            foreach (var data in table)
            {
                synergyAnimalDatas.Add(data.Key, new List<Tuple<int, int>>());
                if (data.Value.Animal1_Type != 0)
                    synergyAnimalDatas[data.Key].Add(new Tuple<int, int>(data.Value.Animal1_Type, data.Value.Animal1_Grade));
                if (data.Value.Animal2_Type != 0)
                    synergyAnimalDatas[data.Key].Add(new Tuple<int, int>(data.Value.Animal2_Type, data.Value.Animal2_Grade));
                if (data.Value.Animal3_Type != 0)
                    synergyAnimalDatas[data.Key].Add(new Tuple<int, int>(data.Value.Animal3_Type, data.Value.Animal3_Grade));
                if (data.Value.Animal4_Type != 0)
                    synergyAnimalDatas[data.Key].Add(new Tuple<int, int>(data.Value.Animal4_Type, data.Value.Animal4_Grade));
                if (data.Value.Animal5_Type != 0)
                    synergyAnimalDatas[data.Key].Add(new Tuple<int, int>(data.Value.Animal5_Type, data.Value.Animal5_Grade));
            }
        }
        return synergyAnimalDatas;
    }

    public override void Load(string path)
    {
        path = string.Format(FormatPath, path);

        table.Clear();

        Addressables.LoadAssetAsync<TextAsset>(DataTableIds.Synergy).Completed += (AsyncOperationHandle<TextAsset> handle) =>
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

                var records = csvReader.GetRecords<SynergyData>();
                foreach (var record in records)
                {
                    table.Add(record.Synergy_ID, record);
                }
            }

            IsLoaded = true;
        };
    }

    public SynergyData Get(int id)
    {
        if (!table.ContainsKey(id))
            return defaultData;
        return table[id];
    }
}