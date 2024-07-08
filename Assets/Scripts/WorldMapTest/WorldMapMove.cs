using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldMapMove : MonoBehaviour
{
    private Vector3 mousePos;
    private Vector3 currentMousePos;
    private bool isDragging = false;
    public float speed = 5f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            mousePos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            currentMousePos = Input.mousePosition;
            var pos = currentMousePos - mousePos;
            float angleX = pos.y * speed * Time.deltaTime;
            float angleY = -pos.x * speed * Time.deltaTime;
            transform.Rotate(Vector3.up, angleY, Space.World);
            transform.Rotate(Vector3.right, angleX, Space.World);

            mousePos = currentMousePos;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
