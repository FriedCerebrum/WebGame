using UnityEngine;
using Photon.Pun;

public class PlayerNameTagManager : MonoBehaviour
{
    public GameObject localPlayerNameTag; // Префаб надписи для локального игрока
    public GameObject remotePlayerNameTag; // Префаб надписи для удалённого игрока

    // Вызывается при создании игрока на сцене
    public void AssignNameTag(GameObject player)
    {
        PhotonView photonView = player.GetComponent<PhotonView>();
        GameObject nameTagPrefab = photonView.IsMine ? localPlayerNameTag : remotePlayerNameTag;

        if (nameTagPrefab != null)
        {
            GameObject nameTag = Instantiate(nameTagPrefab, player.transform.position, Quaternion.identity);
            nameTag.transform.SetParent(player.transform); // Делаем надпись дочерним объектом игрока
            nameTag.transform.localPosition = new Vector3(0, 2, 0); // Смещение над головой игрока
        }
    }
}
