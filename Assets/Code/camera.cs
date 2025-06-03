using UnityEngine;
using System.Collections; // Добавлено для IEnumerator

public class CameraFollow : MonoBehaviour
{
    private Transform player;
    public float zoomedInSize = 2.5f; // Размер камеры при приближении
    public float normalSize = 5f; // Нормальный размер камеры
    public float zoomSpeed = 2f; // Скорость изменения размера
    private bool isZooming = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (!isZooming)
        {
            Vector3 temp = transform.position;
            temp.x = player.position.x;
            temp.y = player.position.y;
            transform.position = temp;
        }
    }

    public void StartDialog()
    {
        StartCoroutine(ZoomCamera(zoomedInSize));
    }

    public void EndDialog()
    {
        StartCoroutine(ZoomCamera(normalSize));
    }

    private IEnumerator ZoomCamera(float targetSize)
    {
        isZooming = true;
        Camera camera = GetComponent<Camera>();
        float startSize = camera.orthographicSize;

        float elapsed = 0f;
        while (elapsed < zoomSpeed)
        {
            camera.orthographicSize = Mathf.Lerp(startSize, targetSize, elapsed / zoomSpeed);
            elapsed += Time.deltaTime;
            yield return null;
        }
        camera.orthographicSize = targetSize; // Устанавливаем конечный размер
        isZooming = false;
    }
}