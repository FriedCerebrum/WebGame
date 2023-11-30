using UnityEngine;
using UnityEngine.UI;

public class ShowCurrentSelectedCharacter2 : MonoBehaviour
{
    public Image characterImage;
    public Sprite[] characterSprites;

    // ����� OnEnable ���������� ��� ������ ��������� �������
    void OnEnable()
    {
        string selectedCharacter = PlayerPrefs.GetString("CurrentSelectedCharacter", "DefaultCharacter");
        UpdateCharacterImage(selectedCharacter);
    }

    private void UpdateCharacterImage(string characterName)
    {
        // ��������������, ��� ���� ��������� ������ ��� ������, ����� ��� ��������� �� �������
        Sprite defaultSprite = characterImage.sprite;

        foreach (Sprite sprite in characterSprites)
        {
            if (sprite.name == characterName)
            {
                characterImage.sprite = sprite;
                return; // ����� ������ ������, �� ������� �� �����
            }
        }

        // ���� �������� �� ������, ������������� ��������� ������
        characterImage.sprite = defaultSprite;
    }
}
