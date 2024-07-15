public class AnimalStat
{
    private AnimalData animalData;
    public AnimalData AnimalData
    {
        get
        {
            if(animalData == null)
            {
                animalData = new AnimalData();
            }
            return animalData;
        }
        set
        {
            animalData = value;
            Animal_ID = AnimalData.Animal_ID;
            Level = AnimalData.Level;
            Level_Max = AnimalData.Level_Max;
            Workload = AnimalData.Workload;
            Stamina = AnimalData.Stamina;
            Merge_ID = AnimalData.Merge_ID;
            Sale_Coin = AnimalData.Sale_Coin;
            Level_Up_Coin_ID = AnimalData.Level_Up_Coin_ID;
            Level_Up_Coin_Value = AnimalData.Level_Up_Coin_Value;
            Prefab = AnimalData.Prefab;
            Profile = AnimalData.Profile;
        }
    }
    public int Animal_ID { get; set; }
    public int Level { get; set; }
    public int Level_Max { get; set; }
    public int Workload { get; set; }
    public float Stamina { get; set; }
    public int Merge_ID { get; set; }
    public string Sale_Coin { get; set; }
    public string Level_Up_Coin_ID { get; set; }
    public string Level_Up_Coin_Value { get; set; }
    public string Prefab { get; set; }
    public string Profile { get; set; }
    public string CurrentFloor { get; set; }

    public AnimalStat() { }
    
    public AnimalStat(int aniamlId) 
    {
        this.AnimalData = DataTableMgr.GetAnimalTable().Get(aniamlId);
    }
}

public class FurnitureStat
{
    private FunitureData furnitureData;
    public FunitureData FurnitureData
    {
        get
        {
            if (furnitureData == null)
            {
                furnitureData = new FunitureData();
            }
            return furnitureData;
        }
        set
        {
            furnitureData = value;
            Furniture_ID = FurnitureData.Furniture_ID;
            Furniture_Name = FurnitureData.Furniture_Name_ID;
            Level = FurnitureData.Level;
            Level_Max = FurnitureData.Level_Max;
            Effect_Type = FurnitureData.Effect_Type;
            Effect_Value = FurnitureData.Effect_Value;
            Level_Up_Coin_ID = FurnitureData.Level_Up_Coin_ID;
            Level_Up_Coin_Value = FurnitureData.Level_Up_Coin_Value;
            Prefab = FurnitureData.Prefab;
        }
    }
    public int Furniture_ID { get; set; }
    public string Furniture_Name { get; set; }
    public int Level { get; set; }
    public int Level_Max { get; set; }
    public int Effect_Type { get; set; }
    public int Effect_Value { get; set; }
    public int Level_Up_Coin_ID { get; set; }
    public string Level_Up_Coin_Value { get; set; }
    public string Prefab { get; set; }

    public FurnitureStat() { }

    public FurnitureStat(int furnitureId)
    {
        this.FurnitureData = DataTableMgr.GetFurnitureTable().Get(furnitureId);
    }
}

public class FloorStat
{
    private FloorData floorData;
    public FloorData FloorData
    {
        get
        {
            if (floorData == null)
            {
                floorData = new FloorData();
            }
            return floorData;
        }
        set
        {
            floorData = value;
            Floor_ID = floorData.Floor_ID;
            World_Type = floorData.World_Type;
            Floor_Num = floorData.Floor_Num;
            Floor_Type = floorData.Floor_Type;
            Floor_Name_ID = floorData.Floor_Name_ID;
            Grade = floorData.Grade;
            Grade_Max = floorData.Grade_Max;
            Unlock_Facility = floorData.Unlock_Facility;
            Unlock_Content = floorData.Unlock_Content;
            Max_Population = floorData.Max_Population;
            Level_Up_Coin_ID = floorData.Level_Up_Coin_ID;
            Level_Up_Coin_Value = floorData.Level_Up_Coin_Value;
            Level_Up_Resource_1 = floorData.Level_Up_Resource_1;
            Resource_1_Value = floorData.Resource_1_Value;
            Level_Up_Resource_2 = floorData.Level_Up_Resource_2;
            Resource_2_Value = floorData.Resource_2_Value;
            Level_Up_Resource_3 = floorData.Level_Up_Resource_3;
            Resource_3_Value = floorData.Resource_3_Value;
        }
    }
    public int Floor_ID { get; set; }
    public int World_Type { get; set; }
    public int Floor_Num { get; set; }
    public int Floor_Type { get; set; }
    public string Floor_Name_ID { get; set; }
    public int Grade { get; set; }
    public int Grade_Max { get; set; }
    public int Unlock_Facility { get; set; }
    public int Unlock_Content { get; set; }
    public int Max_Population { get; set; }
    public int Level_Up_Coin_ID { get; set; }
    public string Level_Up_Coin_Value { get; set; }
    public int Level_Up_Resource_1 { get; set; }
    public string Resource_1_Value { get; set; }
    public int Level_Up_Resource_2 { get; set; }
    public string Resource_2_Value { get; set; }
    public int Level_Up_Resource_3 { get; set; }
    public string Resource_3_Value { get; set; }

    public FloorStat() { }

    public FloorStat(int floorId)
    {
        this.FloorData = DataTableMgr.GetFloorTable().Get(floorId);
    }
}

