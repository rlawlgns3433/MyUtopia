using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using Cysharp.Threading.Tasks;
public class UiInvitation : MonoBehaviour
{
    private static readonly string format = "{0}를 지불하고 동물을 초대할까요?";

    public TextMeshProUGUI textConfirm;
    public Button buttonConfirm;
    public ParticleSystem ps;
    public FloorStat floorStat;
    public Floor floor;
    public InvitationData invitationData;
    public GameObject balloonObj;
    private float offSetY = 5f;
    private float targetPosOffset = 1f;
    private List<GameObject> balloons = new List<GameObject>();
    //private bool isCreating = false;
    //private int createCount = 0;
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
        var floor = FloorManager.Instance.GetCurrentFloor();
        //���� ���� ���� �ִ�ġ�϶� ����
        if (floor.FloorStat.Max_Population <= floor.animals.Count)
            return;
        // ��ü �������� 1���� max�� �����ϰų� ũ�� ����
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
        if (FloorManager.Instance.IsCreating)
            return;
        if (currentCount >= maximumCount)
            return;

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

        // ���� �� ����
        float value = Random.Range(0, totalRate);
        float cumulativeRate = 0f;
        int animalId = 0;

        // ������ ���� Ȯ�� üũ
        for (int i = 0; i < rates.Length; i++)
        {
            cumulativeRate += rates[i];
            if (value <= cumulativeRate)
            {
                animalId = ids[i];
                break;
            }
        }
        // Ȯ���� ���� �̱�
        floor = FloorManager.Instance.GetCurrentFloor();
        var pos = floor.transform.position;
        pos.z -= 8;
        if (FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.MurgeAnimalConfirm || FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.Confirm)
        {
            FloorManager.Instance.touchManager.tutorial.SetEmpty();
        }
        FloorManager.Instance.IsCreating = false;
        CreateBalloon(pos, floor, animalId, invitationData.Level_Up_Coin_Value).Forget();
    }

    public async UniTask CreateBalloon(Vector3 target, Floor floor, int id,int value)
    {
        FloorManager.Instance.IsCreating = true;
        var balloonPos = target;
        balloonPos.y += offSetY;
        var balloon = Instantiate(balloonObj, balloonPos, Quaternion.identity);
        balloons.Add(balloon);
        var targetPos = target;
        targetPos.y += targetPosOffset;
        await balloon.transform.DOMove(targetPos, 3).SetEase(Ease.InOutQuad).AsyncWaitForCompletion();
        targetPos.y += targetPosOffset;
        Instantiate(ps, targetPos, Quaternion.identity);
        ps.Play();
        Destroy(balloon);
        balloons.Remove(balloon);
        await UniTask.Delay(500);
        if (FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.MurgeAnimalConfirm)
        {
            FloorManager.Instance.touchManager.tutorial.SetTutorialProgress();
            id= FloorManager.Instance.touchManager.tutorial.targetAnimalId;
        }
        GameManager.Instance.GetAnimalManager().Create(target, floor, id, 0);
        CurrencyManager.currency[(CurrencyType)invitationData.Level_Up_Coin_ID] -= value;
        FloorManager.Instance.IsCreating = false;
        if (FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.Confirm)
        {
            FloorManager.Instance.touchManager.tutorial.SetTutorialProgress();
            FloorManager.Instance.touchManager.tutorial.targetAnimalId = id;
        }

    }
}
