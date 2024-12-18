using UnityEngine;
using TMPro;

public class FacePlayerAndActivate : MonoBehaviour
{
    public Transform player;
    public float activationDistance = 10f;

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

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        textMeshPro.enabled = distanceToPlayer <= activationDistance;

        if (textMeshPro.enabled)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

            lookRotation *= Quaternion.Euler(0, 180, 0);

            transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
        }
    }
}