using System.Collections.Generic;

public static class DataTableMgr
{
    private static Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();

    static DataTableMgr()
    {

        DataTable stringTable = new StringTable();
        stringTable.Load(DataTableIds.String[(int)Vars.currentLang]);
        tables.Add(DataTableIds.String[(int)Vars.currentLang], stringTable);

        // ���̺� �Ϸ� �� �ּ� ����
        DataTable floorTable = new FloorTable();
        floorTable.Load(DataTableIds.Floor);
        tables.Add(DataTableIds.Floor, floorTable);

        // ���̺� �Ϸ� �� �ּ� ����
        DataTable animalTable = new AnimalTable();
        animalTable.Load(DataTableIds.Animal);
        tables.Add(DataTableIds.Animal, animalTable);

        // ���̺� �Ϸ� �� �ּ� ����
        DataTable buildingTable = new BuildingTable();
        buildingTable.Load(DataTableIds.Building);
        tables.Add(DataTableIds.Building, buildingTable);

        // ���̺� �Ϸ� �� �ּ� ����
        //DataTable facilityTable = new FacilityTable();
        //facilityTable.Load(DataTableIds.Facility);
        //tables.Add(DataTableIds.Facility, facilityTable);

        // ���̺� �Ϸ� �� �ּ� ����
        DataTable mergeTable = new MergeTable();
        mergeTable.Load(DataTableIds.MergeTable);
        tables.Add(DataTableIds.MergeTable, mergeTable);
    }

    public static StringTable GetStringTable()
    {
        return Get<StringTable>(DataTableIds.String[(int)Vars.currentLang]);
    }

    public static AnimalTable GetAnimalTable()
    {
        return Get<AnimalTable>(DataTableIds.Animal);
    }

    public static MergeTable GetMergeTable()
    {
        return Get<MergeTable>(DataTableIds.MergeTable);
    }

    public static FloorTable GetFloorTable()
    {
        return Get<FloorTable>(DataTableIds.Floor);
    }

    public static T Get<T>(string id) where T : DataTable
    {
        if (!tables.ContainsKey(id))
            return null;
        return tables[id] as T;
    }
}
