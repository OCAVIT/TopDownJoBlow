using UnityEngine;

public class DialogueTrigger3 : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            Debug.Log("DialogueTrigger: Игрок вошел в триггер");
            DialogueTraining1 dialogueSystem = FindObjectOfType<DialogueTraining1>();
            if (dialogueSystem != null)
            {
                dialogueSystem.StartDialogue();
            }
            hasTriggered = true;
        }
    }
}