using Photon.Pun;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public void SelectCharacter(string characterName)
    {
        PlayerPrefs.SetString("CurrentSelectedCharacter", characterName);
        PlayerPrefs.Save();
        SetSelectedCharacterProperty();
    }

    void SetSelectedCharacterProperty()
    {
        string selectedCharacter = PlayerPrefs.GetString("CurrentSelectedCharacter", "DefaultCharacter");
        ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable()
    {
        { "SelectedCharacter", selectedCharacter }
    };

        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
    }

}
