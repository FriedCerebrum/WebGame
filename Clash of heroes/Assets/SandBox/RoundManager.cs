using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Photon.Pun.Demo.Asteroids;

public class RoundManager : MonoBehaviourPun
{
    public int totalRounds = 3;
    private int currentRound = 0;
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

        if (PhotonNetwork.IsMasterClient)
        {
            StartNewRound();             //Этот метод вызывает ТОЛЬКО мастер. Ни в коем случае не slave.
        }
    }
    [PunRPC]
    public void StartNewRound()                       // ВЫЗОВ ТОЛЬКО У МАСТЕРА
    {
        photonView.RPC("RemovePlayerobjects", RpcTarget.All);
        //playerSpawnManager.RemovePlayerobjects();
        if (currentRound >= totalRounds)
        {
            photonView.RPC("EndGame", RpcTarget.All);
            return;
        }
        playerSpawnManager.SpawnPlayers();               // ВЫЗОВ ТОЛЬКО У МАСТЕРА
    }
    [PunRPC]
    private void EndGame()
    {
        SceneManager.LoadScene("Lobby(Local)");
    }





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


    [PunRPC]
    public void AddtoRoundCounter()         //Вызывается у всех клиентов
    {
        currentRound += 1;
        overAllRoundsText.text = currentRound.ToString();
    }



}