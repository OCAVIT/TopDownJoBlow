using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TriggerExitHandler : MonoBehaviour
{
    public GameObject playerManager;
    public GameObject playerManagerOUT;
    public GameObject outdoor; // Новый объект, который нужно включить
    public Image blackPanelImage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeInAndSwitchManagers());
        }
    }

    private IEnumerator FadeInAndSwitchManagers()
    {
        // Fade In
        yield return StartCoroutine(FadeImage(blackPanelImage, 0f, 1f, 2f));

        // Включение объекта Outdoor
        outdoor.SetActive(true);

        // Switch Player Managers
        playerManager.SetActive(false);
        playerManagerOUT.SetActive(true);

        // Fade Out
        yield return StartCoroutine(FadeImage(blackPanelImage, 1f, 0f, 2f));
    }

    private IEnumerator FadeImage(Image image, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        Color color = image.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            color.a = alpha;
            image.color = color;
            yield return null;
        }

        // Ensure the final alpha is set
        color.a = endAlpha;
        image.color = color;
    }
}