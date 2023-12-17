using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource; // Ссылка на компонент AudioSource для воспроизведения звука.

    private void Start()
    {
        // Проверяем, что у нас есть компонент AudioSource прикрепленный к объекту.
        if (audioSource == null)
        {
            Debug.LogError("AudioSource не прикреплен к объекту. Пожалуйста, прикрепите его.");
        }
    }

    // Метод для воспроизведения звука.
    public void PlaySound()
    {
        if (audioSource != null)
        {
            audioSource.Play(); // Воспроизводим звук.
        }
        else
        {
            Debug.LogError("AudioSource не настроен. Не могу воспроизвести звук.");
        }
    }
}
