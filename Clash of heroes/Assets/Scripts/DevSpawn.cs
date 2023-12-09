using Photon.Pun;
using UnityEngine;

public class DevSpawn : MonoBehaviour
{
    public GameObject player;
    public float minX, minY, maxX, maxY;
    PhotonView view;
    private void Start()
    {
        view = GetComponent<PhotonView>();
    }
    public void Spawn()
    {
        Vector2 randomPosition = new Vector2(Random.Range(minX, minY), Random.Range(maxX, maxY));
        PhotonNetwork.Instantiate(player.name, randomPosition, Quaternion.identity);
    }
    public void SpawnOtherPlayer(Photon.Realtime.Player player)
    {
        Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        PhotonNetwork.Instantiate("PlayerMain", randomPosition, Quaternion.identity);
    }
}
