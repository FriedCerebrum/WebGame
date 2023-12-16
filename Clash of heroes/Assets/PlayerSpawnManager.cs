using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnManager : MonoBehaviourPunCallbacks
{
    //public GameObject[] spawnPoints; // ������ ����� ������
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

    public void SpawnPlayers()      //���������� ������ �� �������
    {  if (PhotonNetwork.IsMasterClient)
        {
            Transform masterSpawnPoint, slaveSpawnPoint;

            // ����� ��������� ����� ������
            int masterIndex = Random.Range(0, spawnPoints.Length);
            int slaveIndex = (masterIndex + 1) % spawnPoints.Length;

            masterSpawnPoint = spawnPoints[masterIndex];
            slaveSpawnPoint = spawnPoints[slaveIndex];

            // ����� ������-�������
            GameObject masterPlayer = PhotonNetwork.Instantiate(player.name, masterSpawnPoint.position, Quaternion.identity);
            AddMasterPlayer(masterPlayer);
            Entity2 masterEntity = masterPlayer.GetComponent<Entity2>();
            if (masterEntity != null)
            {
                masterEntity.ResetCanDie();
            }


            // �������� ������ � ����� ������ slave-�������
            photonView.RPC("SpawnSlavePlayer", RpcTarget.Others, slaveSpawnPoint.position);       // ����� ����� RPC ������� ������� slave ������.

            photonView.RPC("AddtoRoundCounter", RpcTarget.All);   //��������� �� ���� �������� � �������� �������
        }
        else
        {
            return;
        }
    }


    public void AddMasterPlayer(GameObject playerObject)                          //����� ������ �� ������ ������� �� �����
    {
        if (!playerObjects.Contains(playerObject))
        {
            playerObjects.Add(playerObject);
        }
    }
    [PunRPC]
    public void AddSlavePlayer(GameObject playerObject)                            //����� ������ �� ������ ������� �� Slave
    {
        // �������� ���� ������� � �����
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

        // ���� ����� ��� slave ������ � ��������� ���, ���� �� ������
        foreach (var SlavePlayer in allPlayers)
        {
            PhotonView pv = SlavePlayer.GetComponent<PhotonView>();
            if (pv != null && !pv.IsMine)
            {
                // ���� ����� �� ����������� ������-�������, �������������, �� �������� slave
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

        // ��������� ���������� ���������� ������� � �������
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("Only one player left in the room. Redirecting to the lobby.");
            // �������� ������ �� ���������� 1 ������ � firebase
            LoadLobbyScene();  // �������������� ����������� ������ � �����
        }
    }

    private void LoadLobbyScene()
    {
        // ���������� ������ � �����, ���� ������ ������
        string lobbySceneName = "Lobby(Local)";
        SceneManager.LoadScene(lobbySceneName);
    }

    [PunRPC]
    void SpawnSlavePlayer(Vector3 spawnPosition) // ����� ������ � slave
    {
        GameObject slavePlayer = PhotonNetwork.Instantiate(player.name, spawnPosition, Quaternion.identity);
        string slavePlayerId = slavePlayer.name; // ����������� ��� ��� ������ ���������� �������������

        photonView.RPC("AddSlavePlayer", RpcTarget.MasterClient);

        Entity2 slaveEntity = slavePlayer.GetComponent<Entity2>();
        if (slaveEntity != null)
        {
            slaveEntity.ResetCanDie();
        }
    }

    //���������� ������ �� �������
    //public void RemovePlayerobjects()                     
    //{
    //    //GameObject playerObjectToDelete = player;
    //    //PhotonNetwork.Destroy(playerObjectToDelete);
    //    RemoveAllPlayers();
    //}

    public void RemoveAllPlayers() // ���������� ������ �� �������
    {
        bool isFirstCall = true; // ���������� isFirstCall ���������� ������ ������

        if (isFirstCall)
        {
            isFirstCall = false;
            return; // ������ �� ������ ��� ������ ������
        }

        foreach (var player in new List<GameObject>(playerObjects))
        {
            PhotonNetwork.Destroy(player);
        }
        playerObjects.Clear();
    }


}
