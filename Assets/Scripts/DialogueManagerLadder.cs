using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManagerLadder : MonoBehaviour
{
    public GameObject dialoguePanell;
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public TMP_Text taskText;

    public List<float> dialogueDurations = new List<float>();
    public List<string> dialogueLines = new List<string>();
    public List<string> characterNames = new List<string>();
    public List<GameObject> characterImages = new List<GameObject>();

    private int currentDialogueIndex = 0;
    private bool isDialogueActive = false;

    public float taskTextBlinkSpeed = 1f;

    void Start()
    {
        DeactivateAllUIElements();
        taskText.gameObject.SetActive(false);
    }

    public void StartDialogue()
    {
        if (!isDialogueActive)
        {
            StartCoroutine(DialogueCoroutine());
        }
    }

    private IEnumerator DialogueCoroutine()
    {
        dialoguePanell.SetActive(true);
        isDialogueActive = true;
        Time.timeScale = 0f;

        taskText.gameObject.SetActive(false);

        while (currentDialogueIndex < dialogueLines.Count)
        {
            ShowDialogue(characterImages[currentDialogueIndex], characterNames[currentDialogueIndex], dialogueLines[currentDialogueIndex]);

            float elapsedTime = 0f;
            bool isLineFullyDisplayed = false;

            while (elapsedTime < dialogueDurations[currentDialogueIndex] || !isLineFullyDisplayed)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (isLineFullyDisplayed)
                    {
                        currentDialogueIndex++;
                        break;
                    }
                    else
                    {
                        dialogueText.text = dialogueLines[currentDialogueIndex];
                        isLineFullyDisplayed = true;
                    }
                }

                if (!isLineFullyDisplayed)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                }

                yield return null;
            }
        }

        DeactivateAllUIElements();
        isDialogueActive = false;
        Time.timeScale = 1f;

        taskText.text = "Задача: выйдите на улицу";
        taskText.gameObject.SetActive(true);
        StartCoroutine(BlinkTaskText());
    }

    private void ShowDialogue(GameObject characterImage, string characterName, string dialogue)
    {
        DeactivateAllCharacterImages();

        if (characterImage != null)
        {
            characterImage.SetActive(true);
        }

        dialogueText.text = dialogue;
        nameText.text = characterName;
        nameText.gameObject.SetActive(true);
    }

    private void DeactivateAllUIElements()
    {
        dialoguePanell.SetActive(false);
        DeactivateAllCharacterImages();
        nameText.gameObject.SetActive(false);
    }

    private void DeactivateAllCharacterImages()
    {
        foreach (var image in characterImages)
        {
            if (image != null)
            {
                image.SetActive(false);
            }
        }
    }

    private IEnumerator BlinkTaskText()
    {
        while (true)
        {
            float alpha = Mathf.PingPong(Time.time * taskTextBlinkSpeed, 1f);
            taskText.color = new Color(taskText.color.r, taskText.color.g, taskText.color.b, alpha);
            yield return null;
        }
    }
}