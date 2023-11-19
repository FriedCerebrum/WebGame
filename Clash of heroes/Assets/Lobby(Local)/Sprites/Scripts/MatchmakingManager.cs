using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro; // Добавляем пространство имен для TextMeshPro
using UnityEditor.XR;

public class MatchmakingManager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI statusText; // Поле для вывода текста

    private void Start()
    {
        // Подключаемся к Photon серверу
        PhotonNetwork.ConnectUsingSettings();
        UpdateStatusText("Connecting to Photon...");
    }

    public override void OnConnectedToMaster()
    {
        // Вызывается, когда подключаемся к главному серверу Photon
        UpdateStatusText("Connected to Master Server");
    }

    public void OnPlayButtonPressed()
    {
        // Запускаем поиск случайной комнаты
        PhotonNetwork.JoinRandomRoom();
        UpdateStatusText("Joining Random Room...");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // Вызывается, если не удалось найти комнату
        UpdateStatusText("Failed to join a random room. Creating a new room...");

        // Создаем новую комнату
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2; // Максимум 2 игрока в комнате
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public override void OnJoinedRoom()
    {
        // Вызывается, когда мы присоединились к комнате
        UpdateStatusText("Joined a Room");
    }

    private void UpdateStatusText(string text)
    {
        // Обновляем текст в TextMeshPro
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
