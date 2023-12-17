using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Photon.Realtime;
using Firebase;
using Firebase.Firestore;

public class RoundManager : MonoBehaviourPun
{
    public int totalRounds = 3;
    private int currentRound = 0;
    PlayerSpawnManager playerSpawnManager;

    public GameObject statisticsPanel; // Панель со статистикой

    public Text LocalPlayerScoreText; // Текст для отображения счета первого игрока. Он всегда локальный.
    public Text RemotePlayerScoreText; // Текст для отображения счета второго игрока. Он всегда удалённый
    public Text overAllRoundsText;  //Текст для общего счета раундов  


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

        if (PhotonNetwork.IsMasterClient)
        {
            StartNewRound();             //Этот метод вызывает ТОЛЬКО мастер. Ни в коем случае не slave.
        }
    }
    [PunRPC]
    public void StartNewRound()                       // ВЫЗОВ ТОЛЬКО У МАСТЕРА
    {
        playerSpawnManager.RemoveAllPlayers();
        if (currentRound >= totalRounds)
        {
            photonView.RPC("EndGame", RpcTarget.All);
            return;
        }
        playerSpawnManager.SpawnPlayers();               // ВЫЗОВ ТОЛЬКО У МАСТЕРА
    }
    [PunRPC]
    private void EndGame()                          //Вызывается у всех по окончанию игры
    {
        string playerId = PlayerPrefs.GetString("PlayerId", "defaultPlayerId");
        statisticsPanel.SetActive(true);
        if(LocalPlayerWins>RemotePlayerWins)
        {
            int AddMoney = 100;
            int wins = 1;
            UpdatePlayerField(playerId, "Wins", wins); // Увеличить количество побед на wins
            UpdatePlayerField(playerId, "Money", AddMoney); // Увеличить количество денег на AddMoney

        }
        if(RemotePlayerWins>LocalPlayerWins)
        {
            int AddMoney = 20;
            int losses = 1;
            UpdatePlayerField(playerId, "Losses", losses); // Увеличить количество поражений на losses
            UpdatePlayerField(playerId, "Money", AddMoney); // Увеличить количество денег на AddMoney


        }
        SceneManager.LoadScene("Lobby(Local)");
    }
    [PunRPC]
    public void AddtoRoundCounter()         //Вызывается у всех клиентов
    {
        currentRound += 1;
        overAllRoundsText.text = currentRound.ToString();
    }
    [PunRPC]
    public void AddToRoundWinnerCounter(string Nickname)                  //Вызывается у всех клиентов после смерти игрока.
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
            Debug.Log("Какая то дичь в AddToRoundWinnerCounter");
        }
    }

    public string GetLocalPlayerNickname()
    {
        if (PhotonNetwork.LocalPlayer == null)
        {
            Debug.LogError("Локальный игрок не определен.");
            return null;
        }

        return PhotonNetwork.LocalPlayer.CustomProperties["Nickname"].ToString();
    }

    public string GetRemotePlayerNickname()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            Debug.LogError("Не находимся в комнате.");
            return null;
        }

        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player != PhotonNetwork.LocalPlayer)
            {
                return player.CustomProperties["Nickname"].ToString();
            }
        }

        Debug.LogError("Удаленный игрок не найден.");
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

                // Создаем объект обновления
                Dictionary<string, object> updates = new Dictionary<string, object>
            {
                { fieldName, FieldValue.Increment(incrementValue) } // Увеличиваем значение поля
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





}