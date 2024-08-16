using UnityEngine;

public class MeteorController : MonoBehaviour
{
    public float downwardSpeed = 2f;     // �Ʒ��� �̵��ϴ� �ӵ�
    public float rotationSpeed = 50f;    // Y�� ȸ�� �ӵ�
    private float leftwardSpeed;         // �������� �̵��ϴ� �ӵ� (1~5 �������� �����ϰ� ����)

    void Start()
    {
        // �������� �̵��ϴ� �ӵ��� 1~5 �������� �����ϰ� ����
        leftwardSpeed = Random.Range(1f, 5f);
    }

    void Update()
    {
        // X���� �߽����� ȸ��
        transform.Rotate( rotationSpeed * Time.deltaTime, 0, 0);

        // ���� �Ʒ� �������� �̵�
        Vector3 movement = new Vector3(-leftwardSpeed, -downwardSpeed, 0) * Time.deltaTime;
        transform.Translate(movement, Space.World);

        // ������Ʈ�� ȭ�� �Ʒ��� ������� ����
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    void OnMouseDown()
    {
        // ��ġ �Ǵ� Ŭ�� �� �ֿܼ� OK ���
        Debug.Log("OK");
        // ������Ʈ ����
        Destroy(gameObject);
    }
}