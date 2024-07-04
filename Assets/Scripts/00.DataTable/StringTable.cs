using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class StringTable : DataTable
{
    private class Data
    {
        public string STRING_ID { get; set; }
        public string STRING { get; set; }
    }

    private Dictionary<string, string> table = new Dictionary<string, string>();

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

            var records = csvReader.GetRecords<Data>();
            foreach (var record in records)
            {
                table.Add(record.STRING_ID, record.STRING);
            }
        }
    }

    public string Get(string id)
    {
        if (!table.ContainsKey(id))
            return string.Empty;
        return table[id];
    }

}
