using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class StringTable : DataTable
{
    private class Data
    {
        public string String_ID { get; set; }
        public string Text { get; set; }
    }

    private Dictionary<string, string> table = new Dictionary<string, string>();

    public override bool IsLoaded { get; protected set; }

    public override void Load(string path)
    {
        path = string.Format(FormatPath, path);

        table.Clear();

        Addressables.LoadAssetAsync<TextAsset>(DataTableIds.CurrString).Completed += (AsyncOperationHandle<TextAsset> handle) =>
        {
            using (var reader = new StringReader(handle.Result.text))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csvReader.Read();
                csvReader.ReadHeader();
                csvReader.Read();

                var records = csvReader.GetRecords<Data>();
                foreach (var record in records)
                {
                    table.Add(record.String_ID, record.Text);
                }
            }
            IsLoaded = true;
        };
    }

    public string Get(string id)
    {
        if (!table.ContainsKey(id))
            return string.Empty;
        return table[id];
    }

}
