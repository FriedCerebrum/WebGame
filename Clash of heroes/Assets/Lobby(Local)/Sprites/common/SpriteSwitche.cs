using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSwitcher : MonoBehaviour
{
    public Image imageComponent; // ������ �� ��������� Image, �� ������� ����� �������� �������.
    public Sprite[] sprites;     // ������ �������� ��� ������������.
    public float delayBetweenFrames = 0.5f; // �������� ����� �������.

    private int currentIndex = 0;
    private bool isSwitching = false;

    private void Start()
    {
        if (imageComponent == null || sprites.Length == 0)
        {
            Debug.LogError("���������� ��������� ��������� Image � �������� �������.");
            enabled = false; // ��������� ������, ����� �������� ������.
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
