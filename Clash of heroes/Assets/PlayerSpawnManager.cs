using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnManager : MonoBehaviourPunCallbacks
{
    //public GameObject[] spawnPoints; // ������ ����� ������
    private Entity2 entity2;
    PhotonView view;
    public GameObject player;
    public float minX, minY, maxX, maxY;
    public Transform[] spawnPoints;
    void Start()
    {

        view = GetComponent<PhotonView>();
        // ���� ��������� Entity2 �� ���� �� �������

        if (PhotonNetwork.IsMasterClient)
        {
            SpawnPlayers();              //���� ����� �������� ������ ������. �� � ���� ������ �� slave.
        }
    }

    public void SpawnPlayers()      //���������� ������ �� �������
    {
        Transform masterSpawnPoint, slaveSpawnPoint;

        // ����� ��������� ����� ������
        int masterIndex = Random.Range(0, spawnPoints.Length);
        int slaveIndex = (masterIndex + 1) % spawnPoints.Length;

        masterSpawnPoint = spawnPoints[masterIndex];
        slaveSpawnPoint = spawnPoints[slaveIndex];

        // ����� ������-�������
        GameObject masterPlayer = PhotonNetwork.Instantiate(player.name, masterSpawnPoint.position, Quaternion.identity);
        Entity2 masterEntity = masterPlayer.GetComponent<Entity2>();
        if (masterEntity != null)
        {
            masterEntity.ResetCanDie();
        }


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
