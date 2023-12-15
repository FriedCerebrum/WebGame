using UnityEngine;
using UnityEngine.UI; // Или TMPro для TextMeshPro
using Photon.Pun;

public class NicknameTag : MonoBehaviourPun
{
    public Text localPlayerText; // Текст "You" для локального игрока
    public Text remotePlayerText; // Текст для никнейма удалённого игрока
    public Vector3 offset = new Vector3(0, 2, 0); // Смещение над головой персонажа

    void Start()
    {
        if (photonView.IsMine)
        {
            // Если это локальный игрок, показываем "You" и скрываем никнейм
            localPlayerText.text = "You";
            localPlayerText.gameObject.SetActive(true);
            remotePlayerText.gameObject.SetActive(false);
        }
        else
        {
            // Если это удалённый игрок, показываем его никнейм и скрываем "You"
            remotePlayerText.text = photonView.Owner.NickName;
            remotePlayerText.gameObject.SetActive(true);
            localPlayerText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Перемещаем активный текстовый объект над головой персонажа
        Text activeText = photonView.IsMine ? localPlayerText : remotePlayerText;
        activeText.transform.position = transform.position + offset;
    }
}
