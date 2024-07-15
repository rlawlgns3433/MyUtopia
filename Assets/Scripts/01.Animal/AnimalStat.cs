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
