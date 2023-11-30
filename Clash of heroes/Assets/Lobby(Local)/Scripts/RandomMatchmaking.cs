using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MatchmakingManager : MonoBehaviourPunCallbacks
{
    public GameObject lobbyObject; // Объект лобби
    public GameObject SearcherScreenObject; // там где кнопочка поиска игры
    public Button readyButton;
    private bool isPlayerReady = false;
    public TextMeshProUGUI nicknameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI lossesText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI winsText;

    void Start()
    {
        readyButton.onClick.AddListener(SetPlayerReady);
        readyButton.gameObject.SetActive(false);

        
        Debug.Log("MatchmakingManager Start()");
    }

    public void FindGame()
    {
        PhotonNetwork.JoinRandomRoom();
        ShowLobby();

        
        Debug.Log("FindGame() called");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions options = new RoomOptions { MaxPlayers = 2 };
        PhotonNetwork.CreateRoom(null, options);

        
        Debug.LogWarning("OnJoinRandomFailed - Creating a new room");
    }

    public override void OnJoinedRoom()
    {
        readyButton.gameObject.SetActive(true); // Показываем кнопку готовности

        
        Debug.Log("OnJoinedRoom() - Player Count: " + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    void SetPlayerReady()
    {
        isPlayerReady = !isPlayerReady;
        readyButton.GetComponentInChildren<Text>().text = isPlayerReady ? "Не готов" : "Готов";
        photonView.RPC("CheckPlayersReady", RpcTarget.AllBuffered);

       
        Debug.Log("SetPlayerReady() - isPlayerReady: " + isPlayerReady);
    }

    [PunRPC]
    void CheckPlayersReady()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                if (!(bool)player.CustomProperties["isReady"])
                {
                    return;
                }
            }

            LoadArena();
        }

        
        Debug.Log("CheckPlayersReady() called");
    }

    void LoadArena()
    {
        PhotonNetwork.LoadLevel("ArenaScene"); // Загружаем сцену арены

       
        Debug.Log("LoadArena() called");
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        CheckPlayersReady();

        
        Debug.Log("OnPlayerPropertiesUpdate() called");
    }

    void ShowLobby()
    {
        lobbyObject.SetActive(true);
        SearcherScreenObject.SetActive(false);
        nicknameText.text = PlayerPrefs.GetString("Nickname", "N/A");
        winsText.text = ("Wins: " + PlayerPrefs.GetInt("Wins", 0).ToString());
        moneyText.text = ("Money: " + PlayerPrefs.GetInt("Money", 0).ToString());
        lossesText.text = ("Losses: " +PlayerPrefs.GetInt("Losses", 0).ToString());
        levelText.text =("Level: " + PlayerPrefs.GetInt("Level", 0).ToString());


        Debug.Log("ShowLobby() called");
    }

    void HideLobby()
    {
        lobbyObject.SetActive(false);
        SearcherScreenObject.SetActive(true);

        
        Debug.Log("HideLobby() called");
    }
}
