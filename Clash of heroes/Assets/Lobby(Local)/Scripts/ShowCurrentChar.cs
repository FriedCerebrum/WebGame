using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ShowCurrentSelectedCharacter2 : MonoBehaviour
{
    public Image characterImage;
    public Sprite[] characterSprites;
    public Image opponentCharacterImage;

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

    private void UpdateOpponentCharacterImage(string characterName)
    {
        // Предполагается, что есть дефолтный спрайт для случая, когда имя персонажа не найдено
        Sprite defaultSprite = opponentCharacterImage.sprite;

        foreach (Sprite sprite in characterSprites)
        {
            if (sprite.name == characterName)
            {
                opponentCharacterImage.sprite = sprite;
                return; // Найдя нужный спрайт, мы выходим из цикла
            }
        }

        // Если персонаж не найден, устанавливаем дефолтный спрайт
        opponentCharacterImage.sprite = defaultSprite;
    }

    public void ChangeOpponentCharacterImage(string characterName)
    {
        UpdateOpponentCharacterImage(characterName);
    }


}
