using Photon.Pun;
using UnityEngine;

public class DevSpawn : MonoBehaviour
{
    public GameObject player;
    public float minX, minY, maxX, maxY;
    public void Spawn()
    {
        Vector2 randomPosition = new Vector2(Random.Range (minX, minY), Random.Range (maxX, maxY));
        PhotonNetwork.Instantiate(player.name, randomPosition, Quaternion.identity);
    }
}
