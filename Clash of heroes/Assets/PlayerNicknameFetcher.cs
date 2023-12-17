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
        // Получаем ссылку на PhotonView компонент этого объекта
        PhotonView photonView = GetComponent<PhotonView>();

        // Проверяем, является ли этот игрок локальным
        if (!photonView.IsMine)
        {
            // Если это не локальный игрок, получаем его никнейм
            if (photonView.Owner.CustomProperties.TryGetValue("Nickname", out object nickname))
            {
                // Отображаем никнейм в текстовом поле
                nicknameText.text = nickname.ToString();
            }
            else
            {
                Debug.LogError("Никнейм не найден в CustomProperties");
            }
        }
    }



}
