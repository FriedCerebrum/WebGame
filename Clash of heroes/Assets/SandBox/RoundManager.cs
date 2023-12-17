using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Photon.Realtime;
using Firebase;
using Firebase.Firestore;
using System.Runtime.CompilerServices;
using UnityEngine.WSA;

public class RoundManager : MonoBehaviourPun
{
    public int totalRounds = 3;
    private int currentRound = 0;
    PlayerSpawnManager playerSpawnManager;


    public Text LocalPlayerScoreText; // ����� ��� ����������� ����� ������� ������. �� ������ ���������.
    public Text RemotePlayerScoreText; // ����� ��� ����������� ����� ������� ������. �� ������ ��������
    public Text overAllRoundsText;  //����� ��� ������ ����� �������
    public Text RemoteNicknameText; //��� �������� �������� ��������� ������

    public Text WinnerWinsCount;
    public Text WinnerMoney;

    public Text LoserWinsCount;
    public Text LoserMoney;

    public GameObject winnerpanel;
    public GameObject loserpanel;

    public bool GameEnded=false;


    private int LocalPlayerWins;
    private int RemotePlayerWins;
    private string RemotePlayerName;          
    private string LocalPlayerName;


    void Start()
    {
        playerSpawnManager = FindObjectOfType<PlayerSpawnManager>();
        if (playerSpawnManager == null)
        {
            Debug.LogError("PlayerSpawnManager not found in the scene!");
        }

        LocalPlayerName = GetLocalPlayerNickname();
        RemotePlayerName = GetRemotePlayerNickname();
        RemoteNicknameText.text = GetRemotePlayerNickname();

        if (PhotonNetwork.IsMasterClient)
        {
            StartNewRound();             //���� ����� �������� ������ ������. �� � ���� ������ �� slave.
        }
    }
    [PunRPC]
    public void StartNewRound()                       // ����� ������ � �������
    {
        playerSpawnManager.RemoveAllPlayers();
        if (currentRound >= totalRounds)
        {
            photonView.RPC("EndGame", RpcTarget.All);
            return;
        }
        playerSpawnManager.SpawnPlayers();               // ����� ������ � �������
    }
    [PunRPC]
    private void EndGame()                          //���������� � ���� �� ��������� ����
    {
        GameEnded = true;

        string playerId = PlayerPrefs.GetString("PlayerId", "defaultPlayerId");
        if(LocalPlayerWins>RemotePlayerWins)
        {
            int AddMoney = 100;
            int wins = 1;
            UpdatePlayerField(playerId, "Wins", wins); // ��������� ���������� ����� �� wins
            UpdatePlayerField(playerId, "Money", AddMoney); // ��������� ���������� ����� �� AddMoney
            DisplayWinnerStats(LocalPlayerWins, AddMoney);


        }
        if(RemotePlayerWins>LocalPlayerWins)
        {
            int AddMoney = 20;
            int losses = 1;
            UpdatePlayerField(playerId, "Losses", losses); // ��������� ���������� ��������� �� losses
            UpdatePlayerField(playerId, "Money", AddMoney); // ��������� ���������� ����� �� AddMoney
            DisplayLoserStats(LocalPlayerWins, AddMoney);

        }
        
    }
    [PunRPC]
    public void AddtoRoundCounter()         //���������� � ���� ��������
    {
        currentRound += 1;
        overAllRoundsText.text = currentRound.ToString();
    }
    [PunRPC]
    public void AddToRoundWinnerCounter(string Nickname)                  //���������� � ���� �������� ����� ������ ������.
    {
        if (Nickname.Equals(LocalPlayerName))
        {
            LocalPlayerWins += 1;
            LocalPlayerScoreText.text = LocalPlayerWins.ToString();

        }
        if (Nickname.Equals(RemotePlayerName))
        {
            RemotePlayerWins += 1;
            RemotePlayerScoreText.text = RemotePlayerWins.ToString();
        }
        else
        {
            Debug.Log("����� �� ���� � AddToRoundWinnerCounter");
        }
    }

    public string GetLocalPlayerNickname()
    {
        if (PhotonNetwork.LocalPlayer == null)
        {
            Debug.LogError("��������� ����� �� ���������.");
            return null;
        }

        return PhotonNetwork.LocalPlayer.CustomProperties["Nickname"].ToString();
    }

    public string GetRemotePlayerNickname()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            Debug.LogError("�� ��������� � �������.");
            return null;
        }

        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player != PhotonNetwork.LocalPlayer)
            {
                return player.CustomProperties["Nickname"].ToString();
            }
        }

        Debug.LogError("��������� ����� �� ������.");
        return null;
    }
    public void UpdatePlayerField(string userId, string fieldName, long incrementValue)
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(dependencyTask =>
        {
            if (dependencyTask.Result == DependencyStatus.Available)
            {
                FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

                DocumentReference docRef = db.Collection("players").Document(userId);

                // ������� ������ ����������
                Dictionary<string, object> updates = new Dictionary<string, object>
            {
                { fieldName, FieldValue.Increment(incrementValue) } // ����������� �������� ����
            };

                docRef.UpdateAsync(updates).ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        Debug.Log($"{fieldName} updated successfully.");
                    }
                    else
                    {
                        Debug.LogError($"Error updating {fieldName}: " + task.Exception);
                    }
                });
            }
            else
            {
                Debug.LogError("Could not resolve Firebase dependencies: " + dependencyTask.Exception);
            }
        });

        
    }

    private void DisplayWinnerStats(int wins, int money)
    {
        winnerpanel.SetActive(true);
        WinnerWinsCount.text=wins.ToString();
        WinnerMoney.text=money.ToString();
    }

    private void DisplayLoserStats(int wins, int money)
    {
        loserpanel.SetActive(true);
        LoserWinsCount.text = wins.ToString();
        LoserMoney.text = money.ToString();
    }

    public void GoToLobby()
    {
       if (PhotonNetwork.IsMasterClient)
        {

            PhotonNetwork.DestroyAll();

        }
        SceneManager.LoadScene("Lobby(Local)");
    }

}