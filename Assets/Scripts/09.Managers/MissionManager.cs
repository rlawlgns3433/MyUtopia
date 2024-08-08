using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : Singleton<MissionManager>
{
    private Dictionary<int, int> missionManager = new Dictionary<int, int>();
    private bool isSetUi = false;
    public bool IsSetUi
    {
        get
        {
            return isSetUi;
        }
        set
        {
            isSetUi = value;
        }
    }

    public void AddMissionCount(int buildingId)
    {
        if(missionManager.ContainsKey(buildingId))
        {
            missionManager[buildingId]++;
        }
        else
        {
            missionManager[buildingId] = 1;
        }
    }

    public int GetMissionCount(int id)
    {
        if(missionManager.ContainsKey(id))
        {
            return missionManager[id];
        }
        return 0;
    }

    public void SetMissionCount(int id, int value)
    {
        if(missionManager.ContainsKey(id))
        {
            missionManager[id] += value;
        }
        else
        {
            missionManager[id] = value;         
        }
    }
}
