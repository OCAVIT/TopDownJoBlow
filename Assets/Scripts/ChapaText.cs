using UnityEngine;
using TMPro;

public class FacePlayerAndActivate : MonoBehaviour
{
    public Transform player;
    public float activationDistance = 10f;
    public Vector3 rotationOffset = Vector3.zero;
    public AudioClip activationClip;
    public DialogueSystem3 dialogueSystem;

    private TextMeshPro textMeshPro;
    private AudioSource audioSource;

    void Start()
    {
        textMeshPro = GetComponent<TextMeshPro>();
        audioSource = gameObject.AddComponent<AudioSource>();

        if (textMeshPro == null)
        {
            Debug.LogError("TextMeshPro component not found on this GameObject.");
        }

        if (dialogueSystem == null)
        {
            Debug.LogError("DialogueSystem3 is not assigned.");
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

            lookRotation *= Quaternion.Euler(rotationOffset);

            transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (activationClip != null)
                {
                    audioSource.PlayOneShot(activationClip);
                }

                textMeshPro.text = "";
                Invoke(nameof(StartDialogue), 1f);
            }
        }
    }

    private void StartDialogue()
    {
        if (dialogueSystem != null)
        {
            dialogueSystem.StartDialogue();
        }
    }
}