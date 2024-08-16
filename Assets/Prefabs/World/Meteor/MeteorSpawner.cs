using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab;
    public float spawnIntervalMin = 3f;
    public float spawnIntervalMax = 10f;
    private float timeToNextSpawn;

    void Start()
    {
        // ù ��° ���� Ÿ�̸� �ʱ�ȭ
        SetNextSpawnTime();
    }

    void Update()
    {
        // �ð� ���
        timeToNextSpawn -= Time.deltaTime;

        if (timeToNextSpawn <= 0)
        {
            SpawnMeteor();
            SetNextSpawnTime();
        }
    }

    void SetNextSpawnTime()
    {
        // ���� ���������� �ð��� �����ϰ� ����
        timeToNextSpawn = Random.Range(spawnIntervalMin, spawnIntervalMax);
    }

    void SpawnMeteor()
    {
        // ���˺��� ������ ��ġ�� ���� (��: ȭ���� ���� ����)
        Vector3 spawnPosition = new Vector3(Random.Range(1f, 3f), 3f, 0);
        Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);
    }
}