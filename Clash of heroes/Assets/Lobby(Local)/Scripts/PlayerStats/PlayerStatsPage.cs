using UnityEngine;
using UnityEngine.UI;

public class PlayerPrefsUI : MonoBehaviour
{
    // Поля для связывания с элементами UI в инспекторе Unity
    public Text levelText;
    public Text lossesText;
    public Text moneyText;
    public Text nicknameText;
    public Text winsText;
    public Text itemsText;

    void Start()
    {
        UpdateUIFromPlayerPrefs();
    }

    public void UpdateUIFromPlayerPrefs()
    {
        // Извлекаем значения из PlayerPrefs и обновляем текст в UI
        if (levelText != null)
            levelText.text =PlayerPrefs.GetInt("Level", 0).ToString();

        if (lossesText != null)
            lossesText.text =PlayerPrefs.GetInt("Losses", 0).ToString();

        if (moneyText != null)
            moneyText.text =PlayerPrefs.GetInt("Money", 0).ToString();

        if (nicknameText != null)
            nicknameText.text =PlayerPrefs.GetString("Nickname", "N/A");

        if (winsText != null)
            winsText.text =PlayerPrefs.GetInt("Wins", 0).ToString();

        if (itemsText != null)
            itemsText.text =PlayerPrefs.GetString("Items", "None");
    }
}