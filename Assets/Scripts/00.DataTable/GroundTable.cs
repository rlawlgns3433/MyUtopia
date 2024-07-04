using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 데이터 테이블 변경에 따른 수정 필요

public struct GroundData
{
    public int Class_ID { get; set; }
    public int Class_WorldType { get; set; }
    public int Class_Floor { get; set; }
    public int Class_Type { get; set; }
    public string Class_Name { get; set; }
    public int Class_Grade { get; set; }
    public int Grade_Building { get; set; }
    public int Grade_facility { get; set; }
    public int Floor_MaxPopulation { get; set; }
    public string Gd_RequiredGold { get; set; } // string(991c) -> BigInteger
    public int Gd_RequiredResourceType { get; set; }

    public string GetClassName()
    {
        return DataTableMgr.GetStringTable().Get(Class_Name);
    }

}

public class GroundTable : DataTable
{
    private Dictionary<string, AnimalData> table = new Dictionary<string, AnimalData>();
    public override void Load(string path)
    {
        throw new System.NotImplementedException();
    }
}
