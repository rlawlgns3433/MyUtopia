using UnityEngine;

public class MeteorController : MonoBehaviour
{
    public float downwardSpeed = 2f;
    public float rotationSpeed = 50f;
    private float leftwardSpeed;

    void Start()
    {
        leftwardSpeed = Random.Range(1f, 5f);
    }

    void Update()
    {
        transform.Rotate( rotationSpeed * Time.deltaTime, 0, 0);
        Vector3 movement = new Vector3(-leftwardSpeed, -downwardSpeed, 0) * Time.deltaTime;
        transform.Translate(movement, Space.World);
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    void OnMouseDown()
    {
        Destroy(gameObject);
    }
}