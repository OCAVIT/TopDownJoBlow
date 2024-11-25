using UnityEngine;

public class StairTransparencyController : MonoBehaviour
{
    public GameObject highFloor; // ������ �� ������ High Floor
    public Transform player; // ������ �� ������
    public Camera mainCamera; // ������ �� �������� ������
    public float transparencyDistance = 5f; // ����������, �� ������� �������� ��������� ������������
    public float deactivateHeight = 0f; // ������, �� ������� High Floor ������ ����������

    private int highFloorLayer;
    private int defaultCullingMask;
    private bool isDeactivated = false;

    void Start()
    {
        // �������� ���� High Floor
        highFloorLayer = LayerMask.NameToLayer("HighFloorLayer");
        defaultCullingMask = mainCamera.cullingMask;
    }

    void Update()
    {
        if (isDeactivated) return;

        float playerHeight = player.position.y;

        // �������� ��������� ����, ���� ����� ��������� � �������� transparencyDistance
        if (playerHeight < transparencyDistance)
        {
            float alpha = Mathf.Clamp01(playerHeight / transparencyDistance);
            SetLayerVisibility(alpha > 0);
        }

        // ������������ High Floor, ���� ����� ���� deactivateHeight
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