using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AttachTextAbovePlayer : MonoBehaviourPun
{
    public GameObject textPrefab; // ������ �� ��������� ����, ������� ����� ����������

    private GameObject playerText; // ������ �� ��������� ����, ������� ����� ������������

    void Start()
    {
        // ���������, �������� �� ������ ����� ������� (photon.isMine)
        if (photonView.IsMine)
        {
            // ������� ��������� ���� �� �������
            playerText = Instantiate(textPrefab, transform.position, Quaternion.identity);

            // ������������� ������� ���������� ���� ��� ������� (��������, ��� ��� �������)
            Vector3 offset = new Vector3(0f, 2f, 0f); // ��� �������� ��� ������� ���������� ����
            playerText.transform.position = transform.position + offset;

            // ���������� ��������� ���� � ������, ����� ��� ��������� ������ � ���
            playerText.transform.parent = transform;
        }
    }
}
