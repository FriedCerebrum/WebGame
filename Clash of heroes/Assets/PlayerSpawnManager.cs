using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnManager : MonoBehaviourPunCallbacks
{
    public GameObject[] spawnPoints; // ������ ����� ������
    private Entity2 entity2;

    void Start()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("Spawn points array is not initialized or empty!");
            return;
        }

        Debug.Log("PlayerSpawnManager Start() called.");

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Master client detected. Spawning player...");
            SpawnPlayer(); // ������ ������ ������� ���� � ���������� ���������� � ����� ������ ������
        }
        else
        {
            Debug.Log("Non-master client detected. Requesting spawn point...");
            RequestSpawnPoint(); // �������� ������ ����������� ����� ������
        }
    }

    void SpawnPlayer(int spawnIndex = -1)
    {
        Debug.Log("SpawnPlayer() called with spawnIndex: " + spawnIndex);

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("Spawn points array is not initialized or empty!");
            return;
        }

        GameObject spawnPoint;

        if (spawnIndex == -1)
        {
            // ��������� ����� ����� ������ ��� ������ �������
            spawnIndex = Random.Range(0, spawnPoints.Length);
            Debug.Log("Random spawn index selected: " + spawnIndex);
        }

        spawnPoint = spawnPoints[spawnIndex];

        // ���������, �� �������� �� spawnPoint null
        if (spawnPoint == null)
        {
            Debug.LogError("Spawn point at index " + spawnIndex + " is null!");
            return;
        }

        Debug.Log("Spawning player at spawn point index: " + spawnIndex);

        GameObject playerPrefab = Resources.Load<GameObject>("Player 2");
        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab not found in Resources!");
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogError("Spawn point at index " + spawnIndex + " is null!");
            return;
        }

        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.transform.position, Quaternion.identity);

        if (PhotonNetwork.IsMasterClient)
        {
            // ���������� ���������� � ����� ������ ��������� �������
            photonView.RPC("ReceiveSpawnPoint", RpcTarget.Others, spawnIndex);
            Debug.Log("Sent spawn point information to other players.");
        }
    }

    [PunRPC]
    void ReceiveSpawnPoint(int spawnIndex)
    {
        Debug.Log("Received spawn point request. Spawning player at index: " + spawnIndex);
        SpawnPlayer(spawnIndex);
    }

    void RequestSpawnPoint()
    {
        Debug.Log("Requesting spawn point from master client.");
        photonView.RPC("RequestSpawnPointFromMaster", RpcTarget.MasterClient);
    }

    [PunRPC]
    void RequestSpawnPointFromMaster()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // ���������� ���������� � ����� ������ ��������������
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            photonView.RPC("ReceiveSpawnPoint", photonView.Owner, spawnIndex);
            Debug.Log("Sent spawn point information to requesting client.");
        }
    }

    public void RespawnPlayers()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Respawning players for new round.");

            // �������� ������ �������
            var existingPlayers = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in existingPlayers)
            {
                if (player.GetComponent<PhotonView>() && player.GetComponent<PhotonView>().IsMine)
                {
                    PhotonNetwork.Destroy(player);
                }
            }

            // ����� ����� �������
            SpawnPlayer();
        }
        else
        {
            Debug.LogWarning("Non-master client attempted to respawn players.");
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

    public void DeleteObjectsWithTagRemotely(string tag)
    {
        // ��������� ��������� ����� � �������������� RPC
        photonView.RPC("DeleteObjectsWithTagRPC", RpcTarget.All, tag);
    }

    [PunRPC]
    private void DeleteObjectsWithTagRPC(string tag)
    {
        // ����� �� ������ ����������� �������� �������� � ��������� ������
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objectsWithTag)
        {
            Destroy(obj);
        }
    }

}
