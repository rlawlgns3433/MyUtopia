using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
public class UiInvitation : MonoBehaviour
{
    private static readonly string format = "{0}를 지불하고 동물을 초대할까요?";

    public TextMeshProUGUI textConfirm;
    public Button buttonConfirm;

    public FloorStat floorStat;
    public Floor floor;
    public InvitationData invitationData;

    public void Set()
    {
        floor = FloorManager.Instance.floors["B1"];
        floorStat = floor.FloorStat;
        SetUi();
    }

    public void SetUi()
    {
        invitationData = DataTableMgr.GetInvitationTable().Get(11100+ floorStat.Grade);

        textConfirm.text = string.Format(format, new BigNumber(invitationData.Level_Up_Coin_Value));
    }

    public void OnClickConfirm()
    {
        if (CurrencyManager.currency[(CurrencyType)invitationData.Level_Up_Coin_ID] < invitationData.Level_Up_Coin_Value)
            return;

        // 전체 동물수와 1층의 max와 동일하거나 크면 리턴
        int maximumCount = floorStat.Max_Population;
        int currentCount = 0;
        foreach (var currentFloor in FloorManager.Instance.floors.Values)
        {
            foreach(var animal in currentFloor.animals)
            {
                if(animal.animalWork == null)
                    continue;
                currentCount++;
            }
        }

        if(currentCount >= maximumCount)
            return;

        CurrencyManager.currency[(CurrencyType)invitationData.Level_Up_Coin_ID] -= invitationData.Level_Up_Coin_Value;
        float totalRate = invitationData.Get_Animal1_Rate + invitationData.Get_Animal2_Rate + invitationData.Get_Animal3_Rate + invitationData.Get_Animal4_Rate + invitationData.Get_Animal5_Rate + invitationData.Get_Animal6_Rate;

        float[] rates = new float[]
        {
            invitationData.Get_Animal1_Rate,
            invitationData.Get_Animal2_Rate,
            invitationData.Get_Animal3_Rate,
            invitationData.Get_Animal4_Rate,
            invitationData.Get_Animal5_Rate,
            invitationData.Get_Animal6_Rate
        };

        int[] ids = new int[]
        {
            invitationData.Get_Animal_ID_1,
            invitationData.Get_Animal_ID_2,
            invitationData.Get_Animal_ID_3,
            invitationData.Get_Animal_ID_4,
            invitationData.Get_Animal_ID_5,
            invitationData.Get_Animal_ID_6
        };

        // 랜덤 값 생성
        float value = Random.Range(0, totalRate);
        float cumulativeRate = 0f;
        int animalId = 0;

        // 루프를 통한 확률 체크
        for (int i = 0; i < rates.Length; i++)
        {
            cumulativeRate += rates[i];
            if (value <= cumulativeRate)
            {
                animalId = ids[i];
                break;
            }
        }

        // 확률에 따라 뽑기

        var floor = FloorManager.Instance.GetFloor($"B{floorStat.Floor_Num}");
        var pos = floor.transform.position;
        pos.z -= 5;
        GameManager.Instance.GetAnimalManager().Create(pos, floor, animalId, 0);
    }
}
