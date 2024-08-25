using System;
using System.Collections.Generic;
using System.Diagnostics;
using static ExchangeStat;

[Serializable]
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
    public string Workload { get; set; }
    public float Stamina { get; set; }
    public int Merge_ID { get; set; }
    public string Sale_Coin { get; set; }
    public int Level_Up_Coin_ID { get; set; }
    public string Level_Up_Coin_Value { get; set; }
    public string Prefab { get; set; }
    public string Profile { get; set; }
    public string CurrentFloor { get; set; }
    public float AcquireTime { get; set; }

    public AnimalStat() { }
    
    public AnimalStat(int aniamlId) 
    {
        this.AnimalData = DataTableMgr.GetAnimalTable().Get(aniamlId);
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
            Floor_Desc = floorData.Floor_Desc;
            Grade = floorData.Grade;
            Grade_Max = floorData.Grade_Max;
            Grade_Level_Max = floorData.Grade_Level_Max;
            Level_Up_Time = floorData.Level_Up_Time;
            Unlock_Facility = floorData.Unlock_Facility;
            Unlock_Content = floorData.Unlock_Content;
            Max_Population = floorData.Max_Population;
            Stamina_Recovery = floorData.Stamina_Recovery;
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
    public string Floor_Desc { get; set; }
    public int Grade { get; set; }
    public int Grade_Max { get; set; }
    public int Grade_Level_Max { get; set; }
    public int Level_Up_Time { get; set; }
    public int Unlock_Facility { get; set; }
    public int Unlock_Content { get; set; }
    public int Max_Population { get; set; }
    public int Stamina_Recovery { get; set; }
    public int Level_Up_Coin_ID { get; set; }
    public string Level_Up_Coin_Value { get; set; }
    public int Level_Up_Resource_1 { get; set; }
    public string Resource_1_Value { get; set; }
    public int Level_Up_Resource_2 { get; set; }
    public string Resource_2_Value { get; set; }
    public int Level_Up_Resource_3 { get; set; }
    public string Resource_3_Value { get; set; }
    public bool IsLock { get; set; }
    public bool IsUpgrading { get; set; } = false;
    public double UpgradeTimeLeft { get; set; }
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
            recipeData = value;
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

    public Dictionary<int, BigNumber> Resources
    {
        get
        {
            Dictionary<int, BigNumber> resources = new Dictionary<int, BigNumber>();
            if (Resource_1 != 0)
                resources[Resource_1] = Resource_1_Value.ToBigNumber();
            if (Resource_2 != 0)
                resources[Resource_2] = Resource_2_Value.ToBigNumber();
            if(Resource_3 != 0)
                resources[Resource_3] = Resource_3_Value.ToBigNumber();

            return resources;
        }
    }


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
            itemData = value;
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

public class BuildingStat
{
    private BuildingData buildingData;
    public BuildingData BuildingData
    {
        get
        {
            if (buildingData == null)
            {
                buildingData = new BuildingData();
            }
            return buildingData;
        }
        set
        {
            buildingData = value;
            Building_ID = BuildingData.Building_ID;
            Floor_Type = BuildingData.Floor_Type;
            Building_Name_ID = BuildingData.Building_Name_ID;
            Level = BuildingData.Level;
            Level_Max = BuildingData.Level_Max;
            Materials_Type = BuildingData.Materials_Type;
            Conversion_rate = BuildingData.Conversion_rate;
            Work_Require = BuildingData.Work_Require;
            Effect_Type = BuildingData.Effect_Type;
            Effect_Value = BuildingData.Effect_Value;
            Resource_Type = BuildingData.Resource_Type;
            Touch_Produce = BuildingData.Touch_Produce;
            Level_Up_Time = BuildingData.Level_Up_Time;
            Level_Up_Resource_ID = BuildingData.Level_Up_Resource_ID;
            Level_Up_Coin_Value = BuildingData.Level_Up_Coin_Value;
            Level_Up_Resource_1 = BuildingData.Level_Up_Resource_1;
            Resource_1_Value = BuildingData.Resource_1_Value;
            Level_Up_Resource_2 = BuildingData.Level_Up_Resource_2;
            Resource_2_Value = BuildingData.Resource_2_Value;
            Level_Up_Resource_3 = BuildingData.Level_Up_Resource_3;
            Resource_3_Value = BuildingData.Resource_3_Value;
            Prefab = BuildingData.Prefab;
        }
    }
    public int Building_ID { get; set; }
    public int Floor_Type { get; set; }
    public string Building_Name_ID { get; set; }
    public int Level { get; set; }
    public int Level_Max { get; set; }
    public int Materials_Type { get; set; }
    public int Conversion_rate { get; set; }
    public int Work_Require { get; set; }
    public int Effect_Type { get; set; }
    public int Effect_Value { get; set; }
    public int Resource_Type { get; set; } // 생산 재화 타입
    public string Touch_Produce { get; set; }
    public int Level_Up_Time { get; set; }
    public string Level_Up_Resource_ID { get; set; }
    public string Level_Up_Coin_Value { get; set; }
    public int Level_Up_Resource_1 { get; set; }
    public string Resource_1_Value { get; set; }
    public int Level_Up_Resource_2 { get; set; }
    public string Resource_2_Value { get; set; }
    public int Level_Up_Resource_3 { get; set; }
    public string Resource_3_Value { get; set; }
    public string Prefab { get; set; }
    public bool IsLock { get; set; } = true;
    public bool IsUpgrading { get; set; } = false;
    public double UpgradeTimeLeft { get; set; }

    public BuildingStat() { }

    public BuildingStat(int buildingId)
    {
        this.BuildingData = DataTableMgr.GetBuildingTable().Get(buildingId);
    }
}


public class SynergyStat
{
    private SynergyData synergyData;
    public SynergyData SynergyData
    {
        get
        {
            if (synergyData == null)
            {
                synergyData = new SynergyData();
            }
            return synergyData;
        }
        set
        {
            synergyData = value;
            {
                synergyData = value;
                Synergy_ID = synergyData.Synergy_ID;
                Synergy_Name_ID = synergyData.Synergy_Name_ID;
                Synergy_Desc_ID = synergyData.Synergy_Desc_ID;
                World_Type = synergyData.World_Type;
                Animal1_Type = synergyData.Animal1_Type;
                Animal2_Type = synergyData.Animal2_Type;
                Animal3_Type = synergyData.Animal3_Type;
                Animal4_Type = synergyData.Animal4_Type;
                Animal5_Type = synergyData.Animal5_Type;
                Animal1_Grade = synergyData.Animal1_Grade;
                Animal2_Grade = synergyData.Animal2_Grade;
                Animal3_Grade = synergyData.Animal3_Grade;
                Animal4_Grade = synergyData.Animal4_Grade;
                Animal5_Grade = synergyData.Animal5_Grade;
                Synergy_Type = synergyData.Synergy_Type;
                Synergy_Value = synergyData.Synergy_Value;
            }
        }
    }
    public int Synergy_ID { get; set; }
    public string Synergy_Name_ID { get; set; }
    public string Synergy_Desc_ID { get; set; }
    public int World_Type { get; set; }
    public int Animal1_Type { get; set; }
    public int Animal2_Type { get; set; }
    public int Animal3_Type { get; set; }
    public int Animal4_Type { get; set; }
    public int Animal5_Type { get; set; }
    public int Animal1_Grade { get; set; }
    public int Animal2_Grade { get; set; }
    public int Animal3_Grade { get; set; }
    public int Animal4_Grade { get; set; }
    public int Animal5_Grade { get; set; }
    public int Synergy_Type { get; set; }
    public float Synergy_Value { get; set; }

    public SynergyStat() { }

    public SynergyStat(int synergyId)
    {
        this.SynergyData = DataTableMgr.GetSynergyTable().Get(synergyId);
    }
}


public class ExchangeStat
{
    public class RequrieExchangeInfo
    {
        public int Type { get; set; }
        public int ID { get; set; }
        public string Value { get; set; }
    }

    private ExchangeData exchangeData;
    public ExchangeData ExchangeData
    {
        get
        {
            if (exchangeData == null)
            {
                exchangeData = new ExchangeData();
            }
            return exchangeData;
        }
        set
        {
            exchangeData = value;
            {
                exchangeData = value;
                Exchange_ID = exchangeData.Exchange_ID;
                World_type = exchangeData.World_Type;
                Exchange_Num = exchangeData.Exchange_Num;
                Furniture_Type = exchangeData.Furniture_Type;
                Exchange_Level = exchangeData.Exchange_Level;
                Require_Resource1_Type = exchangeData.Require_Resource1_Type;
                Require_Resource1_ID = exchangeData.Require_Resource1_ID;
                Require_Resource1_Value = exchangeData.Require_Resource1_Value;
                Require_Resource2_Type = exchangeData.Require_Resource2_Type;
                Require_Resource2_ID = exchangeData.Require_Resource2_ID;
                Require_Resource2_Value = exchangeData.Require_Resource2_Value;
                Require_Resource3_Type = exchangeData.Require_Resource3_Type;
                Require_Resource3_ID = exchangeData.Require_Resource3_ID;
                Require_Resource3_Value = exchangeData.Require_Resource3_Value;
                Reward_ID = exchangeData.Reward_ID;
            }
        }
    }
    public int Exchange_ID { get; set; }
    public int World_type { get; set; }
    public int Exchange_Num { get; set; }
    public int Furniture_Type { get; set; }
    public int Exchange_Level { get; set; }
    public int Require_Resource1_Type { get; set; }
    public int Require_Resource1_ID { get; set; }
    public string Require_Resource1_Value { get; set; }
    public int Require_Resource2_Type { get; set; }
    public int Require_Resource2_ID { get; set; }
    public string Require_Resource2_Value { get; set; }
    public int Require_Resource3_Type { get; set; }
    public int Require_Resource3_ID { get; set; }
    public string Require_Resource3_Value { get; set; }
    public int Reward_ID { get; set; }
    public int RequireCount
    {
        get
        {
            int count = 0;

            if (Require_Resource1_ID > 0)
            {
                requireInfos.Add(new RequrieExchangeInfo() { Type = Require_Resource1_Type, ID = Require_Resource1_ID, Value = Require_Resource1_Value });
                count++;
            }

            if (Require_Resource2_ID > 0)
            {
                requireInfos.Add(new RequrieExchangeInfo() { Type = Require_Resource2_Type, ID = Require_Resource2_ID, Value = Require_Resource2_Value });
                count++;
            }

            if (Require_Resource3_ID > 0)
            {
                requireInfos.Add(new RequrieExchangeInfo() { Type = Require_Resource3_Type, ID = Require_Resource3_ID, Value = Require_Resource3_Value });
                count++;
            }

            return count;
        }
    }
    public List<RequrieExchangeInfo> requireInfos = new List<RequrieExchangeInfo>();
    public bool IsCompleted { get; set; } = false;

    public ExchangeStat() { }

    public ExchangeStat(int exchangeId)
    {
        this.ExchangeData = DataTableMgr.GetExchangeTable().Get(exchangeId);
    }
}


public class ResourceStat
{
    private ResourceData resourceData;
    public ResourceData ResourceData
    {
        get
        {
            if (resourceData == null)
            {
                resourceData = new ResourceData();
            }
            return resourceData;
        }
        set
        {
            resourceData = value;
            {
                resourceData = value;
                Resource_ID = resourceData.Resource_ID;
                Resource_Name_ID = resourceData.Resource_Name_ID;
                World_Type = resourceData.World_Type;
                Floor_Type = resourceData.Floor_Type;
                Resource_Type = resourceData.Resource_Type;
                Sale_Resource_ID = resourceData.Sale_Resource_ID;
                Sale_Price = resourceData.Sale_Price;
            }
        }
    }
    public int Resource_ID { get; set; }
    public string Resource_Name_ID { get; set; }
    public int World_Type { get; set; }
    public int Floor_Type { get; set; }
    public int Resource_Type { get; set; }
    public int Sale_Resource_ID { get; set; }
    public string Sale_Price { get; set; }

    public ResourceStat() { }

    public ResourceStat(int resourceId)
    {
        this.ResourceData = DataTableMgr.GetResourceTable().Get(resourceId);
    }
}


public class RewardStat
{
    public class RequrieRewardInfo
    {
        public int Type { get; set; }
        public int Id { get; set; }
        public string Value { get; set; }
    }

    private RewardData rewardData;
    public RewardData RewardData
    {
        get
        {
            if (rewardData == null)
            {
                rewardData = new RewardData();
            }
            return rewardData;
        }
        set
        {
            rewardData = value;
            {
                rewardData = value;
                Reward_ID = rewardData.Reward_ID;
                World_Type = rewardData.World_Type;
                Reward1_Type = rewardData.Reward1_Type;
                Reward1_ID = rewardData.Reward1_ID;
                Reward1_Value = rewardData.Reward1_Value;
                Reward2_Type = rewardData.Reward2_Type;
                Reward2_ID = rewardData.Reward2_ID;
                Reward2_Value = rewardData.Reward2_Value;
                Reward3_Type = rewardData.Reward3_Type;
                Reward3_ID = rewardData.Reward3_ID;
                Reward3_Value = rewardData.Reward3_Value;
            }
        }
    }
    public int Reward_ID { get; set; }
    public int World_Type { get; set; }
    public int Reward1_Type { get; set; }
    public int Reward1_ID { get; set; }
    public string Reward1_Value { get; set; }
    public int Reward2_Type { get; set; }
    public int Reward2_ID { get; set; }
    public string Reward2_Value { get; set; }
    public int Reward3_Type { get; set; }
    public int Reward3_ID { get; set; }
    public string Reward3_Value { get; set; }
    public int RequireCount
    {
        get
        {
            int count = 0;

            if (Reward1_ID > 0)
            {
                requireInfos.Add(new RequrieRewardInfo() { Type = Reward1_Type, Id = Reward1_ID, Value = Reward1_Value });
                count++;
            }

            if (Reward2_ID > 0)
            {
                requireInfos.Add(new RequrieRewardInfo() { Type = Reward2_Type, Id = Reward2_ID, Value = Reward2_Value });
                count++;
            }

            if (Reward3_ID > 0)
            {
                requireInfos.Add(new RequrieRewardInfo() { Type = Reward3_Type, Id = Reward3_ID, Value = Reward3_Value });
                count++;
            }

            return count;
        }
    }

    public List<RequrieRewardInfo> requireInfos = new List<RequrieRewardInfo>();

    public RewardStat() { }

    public RewardStat(int rewardId)
    {
        this.RewardData = DataTableMgr.GetRewardTable().Get(rewardId);
    }
}
