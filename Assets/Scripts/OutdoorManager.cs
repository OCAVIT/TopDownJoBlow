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
    public GameObject blackPanelUII; // ������ �� BlackPanelUII
    public GameObject canalisation; // ������ �� Canalisation
    public GameObject playerManagerOUT; // ������ �� PlayerManagerOUT
    public GameObject animationCamera; // ������ �� ������ AnimationCamera
    private Animator animationCameraAnimator; // ������ �� �������� ������
    public GameObject canvas; // ������ �� Canvas
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

        // ������ ����� �������
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

        // ��������� ������ ����� ������� � ��������� ��������
        EndDialogue();

        // ������� ���������� ��������
        yield return new WaitForSeconds(animationCameraAnimator.GetCurrentAnimatorStateInfo(0).length);

        // ���������� Canalisation
        if (canalisation != null)
        {
            canalisation.SetActive(true);
        }

        // ������ ����� �������
        currentLineIndex = 0; // ����� ������� ��� ������ �����
        dialogueUI.SetActive(true); // ���������� DialogueUI ��� ������ �����
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

        // ��������� ������ ����� �������
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

        // ������������ Canvas � PlayerManagerOUT
        if (canvas != null)
        {
            canvas.SetActive(false);
        }
        if (playerManagerOUT != null)
        {
            playerManagerOUT.SetActive(false);
        }

        // ���������� AnimationCamera � ������������� �������� Start � true
        if (animationCamera != null)
        {
            animationCamera.SetActive(true);
            if (animationCameraAnimator != null)
            {
                animationCameraAnimator.SetBool("Start", true);
            }
        }

        // ��������� �������� ��� ��������� Canvas ����� ���������� ��������
        StartCoroutine(ReactivateCanvasAfterAnimation());
    }

    private IEnumerator ReactivateCanvasAfterAnimation()
    {
        // ������� ���������� ��������
        yield return new WaitForSeconds(animationCameraAnimator.GetCurrentAnimatorStateInfo(0).length - 1f);

        // ��������� FadeOut
        if (blackPanelUII != null)
        {
            yield return StartCoroutine(FadeOut(blackPanelUII, 1f));
        }

        // ���������� Canalisation
        if (canalisation != null)
        {
            canalisation.SetActive(true);
        }

        // ��������� FadeIn
        if (blackPanelUII != null)
        {
            yield return StartCoroutine(FadeIn(blackPanelUII, 1f));
        }

        // ������������ Canvas
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