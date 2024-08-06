using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : Singleton<MissionManager>
{
    private Dictionary<int, int> missionManager = new Dictionary<int, int>();

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
}
