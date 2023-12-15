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
    }
    [PunRPC]
    public void StartNewRound()                       // ����� ������ � �������
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
        playerSpawnManager.SpawnPlayers();               // ����� ������ � �������

        //        ResetPlayers();

        //        // ����������� ���������� ������ � ���������� �����
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