using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Firebase;
using Firebase.Database;
using Firebase.Auth;

public class RandomMatchmaking : MonoBehaviourPunCallbacks
{
    private FirebaseAuth auth;
    private string playerID;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // ������������ � Photon
        InitializeFirebase();
    }

    private void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    private void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != null)
        {
            playerID = auth.CurrentUser.UserId; // �������� ������������� ������������
        }
    }

    public void JoinRandomGame()
    {
        PhotonNetwork.JoinRandomRoom(); // �������� �������������� � ��������� �������
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 4 });
    }

    public override void OnJoinedRoom()
    {
        // ����� ����� ������������� �� ������� �����
        Debug.Log("Joined to room. Player ID: " + playerID);
        PhotonNetwork.LoadLevel("SampleScene");
    }
}
