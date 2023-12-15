using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnManager : MonoBehaviourPunCallbacks
{
    //public GameObject[] spawnPoints; // Массив точек спавна
    Entity2 entity2;
    PhotonView view;
    public GameObject player;
    public float minX, minY, maxX, maxY;
    public Transform[] spawnPoints;
    void Start()
    {
        //if (spawnPoints == null || spawnPoints.Length == 0)
        //{
        //    Debug.LogError("Spawn points array is not initialized or empty!");
        //    return;
        //}

        //Debug.Log("PlayerSpawnManager Start() called.");

        //if (PhotonNetwork.IsMasterClient)
        //{
        //    Debug.Log("Master client detected. Spawning player...");
        //    SpawnPlayer(); // Мастер клиент спавнит себя и отправляет информацию о точке спавна другим
        //}
        //else
        //{
        //    Debug.Log("Non-master client detected. Requesting spawn point...");
        //    RequestSpawnPoint(); // Немастер клиент запрашивает точку спавна
        //}

        view = GetComponent<PhotonView>();

        //if (PhotonNetwork.IsMasterClient)
        //{
        //    DetermineSpawnPoints();
        //}

        if (PhotonNetwork.IsMasterClient)
        {
            SpawnPlayers();              //Этот метод вызывает ТОЛЬКО мастер. Ни в коем случае не slave.
        }


    }

    public void SpawnPlayers()      //Вызывается только на мастере
    {



        //Vector2 randomPosition = new Vector2(Random.Range(minX, minY), Random.Range(maxX, maxY));
        //PhotonNetwork.Instantiate(player.name, randomPosition, Quaternion.identity);                            - рабочая реализация.
        //Debug.Log("Вы появились, Мисье.");



        Transform masterSpawnPoint, slaveSpawnPoint;

        // Выбор случайных точек спавна
        int masterIndex = Random.Range(0, spawnPoints.Length);
        int slaveIndex = (masterIndex + 1) % spawnPoints.Length;

        masterSpawnPoint = spawnPoints[masterIndex];
        slaveSpawnPoint = spawnPoints[slaveIndex];

        // Спавн мастер-клиента
        PhotonNetwork.Instantiate(player.name, masterSpawnPoint.position, Quaternion.identity);
        entity2.ResetCanDie();


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

    //public void DeleteObjectsWithTagRemotely(string tag)
    //{
    //    // Вызывайте удаленный метод с использованием RPC
    //    photonView.RPC("DeleteObjectsWithTagRPC", RpcTarget.All, tag);
    //}

    //[PunRPC]
    //private void DeleteObjectsWithTagRPC(string tag)
    //{
    //    // Здесь вы можете реализовать удаление объектов с указанной меткой
    //    GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);
    //    foreach (GameObject obj in objectsWithTag)
    //    {
    //        Destroy(obj);
    //    }
    //}


    [PunRPC]
    void SpawnSlavePlayer(Vector3 spawnPosition)
    {
        PhotonNetwork.Instantiate(player.name, spawnPosition, Quaternion.identity);
        entity2.ResetCanDie();
    }

    [PunRPC]
    public void RemovePlayerobjects()
    {
        GameObject playerObjectToDelete = player;
        PhotonNetwork.Destroy(playerObjectToDelete);
    }

}
