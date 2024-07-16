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

public class RecipeStat
{
    private RecipeData recipeData;
    public RecipeData RecipeData
    {
        get
        {
            if (recipeData == null)
            {
                recipeData = new RecipeData();
            }
            return recipeData;
        }
        set
        {
            RecipeData = value;
            Recipe_ID = RecipeData.Recipe_ID;
            Recipe_Name_ID = RecipeData.Recipe_Name_ID;
            Unlock_Lv = RecipeData.Unlock_Lv;
            Workload = RecipeData.Workload;
            Resource_1 = RecipeData.Resource_1;
            Resource_1_Value = RecipeData.Resource_1_Value;
            Resource_2 = RecipeData.Resource_2;
            Resource_2_Value = RecipeData.Resource_2_Value;
            Resource_3 = RecipeData.Resource_3;
            Resource_3_Value = RecipeData.Resource_3_Value;
            Workload = RecipeData.Workload;
            Product_ID = RecipeData.Product_ID;
        }
    }
    public int Recipe_ID { get; set; }
    public string Recipe_Name_ID { get; set; }
    public int Unlock_Lv { get; set; }
    public int Resource_1 { get; set; }
    public string Resource_1_Value { get; set; }
    public int Resource_2 { get; set; }
    public string Resource_2_Value { get; set; }
    public int Resource_3 { get; set; }
    public string Resource_3_Value { get; set; }
    public int Workload { get; set; }
    public int Product_ID { get; set; }
    public RecipeStat() { }

    public RecipeStat(int recipeId)
    {
        this.RecipeData = DataTableMgr.GetRecipeTable().Get(recipeId);
    }
}


public class ItemStat
{
    private ItemData itemData;
    public ItemData ItemData
    {
        get
        {
            if (itemData == null)
            {
                itemData = new ItemData();
            }
            return itemData;
        }
        set
        {
            ItemData = value;
            Item_ID = ItemData.Item_ID;
            Item_Name_ID = ItemData.Item_Name_ID;
            Item_Desc_ID = ItemData.Item_Desc_ID;
            Item_Type = ItemData.Item_Type;
            World_Type = ItemData.World_Type;
            Buy_Resource_ID = ItemData.Buy_Resource_ID;
            Buy_Price = ItemData.Buy_Price;
            Sell_Resource_ID = ItemData.Sell_Resource_ID;
            Sell_Price = ItemData.Sell_Price;
            Effect_ID = ItemData.Effect_ID;
            Image = ItemData.Image;
        }
    }
    public int Item_ID { get; set; }
    public string Item_Name_ID { get; set; }
    public string Item_Desc_ID { get; set; }
    public int Item_Type { get; set; }
    public int World_Type { get; set; }
    public int Buy_Resource_ID { get; set; }
    public string Buy_Price { get; set; }
    public int Sell_Resource_ID { get; set; }
    public string Sell_Price { get; set; }
    public int Effect_ID { get; set; }
    public string Image { get; set; }
    public ItemStat() { }

    public ItemStat(int itemId)
    {
        this.ItemData = DataTableMgr.GetItemTable().Get(itemId);
    }
}