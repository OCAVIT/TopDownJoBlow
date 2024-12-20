using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearPlayerPrefsOnMenuLoad : MonoBehaviour
{
    // �������� �����, ��� �������� ������� ����� �������� PlayerPrefs
    private string menuSceneName = "MainMenu";

    void Start()
    {
        // ������������� �� ������� �������� �����
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // ������������ �� ������� �������� �����, ����� �������� ������ ������
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // �����, ���������� ��� �������� ����� �����
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���������, ��������� �� ����� � ������ "Menu"
        if (scene.name == menuSceneName)
        {
            // ������� ��� PlayerPrefs
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("PlayerPrefs ������� ��� �������� ����� Menu.");
        }
    }
}