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
            Debug.Log($"BulletHitCounter: ���� ������ � 'Target' {hitCount} ���");

            if (hitCount >= 7)
            {
                Debug.Log("BulletHitCounter: ���������� 7 ��������� � 'Target', ������ �������");
                trainingDialogueFinal.StartDialogue();
                dialogueStarted = true;
            }
        }
    }
}