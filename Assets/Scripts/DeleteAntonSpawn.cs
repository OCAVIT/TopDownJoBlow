using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearPlayerPrefsOnMenuLoad : MonoBehaviour
{
    // Название сцены, при загрузке которой нужно очистить PlayerPrefs
    private string menuSceneName = "MainMenu";

    void Start()
    {
        // Подписываемся на событие загрузки сцены
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // Отписываемся от события загрузки сцены, чтобы избежать утечек памяти
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Метод, вызываемый при загрузке новой сцены
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Проверяем, загружена ли сцена с именем "Menu"
        if (scene.name == menuSceneName)
        {
            // Очищаем все PlayerPrefs
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("PlayerPrefs очищены при загрузке сцены Menu.");
        }
    }
}