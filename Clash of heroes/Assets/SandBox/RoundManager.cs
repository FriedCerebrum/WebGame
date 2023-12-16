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

    public GameObject statisticsPanel; // ������ �� �����������
    public Text countdownText; // ����� ��� ����������� �������
    private float countdownTimer = 5f; // ����� �������
    private bool isGameEnding = false; // ���� ���������� ����

    public Text player1ScoreText; // ����� ��� ����������� ����� ������� ������. �� ������ ���������.
    public Text player2ScoreText; // ����� ��� ����������� ����� ������� ������. �� ������ ��������
    public Text overAllRoundsText;  //����� ��� ������ ����� �������  

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

        // ��������� �������� ���������� ������
        //localPlayerNickname = PhotonNetwork.LocalPlayer.NickName;

        if (PhotonNetwork.IsMasterClient)
        {
            StartNewRound();             //���� ����� �������� ������ ������. �� � ���� ������ �� slave.
        }
    }
    [PunRPC]
    public void StartNewRound()                       // ����� ������ � �������
    {
        playerSpawnManager.RemoveAllPlayers();
        //playerSpawnManager.RemovePlayerobjects();
        if (currentRound >= totalRounds)
        {
            photonView.RPC("EndGame", RpcTarget.All);
            return;
        }
        playerSpawnManager.SpawnPlayers();               // ����� ������ � �������
    }
    [PunRPC]
    private void EndGame()
    {
        SceneManager.LoadScene("Lobby(Local)");
    }





    //    private string DetermineRoundWinner()
    //    {
    //        // ������ ����������� ���������� ������
    //        string winnerNickname = "WinnerNickname"; // �������� �� �������� ������ ��� ��������� �������� ����������

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
    public void AddtoRoundCounter()         //���������� � ���� ��������
    {
        currentRound += 1;
        overAllRoundsText.text = currentRound.ToString();
    }



}