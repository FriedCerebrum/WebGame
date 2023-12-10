using System.Collections;
using UnityEngine;
using Photon.Pun; // ��������� ���������� Photon

public class PlayerCombat2 : MonoBehaviourPunCallbacks // ��������� �� MonoBehaviourPunCallbacks
{
    public Rigidbody2D player2dr;
    public Transform LegAttackPoint;
    public PlayerMovement2 playerMovement;
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public float attackSpeed = 0.07f;
    private bool ableToAttack = true;
    public float pushForce;
    public float decreaseFactor = 0.9f;

    void Update()
    {
        // �������� �� ��, �������� �� ������ ����������� ��������� �������
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && ableToAttack && !playerMovement.in_air)
            {
                StartCoroutine(RegularKick());
                Debug.Log("Regular Kick initiated"); 
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0) && ableToAttack && playerMovement.in_air && playerMovement.moving)
            {
                StartCoroutine(LegKick());
                Debug.Log("Leg Kick initiated"); 
            }
        }
    }


    IEnumerator SlowLegAttack()
    {
        int iterations = 20;
        for (int i = 0; i < iterations; i++)
        {
            Debug.Log("���������" + i); // dev
            playerMovement.runSpeed -= 4;
            yield return new WaitForSeconds(0.015f);
            if (!playerMovement.in_air)
            {
                Debug.Log("���������� ��������"); // dev
                break;
            }
        }
        playerMovement.runSpeed = playerMovement.originalRunSpeed;
    }

    IEnumerator LegKick()
    {
        StartCoroutine(SlowLegAttack());
        animator.SetTrigger("leg_kick");
        ableToAttack = false;
        yield return new WaitForSeconds(0.1f);
        Leg_attack();
        yield return new WaitForSeconds(1.2f); // ���� ����� ������������ ������� ������
        ableToAttack = true;
    }

    IEnumerator RegularKick()
    {
        animator.SetTrigger("just_kick");
        ableToAttack = false;
        yield return new WaitForSeconds(attackSpeed);

        // �������� RPC ����� ��� �����
        photonView.RPC("Attack", RpcTarget.All);

        yield return new WaitForSeconds(0.13f);

        // �������� RPC ����� ��� ������ �����
        photonView.RPC("Second_attack", RpcTarget.All);

        ableToAttack = true;
    }

    [PunRPC]
    void Leg_attack()
    {
        Debug.Log("Leg attack!"); // dev
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(LegAttackPoint.position, attackRange * 1.5f, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.GetComponent<Entity2>().hp > 0)
            {
                Debug.Log("Entity �����"); // dev
                if (enemy.GetComponent<ColoredFlash>() != null)
                {
                    Debug.Log("ColoredFlash �����"); // dev
                    Color flashColor = new Color(0.7f, 0f, 0f);
                    enemy.GetComponent<ColoredFlash>().Flash(flashColor);
                }
                Vector2 pushDirection = enemy.transform.position - transform.position;
                pushDirection.Normalize();

                pushDirection += new Vector2(0, 2f);
                pushDirection.Normalize();
                Debug.DrawLine(attackPoint.position, (Vector2)attackPoint.position + pushDirection * 2, Color.red, 2f);

                enemy.GetComponent<Rigidbody2D>().AddForce(pushDirection * (pushForce * 2f), ForceMode2D.Impulse); // ���� � ���� ���������� � 2 ���� �������

                Debug.Log("We hit " + enemy.name); // dev
            }
            Debug.Log("We hit " + enemy.name); // dev
            enemy.GetComponent<Entity>().TakeDamage(Random.Range(30, 45));
            StopCoroutine(SlowLegAttack());
            playerMovement.runSpeed = playerMovement.originalRunSpeed;
        }
    }

    [PunRPC]
    void Attack()
    {
        Debug.Log("Attack!"); // dev
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            // ���������, �� ����� �� enemy null � ����� �� �� ��������� Entity
            if (enemy != null && enemy.GetComponent<Entity2>() != null)
            {
                Entity2 entityComponent = enemy.GetComponent<Entity2>();
                if (entityComponent.hp > 0)
                {
                    Debug.Log("Entity �����"); // dev
                    if (enemy.GetComponent<ColoredFlash>() != null)
                    {
                        Debug.Log("ColoredFlash �����"); // dev
                        Color flashColor = new Color(0.7f, 0f, 0f);
                        enemy.GetComponent<ColoredFlash>().Flash(flashColor);
                    }
                    Vector2 pushDirection = enemy.transform.position - transform.position;
                    pushDirection.Normalize();

                    pushDirection += new Vector2(0, 2f);
                    pushDirection.Normalize();
                    Debug.DrawLine(attackPoint.position, (Vector2)attackPoint.position + pushDirection * 2, Color.red, 2f);
                    enemy.GetComponent<Rigidbody2D>().AddForce(pushDirection * pushForce, ForceMode2D.Impulse);

                    Debug.Log("We hit " + enemy.name); // dev
                    entityComponent.TakeDamage(Random.Range(15, 25));
                }
            }
        }
    }

    [PunRPC]
    void Second_attack()
    {
        Debug.Log("Second Attack!"); // dev
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            // ���������, ��� enemy �� ����� null � ����� ��������� Entity
            if (enemy != null && enemy.GetComponent<Entity2>() != null)
            {
                Entity2 entityComponent = enemy.GetComponent<Entity2>();
                if (entityComponent.hp > 0)
                {
                    Debug.Log("Entity �����"); // dev
                    if (enemy.GetComponent<ColoredFlash>() != null)
                    {
                        Debug.Log("ColoredFlash �����"); // dev
                        Color flashColor = new Color(0.7f, 0f, 0f);
                        enemy.GetComponent<ColoredFlash>().Flash(flashColor);
                    }
                    Vector2 pushDirection = enemy.transform.position - transform.position;
                    pushDirection.Normalize();

                    pushDirection += new Vector2(0, 2f);
                    pushDirection.Normalize();
                    Debug.DrawLine(attackPoint.position, (Vector2)attackPoint.position + pushDirection * 2, Color.red, 2f);
                    enemy.GetComponent<Rigidbody2D>().AddForce(pushDirection * pushForce, ForceMode2D.Impulse);

                    Debug.Log("We hit " + enemy.name); // dev
                    entityComponent.TakeDamage(Random.Range(20, 30));
                }
            }
        }
    }



    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void OnDrawGizmos()
    {
        if (LegAttackPoint == null)
            return;
        Gizmos.DrawWireSphere(LegAttackPoint.position, attackRange * 1.4f);
    }
}
