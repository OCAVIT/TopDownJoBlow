using UnityEngine;
using TMPro;
using System.Collections;

public class TimeFreezeTrigger : MonoBehaviour
{
    public GameObject dialoguePanel;
    public GameObject NMT;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public GameObject vitya;

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
        Time.timeScale = 0f;
        isTimeFrozen = true;

        NMT.SetActive(true);
        dialoguePanel.SetActive(true);
        nameText.text = "Витя";
        dialogueText.text = "Червянутая идея покидать двор...";

        if (vitya != null)
        {
            vitya.SetActive(true);
        }

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

        Time.timeScale = 1f;
        dialoguePanel.SetActive(false);

        if (vitya != null)
        {
            vitya.SetActive(false);
        }

        isTimeFrozen = false;
    }
}