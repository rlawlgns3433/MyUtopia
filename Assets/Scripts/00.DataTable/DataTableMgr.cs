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
        DataTable mergeTable = new MergeTable();
        mergeTable.Load(DataTableIds.Merge);
        tables.Add(DataTableIds.Merge, mergeTable);

        // ���̺� �Ϸ� �� �ּ� ����
        DataTable resourceTable = new ResourceTable();
        resourceTable.Load(DataTableIds.Resource);
        tables.Add(DataTableIds.Resource, resourceTable);

        // ���̺� �Ϸ� �� �ּ� ����
        DataTable furnitureTable = new FurnitureTable();
        furnitureTable.Load(DataTableIds.Furniture);
        tables.Add(DataTableIds.Furniture, furnitureTable);

        // ���̺� �Ϸ� �� �ּ� ����
        DataTable recipeTable = new RecipeTable();
        recipeTable.Load(DataTableIds.Recipe);
        tables.Add(DataTableIds.Recipe, recipeTable);

        // ���̺� �Ϸ� �� �ּ� ����
        DataTable itemTable = new ItemTable();
        itemTable.Load(DataTableIds.Item);
        tables.Add(DataTableIds.Item, itemTable);

        // ���̺� �Ϸ� �� �ּ� ����
        DataTable invitationTable = new InvitationTable();
        invitationTable.Load(DataTableIds.Invitation);
        tables.Add(DataTableIds.Invitation, invitationTable);
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
        return Get<MergeTable>(DataTableIds.Merge);
    }

    public static FloorTable GetFloorTable()
    {
        return Get<FloorTable>(DataTableIds.Floor);
    }
    public static FurnitureTable GetFurnitureTable()
    {
        return Get<FurnitureTable>(DataTableIds.Furniture);
    }

    public static BuildingTable GetBuildingTable()
    {
        return Get<BuildingTable>(DataTableIds.Building);
    }

    public static ResourceTable GetResourceTable()
    {
        return Get<ResourceTable>(DataTableIds.Resource);
    }

    public static RecipeTable GetRecipeTable()
    {
        return Get<RecipeTable>(DataTableIds.Recipe);
    }

    public static ItemTable GetItemTable()
    {
        return Get<ItemTable>(DataTableIds.Item);
    }

    public static InvitationTable GetInvitationTable()
    {
        return Get<InvitationTable>(DataTableIds.Invitation);
    }



    public static T Get<T>(string id) where T : DataTable
    {
        if (!tables.ContainsKey(id))
            return null;
        return tables[id] as T;
    }
}
