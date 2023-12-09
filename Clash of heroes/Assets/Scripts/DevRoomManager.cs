using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DevRoomManager : MonoBehaviourPunCallbacks
{
    public DevSpawn devspawn;
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server");
        CreateOrJoinRoom();
    }
    private void CreateOrJoinRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        if (PhotonNetwork.CountOfRooms == 0)
        {
            PhotonNetwork.CreateRoom("MyRoom", roomOptions);
        }
        else
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join a room, creating a new one.");
        CreateOrJoinRoom();
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("Комната создана");
        devspawn.Spawn();
    }
}

