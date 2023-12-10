using UnityEngine;
using Photon.Pun; // Добавляем библиотеку Photon

public class Entity2 : MonoBehaviourPunCallbacks // Наследуем от MonoBehaviourPunCallbacks
{
    public int maxHp = 100;
    public Animator animator;
    public Rigidbody2D rb;
    public Collider2D col;
    public ColoredFlash2 cf;
    public bool HasFPressed = false;

    public int hp;

    void Start()
    {
        hp = maxHp;
    }

    void Update()
    {
        // Проверка на локального игрока
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

        // Проверка смерти персонажа
        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        rb.isKinematic = true;
        col.enabled = false;
        if (animator != null)
        {
            animator.SetBool("dead", true);
        }
        rb.velocity = Vector3.zero;
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
