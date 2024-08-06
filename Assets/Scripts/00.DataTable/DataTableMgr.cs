using System.Collections.Generic;

public static class DataTableMgr
{
    private static Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();

    static DataTableMgr()
    {
        DataTable stringTable = new StringTable();
        stringTable.Load(DataTableIds.String[(int)Vars.currentLang]);
        tables.Add(DataTableIds.String[(int)Vars.currentLang], stringTable);

        DataTable floorTable = new FloorTable();
        floorTable.Load(DataTableIds.Floor);
        tables.Add(DataTableIds.Floor, floorTable);

        DataTable animalTable = new AnimalTable();
        animalTable.Load(DataTableIds.Animal);
        tables.Add(DataTableIds.Animal, animalTable);

        DataTable buildingTable = new BuildingTable();
        buildingTable.Load(DataTableIds.Building);
        tables.Add(DataTableIds.Building, buildingTable);

        DataTable mergeTable = new MergeTable();
        mergeTable.Load(DataTableIds.Merge);
        tables.Add(DataTableIds.Merge, mergeTable);

        DataTable resourceTable = new ResourceTable();
        resourceTable.Load(DataTableIds.Resource);
        tables.Add(DataTableIds.Resource, resourceTable);

        DataTable furnitureTable = new FurnitureTable();
        furnitureTable.Load(DataTableIds.Furniture);
        tables.Add(DataTableIds.Furniture, furnitureTable);

        DataTable recipeTable = new RecipeTable();
        recipeTable.Load(DataTableIds.Recipe);
        tables.Add(DataTableIds.Recipe, recipeTable);

        DataTable itemTable = new ItemTable();
        itemTable.Load(DataTableIds.Item);
        tables.Add(DataTableIds.Item, itemTable);

        DataTable invitationTable = new InvitationTable();
        invitationTable.Load(DataTableIds.Invitation);
        tables.Add(DataTableIds.Invitation, invitationTable);

        // 테이블 완료 시 주석 해제
        //DataTable worldTable = new WorldTable();
        //worldTable.Load(DataTableIds.World);
        //tables.Add(DataTableIds.World, worldTable);

        DataTable synergyTable = new SynergyTable();
        synergyTable.Load(DataTableIds.Synergy);
        tables.Add(DataTableIds.Synergy, synergyTable);

        //테이블 완료 시 주석 해제
        DataTable exchangeTable = new ExchangeTable();
        exchangeTable.Load(DataTableIds.Exchange);
        tables.Add(DataTableIds.Exchange, exchangeTable);

        DataTable missionTable = new MissionTable();
        missionTable.Load(DataTableIds.Mission);
        tables.Add(DataTableIds.Mission, missionTable);

        DataTable rewardTable = new RewardTable();
        rewardTable.Load(DataTableIds.Reward);
        tables.Add(DataTableIds.Reward, rewardTable);
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
    public static MissionTable GetMissionTable()
    {
        return Get<MissionTable>(DataTableIds.Mission);
    }

    //public static WorldTable GetWorldTable()
    //{
    //    return Get<WorldTable>(DataTableIds.World);
    //}

    public static SynergyTable GetSynergyTable()
    {
        return Get<SynergyTable>(DataTableIds.Synergy);
    }    
    
    public static ExchangeTable GetExchangeTable()
    {
        return Get<ExchangeTable>(DataTableIds.Exchange);
    }

    public static RewardTable GetRewardTable()
    {
        return Get<RewardTable>(DataTableIds.Reward);
    }

    public static T Get<T>(string id) where T : DataTable
    {
        if (!tables.ContainsKey(id))
            return null;
        return tables[id] as T;
    }
}
