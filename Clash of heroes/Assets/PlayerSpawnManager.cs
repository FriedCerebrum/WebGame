using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviourPun
{
    public GameObject[] spawnPoints; // ������ ����� ������

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnPlayer(); // ������ ������ ������� ���� � ���������� ���������� � ����� ������ ������
        }
        else
        {
            RequestSpawnPoint(); // �������� ������ ����������� ����� ������
        }
    }

    void SpawnPlayer(int spawnIndex = -1)
    {
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
        }

        spawnPoint = spawnPoints[spawnIndex];

        // ���������, �� �������� �� spawnPoint null
        if (spawnPoint == null)
        {
            Debug.LogError("Spawn point at index " + spawnIndex + " is null!");
            return;
        }

        GameObject playerPrefab = Resources.Load<GameObject>("Player 2");
        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab not found in Resources!");
            return;
        }

        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.transform.position, Quaternion.identity);

        if (PhotonNetwork.IsMasterClient)
        {
            // ���������� ���������� � ����� ������ ��������� �������
            photonView.RPC("ReceiveSpawnPoint", RpcTarget.Others, spawnIndex);
        }
    }


    [PunRPC]
    void ReceiveSpawnPoint(int spawnIndex)
    {
        SpawnPlayer(spawnIndex);
    }

    void RequestSpawnPoint()
    {
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
        }
    }
}
