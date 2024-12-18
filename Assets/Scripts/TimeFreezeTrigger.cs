using UnityEngine;
using TMPro;
using System.Collections;

public class TimeFreezeTrigger : MonoBehaviour
{
    public GameObject dialoguePanel; // Панель диалога
    public GameObject NMT;
    public TMP_Text nameText; // Текстовое поле для имени
    public TMP_Text dialogueText; // Текстовое поле для диалога
    public GameObject vitya; // Объект Витя

    private bool isTimeFrozen = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTimeFrozen)
        {
            StartCoroutine(FreezeTime());
        }
    }

    private IEnumerator FreezeTime()
    {
        // Заморозка времени
        Time.timeScale = 0f;
        isTimeFrozen = true;

        // Активация панели диалога и установка текста
        NMT.SetActive(true);
        dialoguePanel.SetActive(true);
        nameText.text = "Витя";
        dialogueText.text = "Червянутая идея покидать двор...";

        // Активация объекта Витя
        if (vitya != null)
        {
            vitya.SetActive(true);
        }

        // Ожидание 2 секунд или нажатия клавиши Space
        float elapsedTime = 0f;
        while (elapsedTime < 2f)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                break;
            }
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        // Возобновление времени и деактивация панели диалога
        Time.timeScale = 1f;
        dialoguePanel.SetActive(false);

        // Деактивация объекта Витя
        if (vitya != null)
        {
            vitya.SetActive(false);
        }

        isTimeFrozen = false;
    }
}