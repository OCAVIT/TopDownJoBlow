using UnityEngine;

public class OutdoorTRIG : MonoBehaviour
{
    public DialogueSystem dialogueSystem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueSystem.BeginDialogue();
        }
    }
}