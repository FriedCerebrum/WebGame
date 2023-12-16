using UnityEngine;
using Photon.Pun;
using System.Collections;

public class Entity2 : MonoBehaviourPunCallbacks
{
    public int maxHp = 100;
    public Animator animator;
    public Rigidbody2D rb;
    public Collider2D col;
    public ColoredFlash2 cf;
    public bool HasFPressed = false;
    RoundManager roundManager;

    public int hp;
    private bool canDie = false; // Внешний флажок для разрешения вызова Die()

    //private RoundManager roundManager;

    void Start()
    {
        hp = maxHp;
        //roundManager = FindObjectOfType<RoundManager>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.F) && !HasFPressed && gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                HurtMe(25);
                HasFPressed = true;
            }
            if (!Input.GetKeyDown(KeyCode.F))
            {
                HasFPressed = false;
            }
        }

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

        if (PhotonNetwork.IsMasterClient)
        {

            StartCoroutine(WaitAndStartNewRound());
        }
        else
        {

        }
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
    {                                               // при условии метки all.
        yield return new WaitForSeconds(2.0f);
        roundManager.StartNewRound();
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