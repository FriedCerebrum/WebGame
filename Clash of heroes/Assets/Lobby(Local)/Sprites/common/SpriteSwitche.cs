using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSwitcher : MonoBehaviour
{
    public Image imageComponent; // Ссылка на компонент Image, на котором будут меняться спрайты.
    public Sprite[] sprites;     // Массив спрайтов для переключения.
    public float delayBetweenFrames = 0.5f; // Задержка между кадрами.

    private int currentIndex = 0;
    private bool isSwitching = false;

    private void Start()
    {
        if (imageComponent == null || sprites.Length == 0)
        {
            Debug.LogError("Необходимо настроить компонент Image и добавить спрайты.");
            enabled = false; // Отключаем скрипт, чтобы избежать ошибок.
            return;
        }

        StartCoroutine(SwitchSprites());
    }

    private IEnumerator SwitchSprites()
    {
        while (true)
        {
            if (!isSwitching)
            {
                imageComponent.sprite = sprites[currentIndex];
                currentIndex = (currentIndex + 1) % sprites.Length;
                isSwitching = true;
                yield return new WaitForSeconds(delayBetweenFrames);
                isSwitching = false;
            }
            yield return null;
        }
    }
}
