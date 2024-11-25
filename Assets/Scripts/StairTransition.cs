using UnityEngine;

public class StairTransparencyController : MonoBehaviour
{
    public GameObject highFloor; // Ссылка на объект High Floor
    public Transform player; // Ссылка на игрока
    public Camera mainCamera; // Ссылка на основную камеру
    public float transparencyDistance = 5f; // Расстояние, на котором начнется изменение прозрачности
    public float deactivateHeight = 0f; // Высота, на которой High Floor станет неактивным

    private int highFloorLayer;
    private int defaultCullingMask;
    private bool isDeactivated = false;

    void Start()
    {
        // Получаем слой High Floor
        highFloorLayer = LayerMask.NameToLayer("HighFloorLayer");
        defaultCullingMask = mainCamera.cullingMask;
    }

    void Update()
    {
        if (isDeactivated) return;

        float playerHeight = player.position.y;

        // Изменяем видимость слоя, если игрок находится в пределах transparencyDistance
        if (playerHeight < transparencyDistance)
        {
            float alpha = Mathf.Clamp01(playerHeight / transparencyDistance);
            SetLayerVisibility(alpha > 0);
        }

        // Деактивируем High Floor, если игрок ниже deactivateHeight
        if (playerHeight < deactivateHeight)
        {
            highFloor.SetActive(false);
            isDeactivated = true;
        }
    }

    void SetLayerVisibility(bool isVisible)
    {
        if (isVisible)
        {
            mainCamera.cullingMask |= (1 << highFloorLayer);
        }
        else
        {
            mainCamera.cullingMask &= ~(1 << highFloorLayer);
        }
    }
}