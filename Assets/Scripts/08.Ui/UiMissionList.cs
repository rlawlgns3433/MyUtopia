using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class UiMissionList : MonoBehaviour
{
    private MissionTable missionTable;
    public UiMission uiMissionPrefab;
    public Transform missionParent;
    private List<UiMission> missionList;
    private List<MissionData> missionData;

    private void Awake()
    {
        missionTable = DataTableMgr.GetMissionTable();
        missionList = new List<UiMission>();
        missionData = new List<MissionData>();
        missionData = missionTable.GetAllMissions();
        for (int i = 0; i < missionData.Count; i++)
        {
            Debug.Log($"missionDataLoad{i} = {missionData[i].Mission_ID}");
        }
        SetDailyMissionData();
    }

    private void OnEnable()
    {
        if (missionList[0] == null)
        {
            SetDailyMissionData();
        }
        else
        {
            foreach (var mission in missionList)
            {
                mission.UpDateMission();
            }
        }
    }
    private void OnDisable()
    {
        
    }
    private void SetDailyMissionData()
    {
        int missionCount = 0;
        foreach(var mission in missionData)
        {
            if(mission.Mission_Type == (int)MissionTypes.Daily)
            {
                missionCount++;
            }
        }
        for(int i = 0; i<missionCount; i++)
        {
            bool create = false;
            var floor = FloorManager.Instance.GetFloor($"B{missionData[i].Target_ID / 10000 % 100}");
            foreach(var building in floor.buildings)
            {
                if (missionData[i].Target_ID == building.buildingId)
                {
                    if(!building.BuildingStat.IsLock)
                    {
                        create = true;
                    }
                }
            }
            if(create)
            {
                missionList.Add(Instantiate(uiMissionPrefab, missionParent));
                missionList[i].SetData(missionData[i]);
            }
        }
    }

    private void ClearMissionList()
    {
        foreach(var mission in missionList)
        {
            Destroy(mission.gameObject);
        }
    }
}
