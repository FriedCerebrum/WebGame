using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource; // ������ �� ��������� AudioSource ��� ��������������� �����.

    private void Start()
    {
        // ���������, ��� � ��� ���� ��������� AudioSource ������������� � �������.
        if (audioSource == null)
        {
            Debug.LogError("AudioSource �� ���������� � �������. ����������, ���������� ���.");
        }
    }

    // ����� ��� ��������������� �����.
    public void PlaySound()
    {
        if (audioSource != null)
        {
            audioSource.Play(); // ������������� ����.
        }
        else
        {
            Debug.LogError("AudioSource �� ��������. �� ���� ������������� ����.");
        }
    }
}
