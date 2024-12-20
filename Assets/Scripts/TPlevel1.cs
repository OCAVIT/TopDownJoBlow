using UnityEngine;
using System.Collections;

public class TriggerFade : MonoBehaviour
{
    public CanvasGroup blackPanel;
    public GameObject TaskText;
    public GameObject LOCO1;
    public GameObject LOCO2;
    public float fadeDuration = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TaskText.SetActive(false);
            StartCoroutine(Fade());
        }
    }

    private IEnumerator Fade()
    {
        // Выполняем FadeIn
        yield return StartCoroutine(FadeCanvasGroup(blackPanel, 0f, 1f, fadeDuration));

        // Деактивируем LOCO1 и активируем LOCO2
        LOCO1.SetActive(false);
        LOCO2.SetActive(true);

        // Выполняем FadeOut
        yield return StartCoroutine(FadeCanvasGroup(blackPanel, 1f, 0f, fadeDuration));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float start, float end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = end;
    }
}