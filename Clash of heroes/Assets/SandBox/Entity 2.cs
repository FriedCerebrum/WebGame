using UnityEngine;
using Photon.Pun;
using System.Collections;
using Photon.Pun.Demo.PunBasics;

public class Entity2 : MonoBehaviourPunCallbacks
{
    public int maxHp = 100;
    public Animator animator;
    public Rigidbody2D rb;
    public Collider2D col;
    public ColoredFlash2 cf;
    public bool HasFPressed = false;
    RoundManager roundManager;
    GameManager gameManager;
    PhotonView gameManagerView;

    public int hp;
    private bool canDie = false; // Внешний флажок для разрешения вызова Die()
    void Start()
    {
        hp = maxHp;
        roundManager = FindObjectOfType<RoundManager>();
        gameManager = FindObjectOfType<GameManager>();
        GameObject gameManagerObject = GameObject.Find("GameManager");
        if (gameManagerObject != null)
        {
            // Получаем компонент PhotonView из найденного объекта
            gameManagerView = gameManagerObject.GetComponent<PhotonView>();
        }
        else
        {
            Debug.LogError("Объект GameManager не найден в сцене!");
        }
    }

    void Update()
    {
        if (hp <= 0 && canDie) // Проверяем флажок canDie
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Умираем");
        rb.isKinematic = true;
        col.enabled = false;
        if (animator != null)
        {
            animator.SetBool("dead", true);
        }
        rb.velocity = Vector3.zero;
        SetCantDie(); // Запрещаем дальнейшие вызовы Die()
        if (gameManagerView != null)
        {
            gameManagerView.RPC("AddToRoundWinnerCounter", RpcTarget.All, roundManager.RemotePlayerName);
        }
        else
        {
            Debug.LogError("PhotonView не найден на объекте GameManager");
        }

        Debug.Log("RemotePlayerName в Entity2: " + roundManager.RemotePlayerName);
        StartCoroutine(WaitAndStartNewRound());
    }

    public void ResetCanDie()
    {
        canDie = true;
    }
    public void SetCantDie()
    {
        canDie = false;
    }
    [PunRPC]
    private IEnumerator WaitAndStartNewRound()      // Если этот метод упихать под RPC, то StartNewRound вызовется только на машине, откуда вызывался
    {   if (PhotonNetwork.IsMasterClient)
        {
            yield return new WaitForSeconds(2.0f);
            roundManager.StartNewRound();
        }
        else
        {
            photonView.RPC("WaitAndStartNewRound", RpcTarget.MasterClient);
        }
    }

    public void TakeDamage(int damage)
    {
        // Убедитесь, что урон принимается только локальным игроком
        if (photonView.IsMine)
        {
            hp -= damage;
            // Отправляем изменение здоровья всем игрокам через RPC
            photonView.RPC("UpdateHealthRPC", RpcTarget.Others, hp);
        }
    }

    // RPC-метод для обновления здоровья
    [PunRPC]
    void UpdateHealthRPC(int newHp)
    {
        hp = newHp;
    }

    public void HurtMe(int damage)
    {
        // Применяем урон и визуальный эффект
        if (photonView.IsMine)
        {
            hp -= damage;
            Color flashColor = new Color(0.7f, 0f, 0f);
            cf.Flash(flashColor);
            // Отправляем изменение здоровья и визуальный эффект через RPC
            photonView.RPC("UpdateHealthRPC", RpcTarget.Others, hp);
        }
    }
}