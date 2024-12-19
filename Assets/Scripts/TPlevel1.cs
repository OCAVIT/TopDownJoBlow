using UnityEngine;
using System.Collections;
public class TriggerFadeAndTeleport : MonoBehaviour
{
    public CanvasGroup blackPanel;
    public GameObject playerManager;
    public Transform spawnPoint;
    public float fadeDuration = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeAndTeleport());
        }
    }

    private IEnumerator FadeAndTeleport()
    {
        yield return StartCoroutine(FadeCanvasGroup(blackPanel, 0f, 1f, fadeDuration));

        playerManager.transform.position = spawnPoint.position;

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