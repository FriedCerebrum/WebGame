using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AttachTextAbovePlayer : MonoBehaviourPun
{
    public GameObject textPrefab; // Ссылка на текстовое поле, которое нужно отобразить

    private GameObject playerText; // Ссылка на текстовое поле, которое будет отображаться

    void Start()
    {
        // Проверяем, является ли объект нашим игроком (photon.isMine)
        if (photonView.IsMine)
        {
            // Создаем текстовое поле из префаба
            playerText = Instantiate(textPrefab, transform.position, Quaternion.identity);

            // Устанавливаем позицию текстового поля над игроком (например, над его головой)
            Vector3 offset = new Vector3(0f, 2f, 0f); // Это смещение для позиции текстового поля
            playerText.transform.position = transform.position + offset;

            // Закрепляем текстовое поле к игроку, чтобы оно двигалось вместе с ним
            playerText.transform.parent = transform;
        }
    }
}
