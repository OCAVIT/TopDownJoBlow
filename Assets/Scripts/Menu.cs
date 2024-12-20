using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameControl : MonoBehaviour
{
    public Button reloadButton;
    public Button loadButton;
    public Image blackPanel;
    public float fadeDuration = 1.0f; // ������������ ����������

    private void Start()
    {
        // ����������� ������� � �������
        reloadButton.onClick.AddListener(OnReloadButtonClicked);
        loadButton.onClick.AddListener(OnLoadButtonClicked);
    }

    private void OnReloadButtonClicked()
    {
        // ���������� �������� � PlayerPrefs
        PlayerPrefs.Save(); // ��������� ���������
    }

    private void OnLoadButtonClicked()
    {
        // ��������� �������� ��� ���������� ������
        StartCoroutine(FadeInAndLoadScene());
    }

    private IEnumerator FadeInAndLoadScene()
    {
        float elapsedTime = 0f;
        Color panelColor = blackPanel.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            blackPanel.color = new Color(panelColor.r, panelColor.g, panelColor.b, alpha);
            yield return null;
        }

        // ���������, ��� ������ ��������� ����������� � �����
        blackPanel.color = new Color(panelColor.r, panelColor.g, panelColor.b, 1f);

        // ��������� ����� "Prologue"
        SceneManager.LoadScene("Prologue");
    }
}