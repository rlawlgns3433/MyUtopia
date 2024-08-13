using UnityEngine;

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
    public static readonly string Building = "BuildingTable";
    public static readonly string Facility = "FacilityTable";
    public static readonly string Merge = "MergeTable";
    public static readonly string Resource = "ResourceTable";
    public static readonly string Furniture = "FurnitureTable";
    public static readonly string Recipe = "RecipeTable";
    public static readonly string Item = "ItemTable";
    public static readonly string Invitation = "InvitationTable";
    public static readonly string World = "WorldTable";
    public static readonly string Synergy = "SynergyTable";
    public static readonly string Exchange = "ExchangeTable";
    public static readonly string Mission = "MissionTable";
    public static readonly string Reward = "RewardTable";

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
    public static readonly string VirtualCamera = "VirtualCamera";
    public static readonly string AnimalManager = "AnimalManager";
    public static readonly string FloorInformation = "FloorInformation";
    public static readonly string AnimalInventory = "AnimalInventory";
    public static readonly string Floors = "Floors";

}
public static class ButtonText
{
    public static readonly string Move = "이동";
    public static readonly string Success = "획득";
    public static readonly string Completed = "완료";
}
public static class SortingLayers
{
    public static readonly string Default = "Default";
    public static readonly string UI = "UI";

}

public static class Layers
{
    public static readonly string UI = "UI";
}

public static class InputActions
{
    public static readonly string Swipe = "Swipe";
    public static readonly string Press = "Press";
    public static readonly string Position = "Position";
}

public static class AnimationHash
{
    public static readonly int Walk = Animator.StringToHash("Walk");
    public static readonly int Run = Animator.StringToHash("Run");
    public static readonly int Idle = Animator.StringToHash("Idle_A");
    public static readonly int Rest = Animator.StringToHash("Sit");
    public static readonly int Work = Animator.StringToHash("Attack");

    public static readonly int eyeAnnoyed = Animator.StringToHash("Eyes_Annoyed");
}

public enum CurrencyType
{
    Diamond = 90000101,
    Coin = 91050201,
    /*
    건물 테이블
	생산 재화타입이 7(특산물)인 경우
	레시피 테이블 [해금레벨] <= [건물 레벨] 레시피를 모두 가져와 선택할 수 있게 함
     */
}

public enum CurrencyProductType
{
    CopperStone = 91050301,
    SilverStone = 91050302,
    GoldStone = 91050303,
    CopperIngot = 91040401,
    SilverIngot = 91040402,
    GoldIngot = 91040403,
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
    Floor = 2,
    Building = 3,
}
public enum MissionTypes
{
    Daily = 1,
    Weekly = 2,
    Monthly = 3,
    Event = 4,
}
public enum SceneIds
{
    None = -1,
    WorldSelect,
    WorldLandOfHope,
}

public enum AnimalListMode
{
    None = -1,
    AnimalList,
    Exchange,
    Eliminate,
}