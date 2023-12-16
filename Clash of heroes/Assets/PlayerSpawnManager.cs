using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
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
    private List<GameObject> playerObjects;
    void Start()
    {

        playerObjects = new List<GameObject>();

    }

    public void SpawnPlayers()      //Вызывается только на мастере
    {  if (PhotonNetwork.IsMasterClient)
        {
            Transform masterSpawnPoint, slaveSpawnPoint;

            // Выбор случайных точек спавна
            int masterIndex = Random.Range(0, spawnPoints.Length);
            int slaveIndex = (masterIndex + 1) % spawnPoints.Length;

            masterSpawnPoint = spawnPoints[masterIndex];
            slaveSpawnPoint = spawnPoints[slaveIndex];

            // Спавн мастер-клиента
            GameObject masterPlayer = PhotonNetwork.Instantiate(player.name, masterSpawnPoint.position, Quaternion.identity);
            AddMasterPlayer(masterPlayer);
            Entity2 masterEntity = masterPlayer.GetComponent<Entity2>();
            if (masterEntity != null)
            {
                masterEntity.ResetCanDie();
            }


            // Отправка данных о точке спавна slave-клиенту
            photonView.RPC("SpawnSlavePlayer", RpcTarget.Others, slaveSpawnPoint.position);       // целью этого RPC запроса явлется slave клиент.

            photonView.RPC("AddtoRoundCounter", RpcTarget.All);   //Добавляем на всех клиентах к счётчику единицу
        }
        else
        {
            return;
        }
    }


    public void AddMasterPlayer(GameObject playerObject)                          //вызов только на мастер клиенте им самим
    {
        if (!playerObjects.Contains(playerObject))
        {
            playerObjects.Add(playerObject);
        }
    }
    [PunRPC]
    public void AddSlavePlayer(GameObject playerObject)                            //Вызов только на мастер клиенте ОТ Slave
    {
        // Получаем всех игроков в сцене
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

        // Ищем среди них slave игрока и добавляем его, если не найден
        foreach (var SlavePlayer in allPlayers)
        {
            PhotonView pv = SlavePlayer.GetComponent<PhotonView>();
            if (pv != null && !pv.IsMine)
            {
                // Этот игрок не принадлежит мастер-клиенту, следовательно, он является slave
                if (!playerObjects.Contains(SlavePlayer))
                {
                    playerObjects.Add(SlavePlayer);
                }
            }
        }
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
    void SpawnSlavePlayer(Vector3 spawnPosition) // Вызов только у slave
    {
        GameObject slavePlayer = PhotonNetwork.Instantiate(player.name, spawnPosition, Quaternion.identity);
        string slavePlayerId = slavePlayer.name; // Используйте имя или другой уникальный идентификатор

        photonView.RPC("AddSlavePlayer", RpcTarget.MasterClient);

        Entity2 slaveEntity = slavePlayer.GetComponent<Entity2>();
        if (slaveEntity != null)
        {
            slaveEntity.ResetCanDie();
        }
    }

    //вызывается только на мастере
    //public void RemovePlayerobjects()                     
    //{
    //    //GameObject playerObjectToDelete = player;
    //    //PhotonNetwork.Destroy(playerObjectToDelete);
    //    RemoveAllPlayers();
    //}

    public void RemoveAllPlayers() // вызывается только на мастере
    {
        bool isFirstCall = true; // Переменная isFirstCall определена внутри метода

        if (isFirstCall)
        {
            isFirstCall = false;
            return; // Ничего не делать при первом вызове
        }

        foreach (var player in new List<GameObject>(playerObjects))
        {
            PhotonNetwork.Destroy(player);
        }
        playerObjects.Clear();
    }


}
