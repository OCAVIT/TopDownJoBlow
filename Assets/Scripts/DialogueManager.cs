using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public GameObject imageMom;
    public GameObject imageVitya;
    public GameObject imageDad;
    public GameObject imageRelative;
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public TMP_Text taskText;

    public Image blackPanelImage; // Ссылка на компонент Image BlackPanel

    public float[] dialogueDurations;
    public string[] dialogueLines;
    public string[] characterNames;

    public float blinkSpeed = 1f;

    private int currentDialogueIndex = 0;
    private bool isDialogueActive = false;
    private bool isLineFullyDisplayed = false;

    void Start()
    {
        StartCoroutine(FadeOutBlackPanel()); // Запустите корутину для уменьшения прозрачности

        DeactivateAllUIElements();
        taskText.gameObject.SetActive(false);
        StartCoroutine(StartDialogue());
    }

    IEnumerator FadeOutBlackPanel()
    {
        if (blackPanelImage == null)
        {
            Debug.LogError("BlackPanelImage is not assigned.");
            yield break;
        }

        float fadeDuration = 2f; // Увеличьте время для более плавного эффекта
        float elapsedTime = 0f;
        Color color = blackPanelImage.color;
        color.a = 1f; // Убедитесь, что начальная прозрачность равна 1
        blackPanelImage.color = color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime; // Используйте unscaledDeltaTime
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            blackPanelImage.color = color;
            yield return null;
        }

        color.a = 0f; // Убедитесь, что прозрачность установлена в 0
        blackPanelImage.color = color;
    }

    IEnumerator StartDialogue()
    {
        dialoguePanel.SetActive(true);
        isDialogueActive = true;
        Time.timeScale = 0f;

        while (currentDialogueIndex < dialogueLines.Length)
        {
            ShowDialogue(GetCharacterImage(currentDialogueIndex), characterNames[currentDialogueIndex], dialogueLines[currentDialogueIndex]);
            isLineFullyDisplayed = false;

            float elapsedTime = 0f;

            while (elapsedTime < dialogueDurations[currentDialogueIndex])
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (isLineFullyDisplayed)
                    {
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
                else
                {
                    elapsedTime += Time.unscaledDeltaTime;
                }

                yield return null;
            }

            currentDialogueIndex++;
        }

        DeactivateAllUIElements();
        isDialogueActive = false;
        Time.timeScale = 1f;

        taskText.gameObject.SetActive(true);
        StartCoroutine(BlinkText(taskText));
    }

    void ShowDialogue(GameObject characterImage, string characterName, string dialogue)
    {
        DeactivateAllCharacterImages();

        if (characterImage != null)
        {
            characterImage.SetActive(true);
        }

        dialogueText.text = dialogue;
        nameText.text = characterName;
        nameText.gameObject.SetActive(true);
        isLineFullyDisplayed = true;
    }

    GameObject GetCharacterImage(int index)
    {
        string characterName = characterNames[index];

        if (characterName == "Отец")
        {
            return imageDad;
        }
        else if (characterName == "Витя")
        {
            return imageVitya;
        }
        else if (characterName == "Мама")
        {
            return imageMom;
        }
        else if (characterName == "..." || characterName == "Родственники")
        {
            return null; // Не включать ни одно изображение
        }
        else
        {
            return null; // По умолчанию не включать изображение
        }
    }

    void DeactivateAllUIElements()
    {
        dialoguePanel.SetActive(false);
        DeactivateAllCharacterImages();
        nameText.gameObject.SetActive(false);
    }

    void DeactivateAllCharacterImages()
    {
        imageMom.SetActive(false);
        imageVitya.SetActive(false);
        imageDad.SetActive(false);
        imageRelative.SetActive(false);
    }

    IEnumerator BlinkText(TMP_Text text)
    {
        while (true)
        {
            for (float alpha = 0f; alpha <= 1f; alpha += Time.unscaledDeltaTime * blinkSpeed)
            {
                text.alpha = alpha;
                yield return null;
            }

            for (float alpha = 1f; alpha >= 0f; alpha -= Time.unscaledDeltaTime * blinkSpeed)
            {
                text.alpha = alpha;
                yield return null;
            }
        }
    }
}