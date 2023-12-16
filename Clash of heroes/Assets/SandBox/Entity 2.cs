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
    private bool canDie = false; // ������� ������ ��� ���������� ������ Die()
    void Start()
    {
        hp = maxHp;
        roundManager = FindObjectOfType<RoundManager>();
    }

    void Update()
    {
        if (hp <= 0 && canDie) // ��������� ������ canDie
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("�������");
        rb.isKinematic = true;
        col.enabled = false;
        if (animator != null)
        {
            animator.SetBool("dead", true);
        }
        rb.velocity = Vector3.zero;
        SetCantDie(); // ��������� ���������� ������ Die()

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
    private IEnumerator WaitAndStartNewRound()      // ���� ���� ����� ������� ��� RPC, �� StartNewRound ��������� ������ �� ������, ������ ���������
    {   if (PhotonNetwork.IsMasterClient)
        {
            yield return new WaitForSeconds(2.0f);
            roundManager.StartNewRound();
        }
        else
        {
            yield return new WaitForSeconds(2.0f);
            photonView.RPC("StartNewRound", RpcTarget.MasterClient);
        }
    }

    public void TakeDamage(int damage)
    {
        // ���������, ��� ���� ����������� ������ ��������� �������
        if (photonView.IsMine)
        {
            hp -= damage;
            // ���������� ��������� �������� ���� ������� ����� RPC
            photonView.RPC("UpdateHealthRPC", RpcTarget.Others, hp);
        }
    }

    // RPC-����� ��� ���������� ��������
    [PunRPC]
    void UpdateHealthRPC(int newHp)
    {
        hp = newHp;
    }

    public void HurtMe(int damage)
    {
        // ��������� ���� � ���������� ������
        if (photonView.IsMine)
        {
            hp -= damage;
            Color flashColor = new Color(0.7f, 0f, 0f);
            cf.Flash(flashColor);
            // ���������� ��������� �������� � ���������� ������ ����� RPC
            photonView.RPC("UpdateHealthRPC", RpcTarget.Others, hp);
        }
    }
}