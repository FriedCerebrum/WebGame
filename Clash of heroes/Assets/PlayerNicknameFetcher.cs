using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerNicknameFetcher : MonoBehaviour
{
    public Text nicknameText;
    void Start()
    {
        // �������� ������ �� PhotonView ��������� ����� �������
        PhotonView photonView = GetComponent<PhotonView>();

        // ���������, �������� �� ���� ����� ���������
        if (!photonView.IsMine)
        {
            // ���� ��� �� ��������� �����, �������� ��� �������
            if (photonView.Owner.CustomProperties.TryGetValue("Nickname", out object nickname))
            {
                // ���������� ������� � ��������� ����
                nicknameText.text = nickname.ToString();
            }
            else
            {
                Debug.LogError("������� �� ������ � CustomProperties");
            }
        }
    }



}
