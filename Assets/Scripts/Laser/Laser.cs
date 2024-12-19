using UnityEngine;

public class LaserPointerScript : MonoBehaviour
{
    public Transform laserOrigin; // �����, ������ ������� �����
    public float laserRange = 100f; // ��������� ������
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        // ������������� ��������� ����� ������
        lineRenderer.SetPosition(0, laserOrigin.position);

        // ���������� ����������� ������
        Vector3 direction = laserOrigin.forward;

        // ��������� Raycast ��� �����������, ���� �������� �����
        RaycastHit hit;
        if (Physics.Raycast(laserOrigin.position, direction, out hit, laserRange))
        {
            // ���� ����� �������� � ������, ������������� �������� ����� �� ����� ������������
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            // ���� ����� ������ �� ��������, ������������� �������� ����� �� ������������ ���������
            lineRenderer.SetPosition(1, laserOrigin.position + direction * laserRange);
        }
    }
}