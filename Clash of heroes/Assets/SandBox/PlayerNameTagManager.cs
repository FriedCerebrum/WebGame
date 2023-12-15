using UnityEngine;
using Photon.Pun;

public class PlayerNameTagManager : MonoBehaviour
{
    public GameObject localPlayerNameTag; // ������ ������� ��� ���������� ������
    public GameObject remotePlayerNameTag; // ������ ������� ��� ��������� ������

    // ���������� ��� �������� ������ �� �����
    public void AssignNameTag(GameObject player)
    {
        PhotonView photonView = player.GetComponent<PhotonView>();
        GameObject nameTagPrefab = photonView.IsMine ? localPlayerNameTag : remotePlayerNameTag;

        if (nameTagPrefab != null)
        {
            GameObject nameTag = Instantiate(nameTagPrefab, player.transform.position, Quaternion.identity);
            nameTag.transform.SetParent(player.transform); // ������ ������� �������� �������� ������
            nameTag.transform.localPosition = new Vector3(0, 2, 0); // �������� ��� ������� ������
        }
    }
}
