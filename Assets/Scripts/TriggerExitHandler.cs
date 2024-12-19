using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TriggerExitHandler : MonoBehaviour
{
    public GameObject playerManager;
    public GameObject playerManagerOUT;
    public GameObject outdoor;
    public Image blackPanelImage;

    public TMP_Text taskText;
    public string newTaskText;

    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = blackPanelImage.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = blackPanelImage.gameObject.AddComponent<CanvasGroup>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeInAndSwitchManagers());
        }
    }

    private IEnumerator FadeInAndSwitchManagers()
    {
        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, 0f, 1f, 2f));

        outdoor.SetActive(true);

        playerManager.SetActive(false);
        playerManagerOUT.SetActive(true);

        UpdateTaskText();

        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, 1f, 0f, 2f));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            canvasGroup.alpha = alpha;
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }

    private void UpdateTaskText()
    {
        if (taskText != null)
        {
            taskText.text = newTaskText;
        }
    }
}