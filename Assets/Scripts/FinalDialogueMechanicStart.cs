using UnityEngine;

public class BulletHitCounter : MonoBehaviour
{
    public TrainingDialogueFinal trainingDialogueFinal;
    private int hitCount = 0;
    private bool dialogueStarted = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet") && !dialogueStarted)
        {
            hitCount++;
            Debug.Log($"BulletHitCounter: Пуля попала в 'Target' {hitCount} раз");

            if (hitCount >= 7)
            {
                Debug.Log("BulletHitCounter: Достигнуто 7 попаданий в 'Target', запуск диалога");
                trainingDialogueFinal.StartDialogue();
                dialogueStarted = true;
            }
        }
    }
}