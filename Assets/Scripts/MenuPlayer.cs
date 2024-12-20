using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    // Аудиоклип, который будет проигрываться
    public AudioClip audioClip;

    // Интервал времени в секундах между проигрываниями
    public float playInterval = 5f;

    // Аудиоисточник для проигрывания аудиоклипа
    private AudioSource audioSource;

    // Время, прошедшее с последнего проигрывания
    private float timeSinceLastPlay;

    void Start()
    {
        // Получаем или добавляем компонент AudioSource
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Устанавливаем аудиоклип в аудиоисточник
        audioSource.clip = audioClip;

        // Проигрываем аудиоклип сразу при старте
        audioSource.Play();

        // Инициализируем время, учитывая, что аудиоклип уже был проигран
        timeSinceLastPlay = 0f;
    }

    void Update()
    {
        // Увеличиваем время, прошедшее с последнего проигрывания
        timeSinceLastPlay += Time.deltaTime;

        // Проверяем, прошло ли достаточно времени для следующего проигрывания
        if (timeSinceLastPlay >= playInterval)
        {
            // Проигрываем аудиоклип
            audioSource.Play();

            // Сбрасываем счетчик времени
            timeSinceLastPlay = 0f;
        }
    }
}