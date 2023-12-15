using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AttachTextAbovePlayer : MonoBehaviourPun
{

    [SerializeField]
    private Text playerText; // ������ �� ��������� ������� UI

    void Start()
    {
        // ���������, �������� �� ������ ����� ������� (photon.isMine)
        if (photonView.IsMine)
        {
            if (playerText != null)
            {
                // ������������� ������� ���������� �������� UI ��� ������� (��������, ��� ��� �������)
                Vector3 offset = new Vector3(0f, 2f, 0f); // ��� �������� ��� ������� ���������� �������� UI
                playerText.transform.position = transform.position + offset;
            }
            else
            {
                Debug.LogError("Text object not assigned in the Inspector!");
            }
        }
    }
}
