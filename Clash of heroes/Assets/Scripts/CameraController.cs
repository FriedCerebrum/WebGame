using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target1;
    public Transform target2;

    public float smoothSpeed = 0.5f;
    public float zoomSpeed = 2.0f;
    public float maxZoom = 10.0f;
    public float minZoom = 2.0f;
    public float maxYOffset = 3.0f;

    private Camera mainCamera;
    private Vector3 initialCameraPosition;
    public Transform centralPoint;
    void Start()
    {
        mainCamera = Camera.main;
        initialCameraPosition = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        // Вычисляем центр между двумя бойцами
        Vector3 centerPoint = (target1.position + target2.position) / 2f;

        // Вычисляем новую Y-координату центральной точки
        float newY = Mathf.Clamp(centerPoint.y, centralPoint.position.y - maxYOffset, centralPoint.position.y + maxYOffset);

        // Учитываем максимальное вертикальное смещение
        centerPoint.y = newY;

        // Вычисляем новый размер зума
        float distance = Vector3.Distance(target1.position, target2.position);
        float targetSize = Mathf.Clamp(distance * 0.5f, minZoom, maxZoom);

        // Плавно перемещаем камеру к новой позиции
        Vector3 newPosition = Vector3.Lerp(transform.position, centerPoint, smoothSpeed * Time.deltaTime);
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);

        // Плавно меняем размер камеры (зум)
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetSize, zoomSpeed * Time.deltaTime);
    }
}
