using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab;
    public float spawnIntervalMin = 3f;
    public float spawnIntervalMax = 10f;
    private float timeToNextSpawn;

    void Start()
    {
        // 첫 번째 스폰 타이머 초기화
        SetNextSpawnTime();
    }

    void Update()
    {
        // 시간 경과
        timeToNextSpawn -= Time.deltaTime;

        if (timeToNextSpawn <= 0)
        {
            SpawnMeteor();
            SetNextSpawnTime();
        }
    }

    void SetNextSpawnTime()
    {
        // 다음 스폰까지의 시간을 랜덤하게 설정
        timeToNextSpawn = Random.Range(spawnIntervalMin, spawnIntervalMax);
    }

    void SpawnMeteor()
    {
        // 별똥별이 등장할 위치를 설정 (예: 화면의 우측 위쪽)
        Vector3 spawnPosition = new Vector3(Random.Range(1f, 3f), 3f, 0);
        Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);
    }
}