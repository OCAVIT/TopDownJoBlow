using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSystem2 : MonoBehaviour
{
    public GameObject dialogueUI;
    public TMP_Text dialogueText;
    public TMP_Text characterNameText;
    public TMP_Text taskText;
    public string taskTextContent;
    public List<string> characterNames = new List<string>();
    public List<string> dialogueLines = new List<string>();
    public List<GameObject> characterPortraits = new List<GameObject>();
    public List<float> lineDurations = new List<float>();

    private int currentLineIndex = 0;
    private bool isDialogueRunning = false;

    void Start()
    {
        Debug.Log("Start: Диалоговая система инициализирована");
    }

    public void StartDialogue()
    {
        if (!isDialogueRunning)
        {
            Debug.Log("StartDialogue: Запуск диалога");
            dialogueUI.SetActive(true);
            if (taskText != null)
            {
                taskText.gameObject.SetActive(false);
            }
            StartCoroutine(RunDialogue());
        }
    }

    private IEnumerator RunDialogue()
    {
        isDialogueRunning = true;
        Time.timeScale = 0f;

        while (currentLineIndex < dialogueLines.Count)
        {
            Debug.Log($"RunDialogue: Показ строки {currentLineIndex}");
            ShowLine(characterNames[currentLineIndex], dialogueLines[currentLineIndex], characterPortraits[currentLineIndex]);

            float elapsedTime = 0f;
            bool isLineComplete = false;

            while (elapsedTime < lineDurations[currentLineIndex] || !isLineComplete)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (isLineComplete)
                    {
                        Debug.Log("RunDialogue: Пропуск строки");
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

    private void EndDialogue()
    {
        Debug.Log("EndDialogue: Завершение диалога");
        isDialogueRunning = false;
        Time.timeScale = 1f;

        dialogueUI.SetActive(false);
        DeactivateAllPortraits();

        if (taskText != null)
        {
            taskText.text = taskTextContent;
            taskText.gameObject.SetActive(true);
        }
    }

    private void ShowLine(string characterName, string dialogue, GameObject characterPortrait)
    {
        Debug.Log($"ShowLine: Показ строки для персонажа {characterName}");
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
        Debug.Log("DeactivateAllPortraits: Деактивация всех портретов");
        foreach (var portrait in characterPortraits)
        {
            if (portrait != null)
            {
                portrait.SetActive(false);
            }
        }
    }
}