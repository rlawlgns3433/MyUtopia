using System.Collections.Generic;

public static class DataTableMgr
{
    private static Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();

    static DataTableMgr()
    {

        DataTable table = new StringTable();
        table.Load(DataTableIds.String[(int)Vars.currentLang]);
        tables.Add(DataTableIds.String[(int)Vars.currentLang], table);
    }

    public static StringTable GetStringTable()
    {
        return Get<StringTable>(DataTableIds.String[(int)Vars.currentLang]);
    }

    public static T Get<T>(string id) where T : DataTable
    {
        if (!tables.ContainsKey(id))
            return null;
        return tables[id] as T;
    }
}
