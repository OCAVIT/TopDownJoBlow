using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    private Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        // —охран€ем начальное смещение камеры относительно игрока
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // –ассчитываем желаемую позицию камеры
        Vector3 desiredPosition = player.position + offset;

        // ѕлавно перемещаем камеру к желаемой позиции
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        transform.position = smoothedPosition;
    }
}