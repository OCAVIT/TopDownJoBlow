using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue1 : MonoBehaviour
{
    public GameObject dialogueUI;
    public TMP_Text dialogueText;
    public TMP_Text characterNameText;
    public TMP_Text taskText;
    public string taskTextContent;
    public GameObject gena;
    public List<string> characterNames = new List<string>();
    public List<string> dialogueLines = new List<string>();
    public List<GameObject> characterPortraits = new List<GameObject>();
    public List<float> lineDurations = new List<float>();
    public GameObject blackPanel;

    private int currentLineIndex = 0;
    private bool isDialogueRunning = false;

    void Start()
    {
        Debug.Log("Start: ���������� ���������� ����");
        dialogueUI.SetActive(true);
        StartCoroutine(FadeOut(blackPanel, 4f));
        StartDialogue();
    }

    public void StartDialogue()
    {
        if (!isDialogueRunning)
        {
            Debug.Log("StartDialogue: ������ �������");
            StartCoroutine(RunDialogue());
        }
    }

    private IEnumerator RunDialogue()
    {
        isDialogueRunning = true;
        Time.timeScale = 0f;

        while (currentLineIndex < dialogueLines.Count)
        {
            Debug.Log($"RunDialogue: ����� ������ {currentLineIndex}");
            ShowLine(characterNames[currentLineIndex], dialogueLines[currentLineIndex], characterPortraits[currentLineIndex]);

            float elapsedTime = 0f;
            bool isLineComplete = false;

            while (elapsedTime < lineDurations[currentLineIndex] || !isLineComplete)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (isLineComplete)
                    {
                        Debug.Log("RunDialogue: ������� ������");
                        break;
                    }
                    else
                    {
                        dialogueText.text = dialogueLines[currentLineIndex];
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

    private void EndDialogue()
    {
        Debug.Log("EndDialogue: ���������� �������");
        isDialogueRunning = false;
        Time.timeScale = 1f;

        dialogueUI.SetActive(false);
        DeactivateAllPortraits();

        if (taskText != null)
        {
            taskText.text = taskTextContent;
            taskText.gameObject.SetActive(true);
        }

        if (gena != null)
        {
            gena.SetActive(false);
        }
    }

    private void ShowLine(string characterName, string dialogue, GameObject characterPortrait)
    {
        Debug.Log($"ShowLine: ����� ������ ��� ��������� {characterName}");
        DeactivateAllPortraits();

        if (characterPortrait != null)
        {
            characterPortrait.SetActive(true);
        }

        dialogueText.text = dialogue;
        characterNameText.text = characterName;
        characterNameText.gameObject.SetActive(true);
    }

    private void DeactivateAllPortraits()
    {
        Debug.Log("DeactivateAllPortraits: ����������� ���� ���������");
        foreach (var portrait in characterPortraits)
        {
            if (portrait != null)
            {
                portrait.SetActive(false);
            }
        }
    }
}