using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public GameObject player;
    public DialogueManagerLadder dialogueManager;

    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.gameObject == player)
        {
            hasTriggered = true;
            if (dialogueManager != null)
            {
                dialogueManager.StartDialogue();
            }
        }
    }
}