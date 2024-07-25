using UnityEngine;
using UnityEngine.SceneManagement;

public class TestPanel : MonoBehaviour
{
    public void ShowMeTheMoney()
    {
        CurrencyManager.currency[CurrencyType.Coin] += 10000;
        CurrencyManager.currency[CurrencyType.CopperStone] += 10000;
        CurrencyManager.currency[CurrencyType.SilverStone] += 10000;
        CurrencyManager.currency[CurrencyType.GoldStone] += 10000;
        CurrencyManager.currency[CurrencyType.CopperIngot] += 10000;
        CurrencyManager.currency[CurrencyType.SilverIngot] += 10000;
        CurrencyManager.currency[CurrencyType.GoldIngot] += 10000;
    }

    // 현재 세이브 파일 삭제
    // 씬 다시 로드
    public void ResetSaveData()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    // 중간 플레이 세이브 파일 로드
}
