using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    // ���������, ������� ����� �������������
    public AudioClip audioClip;

    // �������� ������� � �������� ����� ��������������
    public float playInterval = 5f;

    // ������������� ��� ������������ ����������
    private AudioSource audioSource;

    // �����, ��������� � ���������� ������������
    private float timeSinceLastPlay;

    void Start()
    {
        // �������� ��� ��������� ��������� AudioSource
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // ������������� ��������� � �������������
        audioSource.clip = audioClip;

        // ����������� ��������� ����� ��� ������
        audioSource.Play();

        // �������������� �����, ��������, ��� ��������� ��� ��� ��������
        timeSinceLastPlay = 0f;
    }

    void Update()
    {
        // ����������� �����, ��������� � ���������� ������������
        timeSinceLastPlay += Time.deltaTime;

        // ���������, ������ �� ���������� ������� ��� ���������� ������������
        if (timeSinceLastPlay >= playInterval)
        {
            // ����������� ���������
            audioSource.Play();

            // ���������� ������� �������
            timeSinceLastPlay = 0f;
        }
    }
}