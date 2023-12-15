using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnManager : MonoBehaviourPunCallbacks
{
    //public GameObject[] spawnPoints; // ������ ����� ������
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
        //    SpawnPlayer(); // ������ ������ ������� ���� � ���������� ���������� � ����� ������ ������
        //}
        //else
        //{
        //    Debug.Log("Non-master client detected. Requesting spawn point...");
        //    RequestSpawnPoint(); // �������� ������ ����������� ����� ������
        //}

        view = GetComponent<PhotonView>();

        //if (PhotonNetwork.IsMasterClient)
        //{
        //    DetermineSpawnPoints();
        //}

        if (PhotonNetwork.IsMasterClient)
        {
            SpawnPlayers();              //���� ����� �������� ������ ������. �� � ���� ������ �� slave.
        }


    }

    public void SpawnPlayers()      //���������� ������ �� �������
    {



        //Vector2 randomPosition = new Vector2(Random.Range(minX, minY), Random.Range(maxX, maxY));
        //PhotonNetwork.Instantiate(player.name, randomPosition, Quaternion.identity);                            - ������� ����������.
        //Debug.Log("�� ���������, �����.");



        Transform masterSpawnPoint, slaveSpawnPoint;

        // ����� ��������� ����� ������
        int masterIndex = Random.Range(0, spawnPoints.Length);
        int slaveIndex = (masterIndex + 1) % spawnPoints.Length;

        masterSpawnPoint = spawnPoints[masterIndex];
        slaveSpawnPoint = spawnPoints[slaveIndex];

        // ����� ������-�������
        PhotonNetwork.Instantiate(player.name, masterSpawnPoint.position, Quaternion.identity);
        entity2.ResetCanDie();


        // �������� ������ � ����� ������ slave-�������
        photonView.RPC("SpawnSlavePlayer", RpcTarget.Others, slaveSpawnPoint.position);       // ����� ����� RPC ������� ������� slave ������.

        photonView.RPC("AddtoRoundCounter", RpcTarget.All);   //��������� �� ���� �������� � �������� �������












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

    //public void DeleteObjectsWithTagRemotely(string tag)
    //{
    //    // ��������� ��������� ����� � �������������� RPC
    //    photonView.RPC("DeleteObjectsWithTagRPC", RpcTarget.All, tag);
    //}

    //[PunRPC]
    //private void DeleteObjectsWithTagRPC(string tag)
    //{
    //    // ����� �� ������ ����������� �������� �������� � ��������� ������
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
