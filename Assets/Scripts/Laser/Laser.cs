using UnityEngine;

public class LaserPointerScript : MonoBehaviour
{
    public Transform laserOrigin; // “очка, откуда исходит лазер
    public float laserRange = 100f; // ƒальность лазера
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        // ”станавливаем начальную точку лазера
        lineRenderer.SetPosition(0, laserOrigin.position);

        // ќпредел€ем направление лазера
        Vector3 direction = laserOrigin.forward;

        // ¬ыполн€ем Raycast дл€ определени€, куда попадает лазер
        RaycastHit hit;
        if (Physics.Raycast(laserOrigin.position, direction, out hit, laserRange))
        {
            // ≈сли лазер попадает в объект, устанавливаем конечную точку на месте столкновени€
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            // ≈сли лазер никуда не попадает, устанавливаем конечную точку на максимальной дальности
            lineRenderer.SetPosition(1, laserOrigin.position + direction * laserRange);
        }
    }
}