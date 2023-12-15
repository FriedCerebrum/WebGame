using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AttachTextAbovePlayer : MonoBehaviourPun
{

    [SerializeField]
    private Text playerText; // Ссылка на текстовый элемент UI

    void Start()
    {
        // Проверяем, является ли объект нашим игроком (photon.isMine)
        if (photonView.IsMine)
        {
            if (playerText != null)
            {
                // Устанавливаем позицию текстового элемента UI над игроком (например, над его головой)
                Vector3 offset = new Vector3(0f, 2f, 0f); // Это смещение для позиции текстового элемента UI
                playerText.transform.position = transform.position + offset;
            }
            else
            {
                Debug.LogError("Text object not assigned in the Inspector!");
            }
        }
    }
}
