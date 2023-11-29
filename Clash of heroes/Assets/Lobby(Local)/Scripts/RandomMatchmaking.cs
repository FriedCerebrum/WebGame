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
        PhotonNetwork.ConnectUsingSettings(); // Подключаемся к Photon
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
            playerID = auth.CurrentUser.UserId; // Получаем идентификатор пользователя
        }
    }

    public void JoinRandomGame()
    {
        PhotonNetwork.JoinRandomRoom(); // Пытаемся присоединиться к случайной комнате
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 4 });
    }

    public override void OnJoinedRoom()
    {
        // Здесь можно переключиться на игровую сцену
        Debug.Log("Joined to room. Player ID: " + playerID);
        PhotonNetwork.LoadLevel("SampleScene");
    }
}
