using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public struct AnimalData
{
    public int AnimalId { get; set; }
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

        var textAsset = Resources.Load<TextAsset>(path);

        using (var reader = new StringReader(textAsset.text))
        using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            csvReader.Read();
            csvReader.ReadHeader();
            csvReader.Read();

            var records = csvReader.GetRecords<AnimalData>();
            foreach (var record in records)
            {
                table.Add(record.AnimalId, record);
            }
        }
    }

    public AnimalData Get(int id)
    {
        if (!table.ContainsKey(id))
            return defaultData;
        return table[id];
    }
}
