using UnityEngine;

public class MeteorController : MonoBehaviour
{
    public float downwardSpeed = 2f;     // 아래로 이동하는 속도
    public float rotationSpeed = 50f;    // Y축 회전 속도
    private float leftwardSpeed;         // 왼쪽으로 이동하는 속도 (1~5 범위에서 랜덤하게 설정)

    void Start()
    {
        // 왼쪽으로 이동하는 속도를 1~5 범위에서 랜덤하게 설정
        leftwardSpeed = Random.Range(1f, 5f);
    }

    void Update()
    {
        // X축을 중심으로 회전
        transform.Rotate( rotationSpeed * Time.deltaTime, 0, 0);

        // 좌측 아래 방향으로 이동
        Vector3 movement = new Vector3(-leftwardSpeed, -downwardSpeed, 0) * Time.deltaTime;
        transform.Translate(movement, Space.World);

        // 오브젝트가 화면 아래로 사라지면 제거
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    void OnMouseDown()
    {
        // 터치 또는 클릭 시 콘솔에 OK 출력
        Debug.Log("OK");
        // 오브젝트 제거
        Destroy(gameObject);
    }
}