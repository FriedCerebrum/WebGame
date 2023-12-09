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
            Debug.Log("������ �������");
        }
        else
        {
            PhotonNetwork.JoinRandomRoom();
            Debug.Log("�������������� � ��������� �������");
        }
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("������ �������� �������");
        CreateOrJoinRoom();
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("������� �������");
    }
    public override void OnConnected()
    {
        Debug.Log("��������������");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("�������� ���������");
        devspawn.Spawn();
    }

}

