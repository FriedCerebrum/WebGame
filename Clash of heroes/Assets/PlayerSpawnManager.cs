using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnManager : MonoBehaviourPunCallbacks
{
    //public GameObject[] spawnPoints; // Массив точек спавна
    private Entity2 entity2;
    PhotonView view;
    public GameObject player;
    public float minX, minY, maxX, maxY;
    public Transform[] spawnPoints;
    private Vector3 mySpawnPosition;
    PlayerNameTagManager playerNameTagManager;

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
            SpawnPlayers();
        }


    }

    void SpawnPlayers(int spawnIndex = -1)
    {
        //Debug.Log("SpawnPlayer() called with spawnIndex: " + spawnIndex);

        //if (spawnPoints == null || spawnPoints.Length == 0)
        //{
        //    Debug.LogError("Spawn points array is not initialized or empty!");
        //    return;
        //}

        //GameObject spawnPoint;

        //if (spawnIndex == -1)
        //{
        //    // Рандомный выбор точки спавна для мастер клиента
        //    spawnIndex = Random.Range(0, spawnPoints.Length);
        //    Debug.Log("Random spawn index selected: " + spawnIndex);
        //}

        //spawnPoint = spawnPoints[spawnIndex];

        //// Проверяем, не является ли spawnPoint null
        //if (spawnPoint == null)
        //{
        //    Debug.LogError("Spawn point at index " + spawnIndex + " is null!");
        //    return;
        //}

        //Debug.Log("Spawning player at spawn point index: " + spawnIndex);

        //GameObject playerPrefab = Resources.Load<GameObject>("Player 2");
        //if (playerPrefab == null)
        //{
        //    Debug.LogError("Player prefab not found in Resources!");
        //    return;
        //}

        //if (spawnPoint == null)
        //{
        //    Debug.LogError("Spawn point at index " + spawnIndex + " is null!");
        //    return;
        //}



        //if (PhotonNetwork.IsMasterClient)
        //{
        //    // Отправляем информацию о точке спавна остальным игрокам
        //    photonView.RPC("ReceiveSpawnPoint", RpcTarget.Others, spawnIndex);
        //    Debug.Log("Sent spawn point information to other players.");

        //    PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.transform.position, Quaternion.identity);
        //    Debug.Log("Создали игроков/ка через фотон");
        //}



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
        //PhotonNetwork.Instantiate(player.name, masterSpawnPoint.position, Quaternion.identity);         - рабочая реализация

        GameObject playerInstance = PhotonNetwork.Instantiate(player.name, masterSpawnPoint.position, Quaternion.identity);
        playerNameTagManager.AssignNameTag(playerInstance);

        // Отправка данных о точке спавна slave-клиенту
        photonView.RPC("SpawnSlavePlayer", RpcTarget.Others, slaveSpawnPoint.position);














    }

    //[PunRPC]
    //void ReceiveSpawnPoint(int spawnIndex)
    //{
    //    Debug.Log("Received spawn point request. Spawning player at index: " + spawnIndex);
    //    SpawnPlayer(spawnIndex);
    //}

    //void RequestSpawnPoint()
    //{
    //    Debug.Log("Requesting spawn point from master client.");
    //    photonView.RPC("RequestSpawnPointFromMaster", RpcTarget.MasterClient);
    //}

    //[PunRPC]
    //void RequestSpawnPointFromMaster()
    //{
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        // Отправляем информацию о точке спавна запрашивающему
    //        int spawnIndex = Random.Range(0, spawnPoints.Length);
    //        photonView.RPC("ReceiveSpawnPoint", photonView.Owner, spawnIndex);
    //        Debug.Log("Sent spawn point information to requesting client.");
    //    }
    //}

    //public void RespawnPlayers()
    //{
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        Debug.Log("Respawning players for new round.");

    //        // Удаление старых игроков
    //        var existingPlayers = GameObject.FindGameObjectsWithTag("Player");
    //        foreach (var player in existingPlayers)
    //        {
    //            if (player.GetComponent<PhotonView>() && player.GetComponent<PhotonView>().IsMine)
    //            {
    //                PhotonNetwork.Destroy(player);
    //            }
    //        }

    //        // Спавн новых игроков
    //        SpawnPlayer();
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Non-master client attempted to respawn players.");
    //    }
    //}


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

    //public void DetermineSpawnPoints()
    //{
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        int spawnPointIndex1 = Random.Range(0, spawnPoints.Length);
    //        int spawnPointIndex2;
    //        do
    //        {
    //            spawnPointIndex2 = Random.Range(0, spawnPoints.Length);
    //        } while (spawnPointIndex2 == spawnPointIndex1);

    //        Vector3 spawnPos1 = spawnPoints[spawnPointIndex1].position;
    //        Vector3 spawnPos2 = spawnPoints[spawnPointIndex2].position;

    //        photonView.RPC("SetSpawnPoints", RpcTarget.Others, spawnPos1, spawnPos2);
    //    }
    //}

    //[PunRPC]
    //public virtual void SetSpawnPoints(Vector3 spawnPos1, Vector3 spawnPos2)
    //{
    //    // Здесь можно определить, какой из спавн-поинтов будет использоваться для данного клиента
    //    mySpawnPosition = PhotonNetwork.IsMasterClient ? spawnPos1 : spawnPos2;
    //    SpawnPlayer();
    //}

    //[PunRPC]
    //void BroadcastSpawnPoints(Vector3 spawnPoint1, Vector3 spawnPoint2)
    //{
    //    // Отправить точки спавна всем клиентам
    //    photonView.RPC("ReceiveSpawnPoint", RpcTarget.Others, spawnPoint1, spawnPoint2);
    //}


    [PunRPC]
    void SpawnSlavePlayer(Vector3 spawnPosition)
    {
        GameObject playerInstance = PhotonNetwork.Instantiate(player.name, spawnPosition, Quaternion.identity);
        playerNameTagManager.AssignNameTag(playerInstance);
    }




}
