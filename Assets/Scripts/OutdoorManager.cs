using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public GameObject dialogueUI;
    public TMP_Text dialogueContent;
    public TMP_Text characterNameText;
    public List<string> characterNames = new List<string>();
    public List<string> dialogueLines = new List<string>();
    public List<GameObject> characterPortraits = new List<GameObject>();
    public List<float> lineDurations = new List<float>();
    public GameObject blackPanelUII; // Ссылка на BlackPanelUII
    public GameObject canalisation; // Ссылка на Canalisation
    public GameObject playerManagerOUT; // Ссылка на PlayerManagerOUT
    public GameObject animationCamera; // Ссылка на объект AnimationCamera
    private Animator animationCameraAnimator; // Ссылка на аниматор камеры
    public GameObject canvas; // Ссылка на Canvas
    public List<string> characterNamesPart2 = new List<string>();
    public List<string> dialogueLinesPart2 = new List<string>();
    public List<GameObject> characterPortraitsPart2 = new List<GameObject>();
    public List<float> lineDurationsPart2 = new List<float>();

    private int currentLineIndex = 0;
    private bool isDialogueRunning = false;

    void Start()
    {
        HideAllUIElements();
        if (animationCamera != null)
        {
            animationCameraAnimator = animationCamera.GetComponent<Animator>();
        }
    }

    public void BeginDialogue()
    {
        if (!isDialogueRunning)
        {
            StartCoroutine(RunDialogue());
        }
    }

    private IEnumerator RunDialogue()
    {
        dialogueUI.SetActive(true);
        isDialogueRunning = true;
        Time.timeScale = 0f;

        // Первая часть диалога
        while (currentLineIndex < dialogueLines.Count)
        {
            DisplayLine(characterNames[currentLineIndex], dialogueLines[currentLineIndex], characterPortraits[currentLineIndex]);

            float elapsedTime = 0f;
            bool isLineComplete = false;

            while (elapsedTime < lineDurations[currentLineIndex] || !isLineComplete)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (isLineComplete)
                    {
                        break;
                    }
                    else
                    {
                        dialogueContent.text = dialogueLines[currentLineIndex];
                        isLineComplete = true;
                    }
                }

                if (!isLineComplete)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                    if (elapsedTime >= lineDurations[currentLineIndex])
                    {
                        isLineComplete = true;
                    }
                }

                yield return null;
            }

            currentLineIndex++;
        }

        // Завершаем первую часть диалога и запускаем анимацию
        EndDialogue();

        // Ожидаем завершения анимации
        yield return new WaitForSeconds(animationCameraAnimator.GetCurrentAnimatorStateInfo(0).length);

        // Активируем Canalisation
        if (canalisation != null)
        {
            canalisation.SetActive(true);
        }

        // Вторая часть диалога
        currentLineIndex = 0; // Сброс индекса для второй части
        dialogueUI.SetActive(true); // Активируем DialogueUI для второй части
        while (currentLineIndex < dialogueLinesPart2.Count)
        {
            DisplayLine(characterNamesPart2[currentLineIndex], dialogueLinesPart2[currentLineIndex], characterPortraitsPart2[currentLineIndex]);

            float elapsedTime = 0f;
            bool isLineComplete = false;

            while (elapsedTime < lineDurationsPart2[currentLineIndex] || !isLineComplete)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (isLineComplete)
                    {
                        break;
                    }
                    else
                    {
                        dialogueContent.text = dialogueLinesPart2[currentLineIndex];
                        isLineComplete = true;
                    }
                }

                if (!isLineComplete)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                    if (elapsedTime >= lineDurationsPart2[currentLineIndex])
                    {
                        isLineComplete = true;
                    }
                }

                yield return null;
            }

            currentLineIndex++;
        }

        // Завершаем вторую часть диалога
        EndDialogue();
    }

    private IEnumerator FadeOut(GameObject panel, float duration)
    {
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = panel.AddComponent<CanvasGroup>();
        }

        float startAlpha = canvasGroup.alpha;
        float rate = 1.0f / duration;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, progress);
            progress += rate * Time.unscaledDeltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0;
    }

    private IEnumerator FadeIn(GameObject panel, float duration)
    {
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = panel.AddComponent<CanvasGroup>();
        }

        float startAlpha = canvasGroup.alpha;
        float rate = 1.0f / duration;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1, progress);
            progress += rate * Time.unscaledDeltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1;
    }

    private void EndDialogue()
    {
        HideAllUIElements();
        isDialogueRunning = false;
        Time.timeScale = 1f;

        // Деактивируем Canvas и PlayerManagerOUT
        if (canvas != null)
        {
            canvas.SetActive(false);
        }
        if (playerManagerOUT != null)
        {
            playerManagerOUT.SetActive(false);
        }

        // Активируем AnimationCamera и устанавливаем параметр Start в true
        if (animationCamera != null)
        {
            animationCamera.SetActive(true);
            if (animationCameraAnimator != null)
            {
                animationCameraAnimator.SetBool("Start", true);
            }
        }

        // Запускаем корутину для активации Canvas после завершения анимации
        StartCoroutine(ReactivateCanvasAfterAnimation());
    }

    private IEnumerator ReactivateCanvasAfterAnimation()
    {
        // Ожидаем завершения анимации
        yield return new WaitForSeconds(animationCameraAnimator.GetCurrentAnimatorStateInfo(0).length - 1f);

        // Запускаем FadeOut
        if (blackPanelUII != null)
        {
            yield return StartCoroutine(FadeOut(blackPanelUII, 1f));
        }

        // Активируем Canalisation
        if (canalisation != null)
        {
            canalisation.SetActive(true);
        }

        // Запускаем FadeIn
        if (blackPanelUII != null)
        {
            yield return StartCoroutine(FadeIn(blackPanelUII, 1f));
        }

        // Реактивируем Canvas
        if (canvas != null)
        {
            canvas.SetActive(true);
        }
    }

    private void DisplayLine(string characterName, string dialogue, GameObject characterPortrait)
    {
        DeactivateAllPortraits();

        if (characterPortrait != null)
        {
            characterPortrait.SetActive(true);
        }

        dialogueContent.text = dialogue;
        characterNameText.text = characterName;
        characterNameText.gameObject.SetActive(true);
    }

    private void HideAllUIElements()
    {
        dialogueUI.SetActive(false);
        DeactivateAllPortraits();
        characterNameText.gameObject.SetActive(false);
    }

    private void DeactivateAllPortraits()
    {
        foreach (var portrait in characterPortraits)
        {
            if (portrait != null)
            {
                portrait.SetActive(false);
            }
        }
        foreach (var portrait in characterPortraitsPart2)
        {
            if (portrait != null)
            {
                portrait.SetActive(false);
            }
        }
    }
}