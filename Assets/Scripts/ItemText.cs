using UnityEngine;
using TMPro;

public class FacePlayerAndActivate : MonoBehaviour
{
    public Transform player; // Ссылка на объект игрока
    public float activationDistance = 10f; // Расстояние, на котором текст активируется

    private TextMeshPro textMeshPro;

    void Start()
    {
        textMeshPro = GetComponent<TextMeshPro>();

        if (textMeshPro == null)
        {
            Debug.LogError("TextMeshPro component not found on this GameObject.");
        }
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogError("Player Transform is not assigned.");
            return;
        }

        // Вычисляем расстояние до игрока
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Активируем или деактивируем текст в зависимости от расстояния
        textMeshPro.enabled = distanceToPlayer <= activationDistance;

        // Поворачиваем текст лицевой стороной к игроку
        if (textMeshPro.enabled)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

            // Разворачиваем на 180 градусов по оси Y
            lookRotation *= Quaternion.Euler(0, 180, 0);

            transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
        }
    }
}