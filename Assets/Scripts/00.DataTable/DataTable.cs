public abstract class DataTable
{
    public static readonly string FormatPath = "DataTables/{0}";
    public abstract bool IsLoaded { get; protected set; }
    public abstract void Load(string path);
}
