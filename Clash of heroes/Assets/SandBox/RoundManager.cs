using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class RoundManager : MonoBehaviourPun
{
    public int totalRounds = 3;
    private int currentRound = 0;
    public Text roundText;
    PlayerSpawnManager playerSpawnManager;

    public GameObject statisticsPanel; // Панель со статистикой
    public Text countdownText; // Текст для отображения таймера
    private float countdownTimer = 5f; // Время таймера
    private bool isGameEnding = false; // Флаг завершения игры

    public Text player1ScoreText; // Текст для отображения счета первого игрока. Он всегда локальный.
    public Text player2ScoreText; // Текст для отображения счета второго игрока. Он всегда удалённый
    public Text overAllRoundsText;  //Текст для общего счета раундов  

    private int player1Score;
    private int player2Score;
    private string localPlayerNickname;

    void Start()
    {
        playerSpawnManager = FindObjectOfType<PlayerSpawnManager>();
        if (playerSpawnManager == null)
        {
            Debug.LogError("PlayerSpawnManager not found in the scene!");
        }

        // Установка никнейма локального игрока
        //localPlayerNickname = PhotonNetwork.LocalPlayer.NickName;
    }
    [PunRPC]
    public void StartNewRound()                       // ВЫЗОВ ТОЛЬКО У МАСТЕРА
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        else if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("RemovePlayerobjects", RpcTarget.All);
            //playerSpawnManager.RemovePlayerobjects();
        }




        //if (currentRound >= totalRounds)
        //{
        //    EndGame();
        //    return;
        //}
        playerSpawnManager.SpawnPlayers();               // ВЫЗОВ ТОЛЬКО У МАСТЕРА

        //        ResetPlayers();

        //        // Определение победителя раунда и обновление счета
        //        string winnerNickname = DetermineRoundWinner();
        //        UpdatePlayerScore(winnerNickname);

        //        currentRound++;
        //        Debug.Log("Starting Round: " + currentRound);
        //        roundText.text = "Round " + currentRound.ToString();

        //        if (playerSpawnManager != null)
        //        {
        //            playerSpawnManager.RespawnPlayers();
        //        }
        //    }

        //    private string DetermineRoundWinner()
        //    {
        //        // Логика определения победителя раунда
        //        string winnerNickname = "WinnerNickname"; // Замените на реальную логику для получения никнейма победителя

        //        return winnerNickname;
        //    }

        //    private void UpdatePlayerScore(string playerNickname)
        //    {
        //        if (!playerScores.ContainsKey(playerNickname))
        //        {
        //            playerScores[playerNickname] = 0;
        //        }
        //        playerScores[playerNickname]++;
        //    }

        //    private void EndGame()
        //    {
        //        Debug.Log("Game Over. Total Rounds Played: " + currentRound);
        //        DisplayFinalStatistics();
        //        statisticsPanel.SetActive(true);
        //        isGameEnding = true;
        //    }

        //    private void DisplayFinalStatistics()
        //    {
        //        foreach (var playerScore in playerScores)
        //        {
        //            if (playerScore.Key == localPlayerNickname)
        //            {
        //                player1ScoreText.text = "Player 1 Score: " + playerScore.Value.ToString();
        //            }
        //            else
        //            {
        //                player2ScoreText.text = playerScore.Key + " Score: " + playerScore.Value.ToString();
        //            }
        //        }
        //    }

        //    void Update()
        //    {
        //        if (isGameEnding)
        //        {
        //            if (countdownTimer > 0)
        //            {
        //                countdownTimer -= Time.deltaTime;
        //                countdownText.text = "Time left: " + Mathf.Ceil(countdownTimer).ToString();
        //            }
        //            else
        //            {
        //                SceneManager.LoadScene("Lobby(Local)");
        //                isGameEnding = false;
        //            }
        //        }
        //    }

        //    public void ResetPlayers()
        //    {
        //        playerSpawnManager.DeleteObjectsWithTagRemotely("Player");

        //        var players = FindObjectsOfType<Entity2>();
        //        foreach (var player in players)
        //        {
        //            player.ResetCanDie();
        //        }
        //    }
    }

    [PunRPC]
    public void AddtoRoundCounter()
    {
        currentRound += 1;
    }



}