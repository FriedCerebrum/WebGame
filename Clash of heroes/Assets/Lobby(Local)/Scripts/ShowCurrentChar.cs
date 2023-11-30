using UnityEngine;
using UnityEngine.UI;

public class ShowCurrentSelectedCharacter2 : MonoBehaviour
{
    public Image characterImage;
    public Sprite[] characterSprites;

    // Метод OnEnable вызывается при каждой активации объекта
    void OnEnable()
    {
        string selectedCharacter = PlayerPrefs.GetString("CurrentSelectedCharacter", "DefaultCharacter");
        UpdateCharacterImage(selectedCharacter);
    }

    private void UpdateCharacterImage(string characterName)
    {
        // Предполагается, что есть дефолтный спрайт для случая, когда имя персонажа не найдено
        Sprite defaultSprite = characterImage.sprite;

        foreach (Sprite sprite in characterSprites)
        {
            if (sprite.name == characterName)
            {
                characterImage.sprite = sprite;
                return; // Найдя нужный спрайт, мы выходим из цикла
            }
        }

        // Если персонаж не найден, устанавливаем дефолтный спрайт
        characterImage.sprite = defaultSprite;
    }
}
