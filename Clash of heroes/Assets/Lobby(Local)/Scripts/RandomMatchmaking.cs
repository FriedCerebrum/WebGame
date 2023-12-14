using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class MatchmakingManager : MonoBehaviourPunCallbacks
{
    public GameObject lobbyObject; // ������ �����
    public GameObject SearcherScreenObject; // ��� ��� �������� ������ ����
    public Button readyButton;
    private bool isPlayerReady = false;
    public TextMeshProUGUI nicknameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI lossesText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI winsText;

    public TextMeshProUGUI opponentNicknameText;
    public TextMeshProUGUI opponentLevelText;
    public TextMeshProUGUI opponentLossesText;
    public TextMeshProUGUI opponentMoneyText;
    public TextMeshProUGUI opponentWinsText;
    public event Action<Player, bool> OnPlayerReadyStatusChanged;

    public Text opponentReadyText;

    protected ShowCurrentSelectedCharacter2 opponentChar;


    void Start()
    {

        readyButton.onClick.AddListener(SetPlayerReady);
        readyButton.gameObject.SetActive(false);

        
        Debug.Log("MatchmakingManager Start()");
    }

    void SetLocalPlayerProperties()
    {
        ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable()
    {
        { "Nickname", PlayerPrefs.GetString("Nickname", "N/A") },
        { "Wins", PlayerPrefs.GetInt("Wins", 0) },
        { "Money", PlayerPrefs.GetInt("Money", 0) },
        { "Losses", PlayerPrefs.GetInt("Losses", 0) },
        { "Level", PlayerPrefs.GetInt("Level", 0) }
    };

        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
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
        SetLocalPlayerProperties();
        readyButton.gameObject.SetActive(true); // ���������� ������ ����������

        
        Debug.Log("OnJoinedRoom() - Player Count: " + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // ������������� ������� � ����� �������
            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                if (player != newPlayer)
                {
                    player.SetCustomProperties(player.CustomProperties);
                }
            }
        }

        if (newPlayer != PhotonNetwork.LocalPlayer)
        {
            bool isOpponentReady = newPlayer.CustomProperties.TryGetValue("isReady", out object isReadyValue) && (bool)isReadyValue;
            opponentReadyText.text = isOpponentReady ? "Opponent was ready" : "Opponent was not ready";
        }

        // ���������� UI ��� ���� �������
        UpdateOpponentUI(newPlayer);
    }

    void UpdateOpponentUI(Player targetPlayer)
    {
        if (targetPlayer != PhotonNetwork.LocalPlayer)
        {
            // ���������� UI ��� ���������
            opponentNicknameText.text = targetPlayer.CustomProperties["Nickname"].ToString();
            opponentWinsText.text = "Wins: " + targetPlayer.CustomProperties["Wins"].ToString();
            opponentMoneyText.text = "Money: " + targetPlayer.CustomProperties["Money"].ToString();
            opponentLossesText.text = "Losses: " + targetPlayer.CustomProperties["Losses"].ToString();
            opponentLevelText.text = "Level: " + targetPlayer.CustomProperties["Level"].ToString();
        }
    }

    void SetPlayerReady()
    {
        isPlayerReady = !isPlayerReady;
        readyButton.GetComponentInChildren<Text>().text = isPlayerReady ? "Ready" : "Not Ready";

        // ���������� CustomProperties
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
    {
        { "isReady", isPlayerReady }
    };
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);

        // ��������� ���������� ���� �������
        CheckPlayersReady();

        Debug.Log("SetPlayerReady() - isPlayerReady: " + isPlayerReady);
    }


    [PunRPC]
    void CheckPlayersReady()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                if (!player.CustomProperties.TryGetValue("isReady", out object isReadyValue) || !(bool)isReadyValue)
                {
                    Debug.Log($"[MatchmakingManager] �� ��� ������ ������: {player.NickName} �� �����.");
                    return;
                }
            }

            Debug.Log("[MatchmakingManager] ��� ������ ������. ��������� �����.");
            LoadArena();
        }
        else
        {
            Debug.Log("[MatchmakingManager] � ������� �� ���������� �������.");
        }
    }


    void LoadArena()
    {
        Debug.Log("�������� �����: SampleScene");
        PhotonNetwork.LoadLevel("Sandbox");
    }


    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        // ��������� ����� CheckPlayersReady, ����� ����������� �� ��������� �������
        if (changedProps.ContainsKey("isReady"))
        {
        CheckPlayersReady();
        }

        // ��������� �������� �� null
        if (opponentReadyText != null && targetPlayer != PhotonNetwork.LocalPlayer && changedProps.ContainsKey("isReady"))
        {
            bool isOpponentReady = (bool)changedProps["isReady"];
            opponentReadyText.text = isOpponentReady ? "Opponent was ready" : "Opponent was not ready";
        }

        // ��������� � ��������� UI, ���� ������� ����������������
        if (opponentNicknameText != null && opponentWinsText); // � ������ ��������)
    {
            UpdateOpponentUI(targetPlayer);
        }
    }



    void UpdatePlayerReadyStatus(Player player, bool isReady)
    {
        player.CustomProperties["isReady"] = isReady;
        player.SetCustomProperties(player.CustomProperties);
        OnPlayerReadyStatusChanged?.Invoke(player, isReady);

        Debug.Log($"[MatchmakingManager] �������� ������ ���������� ������ {player.NickName}: {isReady}");
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

    public void LeaveLobby()
    {
        // ���������� �� �������
        PhotonNetwork.LeaveRoom();
        lobbyObject.SetActive(false); 
        SearcherScreenObject.SetActive(true); // ���������� ������ ������ ����

        // ����� ����� �������� �������������� ������

        Debug.Log("�� ����� �� �����");
    }

    public override void OnLeftRoom()
    {
        // ���� ����� ����������, ����� ����� ������� �������� �������
        Debug.Log("�� �������� ������� Photon");
    }


}
