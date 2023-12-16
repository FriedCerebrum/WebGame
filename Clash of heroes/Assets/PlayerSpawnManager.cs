using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnManager : MonoBehaviourPunCallbacks
{
    //public GameObject[] spawnPoints; // Массив точек спавна
    //private Entity2 entity2;
    PhotonView view;
    RoundManager roundManager;
    public GameObject player;
    public float minX, minY, maxX, maxY;
    public Transform[] spawnPoints;
    void Start()
    {



    }

    public void SpawnPlayers()      //Вызывается только на мастере
    {
        Transform masterSpawnPoint, slaveSpawnPoint;

        // Выбор случайных точек спавна
        int masterIndex = Random.Range(0, spawnPoints.Length);
        int slaveIndex = (masterIndex + 1) % spawnPoints.Length;

        masterSpawnPoint = spawnPoints[masterIndex];
        slaveSpawnPoint = spawnPoints[slaveIndex];

        // Спавн мастер-клиента
        GameObject masterPlayer = PhotonNetwork.Instantiate(player.name, masterSpawnPoint.position, Quaternion.identity);
        Entity2 masterEntity = masterPlayer.GetComponent<Entity2>();
        if (masterEntity != null)
        {
            masterEntity.ResetCanDie();
        }


        // Отправка данных о точке спавна slave-клиенту
        photonView.RPC("SpawnSlavePlayer", RpcTarget.Others, slaveSpawnPoint.position);       // целью этого RPC запроса явлется slave клиент.

        photonView.RPC("AddtoRoundCounter", RpcTarget.All);   //Добавляем на всех клиентах к счётчику единицу

    }


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Player left room: " + otherPlayer.NickName);

        // Проверяем количество оставшихся игроков в комнате
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("Only one player left in the room. Redirecting to the lobby.");
            // написать запрос на добавление 1 победы в firebase
            LoadLobbyScene();  // Перенаправляем оставшегося игрока в лобби
        }
    }

    private void LoadLobbyScene()
    {
        // Выкидываем игрока в лобби, если второй ливнул
        string lobbySceneName = "Lobby(Local)";
        SceneManager.LoadScene(lobbySceneName);
    }

    [PunRPC]
    void SpawnSlavePlayer(Vector3 spawnPosition)                           //Вызов только у slave
    {
        GameObject slavePlayer = PhotonNetwork.Instantiate(player.name, spawnPosition, Quaternion.identity);
        Entity2 slaveEntity = slavePlayer.GetComponent<Entity2>();
        if (slaveEntity != null)
        {
            slaveEntity.ResetCanDie();
        }
    }

    [PunRPC]
    public void RemovePlayerobjects()                     //Вызывается У ВСЕХ в методе начала нового раунда
    {
        GameObject playerObjectToDelete = player;
        PhotonNetwork.Destroy(playerObjectToDelete);
    }

}
