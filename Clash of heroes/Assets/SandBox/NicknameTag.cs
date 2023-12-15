using UnityEngine;
using UnityEngine.UI; // ��� TMPro ��� TextMeshPro
using Photon.Pun;

public class NicknameTag : MonoBehaviourPun
{
    public Text localPlayerText; // ����� "You" ��� ���������� ������
    public Text remotePlayerText; // ����� ��� �������� ��������� ������
    public Vector3 offset = new Vector3(0, 2, 0); // �������� ��� ������� ���������

    void Start()
    {
        if (photonView.IsMine)
        {
            // ���� ��� ��������� �����, ���������� "You" � �������� �������
            localPlayerText.text = "You";
            localPlayerText.gameObject.SetActive(true);
            remotePlayerText.gameObject.SetActive(false);
        }
        else
        {
            // ���� ��� �������� �����, ���������� ��� ������� � �������� "You"
            remotePlayerText.text = photonView.Owner.NickName;
            remotePlayerText.gameObject.SetActive(true);
            localPlayerText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // ���������� �������� ��������� ������ ��� ������� ���������
        Text activeText = photonView.IsMine ? localPlayerText : remotePlayerText;
        activeText.transform.position = transform.position + offset;
    }
}
