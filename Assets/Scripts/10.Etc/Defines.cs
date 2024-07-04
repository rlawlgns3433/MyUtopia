public static class DataTableIds
{
    public static readonly string[] String =
    {
        "StringTableKr",
        "StringTableEn",
        "StringTableJp"
    };

    public static readonly string Floor = "FloorTable";
    public static readonly string Animal = "AnimalTable";

    public static string CurrString
    {
        get
        {
            return String[(int)Vars.currentLang];
        }
    }
}

public static class Vars
{
    public static readonly string Version = "1.0.0";
    public static readonly int BuildVersion = 1;

    public static Languages currentLang = Languages.Korean;

    public static Languages editorLang = Languages.Korean;
}

public static class Tags
{
    public static readonly string Enemy = "Enemy";
}

public static class SortingLayers
{
    public static readonly string Default = "Default";
}

public static class Layers
{
    public static readonly string UI = "UI";
}

public enum AnimalType
{
    OnFoot,
    Wing
}

public enum Languages
{
    Korean,
    English,
    Japanese,
}

public enum TableIdentifier
{
    Animal = 1, // Identifier(1) + Type(D2) + Grade(D2) + CurrentLevel(D3)
    Layer = 2,
}

public enum SceneIds
{
    None = -1,
    Start,
    Game,
}