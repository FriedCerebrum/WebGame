using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public void SelectCharacter(string characterName)
    {
        PlayerPrefs.SetString("CurrentSelectedCharacter", characterName);
        PlayerPrefs.Save();
    }
}
