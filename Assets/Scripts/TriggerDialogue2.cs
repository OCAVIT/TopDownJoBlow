using UnityEngine;

public class DialogueTrigger2 : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            Debug.Log("DialogueTrigger: ����� ����� � �������");
            DialogueSystem2 dialogueSystem = FindObjectOfType<DialogueSystem2>();
            if (dialogueSystem != null)
            {
                dialogueSystem.StartDialogue();
            }
            hasTriggered = true;
        }
    }
}