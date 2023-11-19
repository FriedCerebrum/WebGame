using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro; // ��������� ������������ ���� ��� TextMeshPro
using UnityEditor.XR;

public class MatchmakingManager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI statusText; // ���� ��� ������ ������

    private void Start()
    {
        // ������������ � Photon �������
        PhotonNetwork.ConnectUsingSettings();
        UpdateStatusText("Connecting to Photon...");
    }

    public override void OnConnectedToMaster()
    {
        // ����������, ����� ������������ � �������� ������� Photon
        UpdateStatusText("Connected to Master Server");
    }

    public void OnPlayButtonPressed()
    {
        // ��������� ����� ��������� �������
        PhotonNetwork.JoinRandomRoom();
        UpdateStatusText("Joining Random Room...");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // ����������, ���� �� ������� ����� �������
        UpdateStatusText("Failed to join a random room. Creating a new room...");

        // ������� ����� �������
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2; // �������� 2 ������ � �������
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public override void OnJoinedRoom()
    {
        // ����������, ����� �� �������������� � �������
        UpdateStatusText("Joined a Room");
    }

    private void UpdateStatusText(string text)
    {
        // ��������� ����� � TextMeshPro
        if (statusText != null)
        {
            statusText.text = text;
        }
        else
        {
            Debug.LogError("Status TextMeshPro is not set!");
        }
    }
}
