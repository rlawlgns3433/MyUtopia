using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.Playables;

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

    public enum SaveType
    {
        World,
        Currency,
        EmptyWorld,
        EmptyCurrency,
        EmptyProduct,
        PlayingWorld,
        PlayingCurrency,
        EmptyCurrencyProduct,
        Product,
        Missions,
        CurrencyProduct,
        Catalouge,
        EmptyMission
    }

    // 0 (ÀÚµ¿), 1, 2, 3 ...
    public static readonly string[] SaveFileName =
    {
        "SaveWorld.sav",
        "SaveCurrency.sav",
        "SaveEmptyWorld.sav",
        "SaveEmptyCurrency.sav",
        "SaveEmptyProduct.sav",
        "SavePlayingWorld.sav",
        "SavePlayingCurrency.sav",
        "SaveEmptyCurrencyProduct.sav",
        "SaveProduct.sav",
        "SaveMissions.sav",
        "SaveCurrencyProduct.sav",
        "SaveCatalogue.sav",
        "SaveEmptyMission.sav"
    };

    private static string SaveDirectory
    {
        get
        {
            return $"{Application.persistentDataPath}/Save";
        }
    }

    public static bool Save(SaveData data, SaveType slot = 0)
    {
        if (slot < 0 ||  (int)slot >= SaveFileName.Length)
        {
            return false;
        }

        if (!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
        }

        var path = Path.Combine(SaveDirectory, SaveFileName[(int)slot]);

        using (var writer = new JsonTextWriter(new StreamWriter(path)))
        {
            var serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            serializer.TypeNameHandling = TypeNameHandling.All;
            serializer.Converters.Add(new WorldConverter());
            serializer.Converters.Add(new CurrencyConverter());
            serializer.Converters.Add(new CurrencyProductConverter());
            serializer.Serialize(writer, data);
        }

        return true;
    }

    public static SaveData Load(SaveType slot = 0)
    {
        if (slot < 0 ||  (int)slot >= SaveFileName.Length)
        {
            return null;
        }
        var path = Path.Combine(SaveDirectory, SaveFileName[(int)slot]);
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
            serializer.Converters.Add(new CurrencyProductConverter());

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

    public static bool Save(SaveMissionData data, SaveType slot = SaveType.Missions)
    {
        if (slot < 0 || (int)slot >= SaveFileName.Length)
        {
            return false;
        }

        if (!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
        }

        var path = Path.Combine(SaveDirectory, SaveFileName[(int)slot]);

        using (var writer = new JsonTextWriter(new StreamWriter(path)))
        {
            var serializer = new JsonSerializer
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.All
            };
            serializer.Converters.Add(new MissionDataConverter());
            serializer.Serialize(writer, data);
        }
        Debug.Log("Mission Save Succeed");

        return true;
    }

    public static SaveMissionData MissionLoad(SaveType slot = SaveType.Missions)
    {
        if (slot < 0 || (int)slot >= SaveFileName.Length)
        {
            return default(SaveMissionData);
        }
        var path = Path.Combine(SaveDirectory, SaveFileName[(int)slot]);
        if (!File.Exists(path))
        {
            return default(SaveMissionData);
        }

        using (var reader = new JsonTextReader(new StreamReader(path)))
        {
            var serializer = new JsonSerializer
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.All
            };
            serializer.Converters.Add(new MissionDataConverter());

            return serializer.Deserialize<SaveMissionData>(reader);
        }
    }

    public static SaveMissionData EmptyMissionLoad(SaveType slot = SaveType.EmptyMission)
    {
        if (slot < 0 || (int)slot >= SaveFileName.Length)
        {
            return default(SaveMissionData);
        }
        var path = Path.Combine(SaveDirectory, SaveFileName[(int)slot]);
        if (!File.Exists(path))
        {
            return default(SaveMissionData);
        }

        using (var reader = new JsonTextReader(new StreamReader(path)))
        {
            var serializer = new JsonSerializer
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.All
            };
            serializer.Converters.Add(new MissionDataConverter());

            return serializer.Deserialize<SaveMissionData>(reader);
        }
    }

    public static bool Save(SaveCatalogueData data, SaveType slot = SaveType.Catalouge)
    {
        if (slot < 0 || (int)slot >= SaveFileName.Length)
        {
            return false;
        }

        if (!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
        }

        var path = Path.Combine(SaveDirectory, SaveFileName[(int)slot]);

        using (var writer = new JsonTextWriter(new StreamWriter(path)))
        {
            var serializer = new JsonSerializer
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.All
            };
            serializer.Converters.Add(new CatalougeDataConverter());
            serializer.Serialize(writer, data);
        }
        return true;
    }

    public static SaveCatalogueData CatalougeLoad(SaveType slot = SaveType.Catalouge)
    {
        if (slot < 0 || (int)slot >= SaveFileName.Length)
        {
            return default(SaveCatalogueData);
        }
        var path = Path.Combine(SaveDirectory, SaveFileName[(int)slot]);
        if (!File.Exists(path))
        {
            return default(SaveCatalogueData);
        }

        using (var reader = new JsonTextReader(new StreamReader(path)))
        {
            var serializer = new JsonSerializer
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.All
            };
            serializer.Converters.Add(new CatalougeDataConverter());

            return serializer.Deserialize<SaveCatalogueData>(reader);
        }
    }
}