using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueSystem3 : MonoBehaviour
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
    public GameObject Gena;
    public GameObject WeaponPanel;
    public Button ButtonShot;
    public Button ButtonRifle;
    public Button ButtonStorm;
    public GameObject Rifle;
    public GameObject Shotgun;
    public GameObject StormRifle;
    public GameObject Ammo;
    public GameObject Weapon;
    public GameObject Buffs;
    public GameObject BigDoorsBefore;
    public GameObject BigDoorsAfter;

    private int currentLineIndex = 0;
    private bool isDialogueRunning = false;

    void Start()
    {
        Debug.Log("Start: Диалоговая система инициализирована");

        ButtonShot.onClick.AddListener(() => SelectWeapon(Shotgun));
        ButtonRifle.onClick.AddListener(() => SelectWeapon(Rifle));
        ButtonStorm.onClick.AddListener(() => SelectWeapon(StormRifle));
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

            while (!isLineComplete)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (elapsedTime < lineDurations[currentLineIndex])
                    {
                        dialogueText.text = dialogueLines[currentLineIndex];
                        elapsedTime = lineDurations[currentLineIndex];
                    }
                    else
                    {
                        break;
                    }
                }

                elapsedTime += Time.unscaledDeltaTime;
                if (elapsedTime >= lineDurations[currentLineIndex])
                {
                    isLineComplete = true;
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
        dialogueUI.SetActive(false);
        DeactivateAllPortraits();

        if (Gena != null)
        {
            Gena.SetActive(false);
        }

        if (WeaponPanel != null)
        {
            WeaponPanel.SetActive(true);
            Time.timeScale = 0f;
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

    private void SelectWeapon(GameObject selectedWeapon)
    {
        if (WeaponPanel != null)
        {
            WeaponPanel.SetActive(false);
        }

        if (selectedWeapon != null)
        {
            selectedWeapon.SetActive(true);
        }

        if (taskText != null)
        {
            taskText.text = taskTextContent;
            taskText.gameObject.SetActive(true);
        }

        if (Ammo != null) Ammo.SetActive(true);
        if (Weapon != null) Weapon.SetActive(true);
        if (Buffs != null) Buffs.SetActive(true);

        if (BigDoorsBefore != null) BigDoorsBefore.SetActive(false);
        if (BigDoorsAfter != null) BigDoorsAfter.SetActive(true);

        Time.timeScale = 1f;
    }
}