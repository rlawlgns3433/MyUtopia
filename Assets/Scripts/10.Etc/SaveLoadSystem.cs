using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public static class SaveLoadSystem 
{
    public enum Mode
    {
        Json,
        Binary,
        EncryptedBinary,
    }

    public static Mode FileMode { get; set; } = Mode.Json;

    public static int SaveDataVersion { get; private set; } = 1;

    // 0 (ÀÚµ¿), 1, 2, 3 ...
    private static readonly string[] SaveFileName =
    {
        "SaveWorld.sav",
        "SaveCurrency.sav",
        "SaveEmpty.sav",
        "Save3.sav"
    };

    private static string SaveDirectory
    {
        get
        {
            return $"{Application.persistentDataPath}/Save";
        }
    }

    public static bool Save(SaveData data, int slot = 0)
    {
        if (slot < 0 ||  slot >= SaveFileName.Length)
        {
            return false;
        }

        if (!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
        }

        var path = Path.Combine(SaveDirectory, SaveFileName[slot]);

        using (var writer = new JsonTextWriter(new StreamWriter(path)))
        {
            var serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            serializer.TypeNameHandling = TypeNameHandling.All;
            serializer.Converters.Add(new WorldConverter());
            serializer.Converters.Add(new CurrencyConverter());
            serializer.Serialize(writer, data);
        }

        return true;
    }

    public static SaveData Load(int slot = 0)
    {
        if (slot < 0 ||  slot >= SaveFileName.Length)
        {
            return null;
        }
        var path = Path.Combine(SaveDirectory, SaveFileName[slot]);
        if (!File.Exists(path))
        {
            return null;
        }

        SaveData data = null;
        using (var reader = new JsonTextReader(new StreamReader(path)))
        {
            var serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            serializer.TypeNameHandling = TypeNameHandling.All;
            serializer.Converters.Add(new WorldConverter());
            serializer.Converters.Add(new CurrencyConverter());

            data = serializer.Deserialize<SaveData>(reader);
        }

        if(data == null)
        {
            return null;
        }

        while (data.Version < SaveDataVersion)
        {
            data = data.VersionUp();
        }

        return data;
    }
    public static void Delete(int slot)
    {
        if (slot < 0 ||  slot >= SaveFileName.Length)
        {
            return;
        }
        var path = Path.Combine(SaveDirectory, SaveFileName[slot]);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}