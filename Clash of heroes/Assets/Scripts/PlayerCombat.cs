using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Rigidbody2D player2dr;
    public Transform LegAttackPoint;
    public PlayerMovement playerMovement;
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
        if (Input.GetKeyDown(KeyCode.Mouse0) && ableToAttack && !playerMovement.in_air)
        {
            StartCoroutine(RegularKick());
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && ableToAttack && playerMovement.in_air && playerMovement.moving)
        {
            StartCoroutine(LegKick());
        }
    }
    IEnumerator SlowLegAttack()
    {
        int iterations = 20;
        for (int i = 0; i < iterations; i++)
        {
            Debug.Log("Замедлено" + i); // dev
            playerMovement.runSpeed -= 4;
            yield return new WaitForSeconds(0.015f);
            if (!playerMovement.in_air)
            {
                Debug.Log("Замедление отменено"); // dev
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
        yield return new WaitForSeconds(1.2f); // Удар ногой откатывается намного дольше
        ableToAttack = true;
    }

    IEnumerator RegularKick()
    {
        animator.SetTrigger("just_kick");
        ableToAttack = false;
        yield return new WaitForSeconds(attackSpeed);
        Attack();
        yield return new WaitForSeconds(0.13f);
        Second_attack();
        ableToAttack = true;
    }
    void Leg_attack() // Неэффективно, повысил урон
    {
        Debug.Log("Leg attack!"); // dev
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(LegAttackPoint.position, attackRange * 1.5f, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.GetComponent<Entity>().hp > 0)
            {
                Debug.Log("Entity найдён"); // dev
                if (enemy.GetComponent<ColoredFlash>() != null)
                {
                    Debug.Log("ColoredFlash найдён"); // dev
                    Color flashColor = new Color(0.7f, 0f, 0f);
                    enemy.GetComponent<ColoredFlash>().Flash(flashColor);
                }
                Vector2 pushDirection = enemy.transform.position - transform.position;
                pushDirection.Normalize();

                pushDirection += new Vector2(0, 2f);
                pushDirection.Normalize();
                Debug.DrawLine(attackPoint.position, (Vector2)attackPoint.position + pushDirection * 2, Color.red, 2f);

                enemy.GetComponent<Rigidbody2D>().AddForce(pushDirection * (pushForce * 2f), ForceMode2D.Impulse); // Удар с ноги откидывает в 2 раза сильнее

                Debug.Log("We hit " + enemy.name); // dev
            }
            Debug.Log("We hit " + enemy.name); // dev
            enemy.GetComponent<Entity>().TakeDamage(Random.Range(30, 45));
            StopCoroutine(SlowLegAttack());
            playerMovement.runSpeed = playerMovement.originalRunSpeed;
        }
    }
    void Attack()
    {
        Debug.Log("Attack!"); // dev
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {   
            if (enemy.GetComponent<Entity>().hp > 0)
            {
                Debug.Log("Entity найдён"); // dev
                if (enemy.GetComponent<ColoredFlash>() != null)
                {
                    Debug.Log("ColoredFlash найдён"); // dev
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
            }
            Debug.Log("We hit " + enemy.name); // dev
            enemy.GetComponent<Entity>().TakeDamage(Random.Range(15, 25));
        }
    }
    void Second_attack()
    {
        Debug.Log("Attack!"); // dev
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.GetComponent<Entity>().hp > 0)
            {
                Debug.Log("Entity найдён"); // dev
                if (enemy.GetComponent<ColoredFlash>() != null)
                {
                    Debug.Log("ColoredFlash найдён"); // dev
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
            }
            Debug.Log("We hit " + enemy.name); // dev
            enemy.GetComponent<Entity>().TakeDamage(Random.Range(20, 30));
        }
    }

    private void OnDrawGizmosSelected() // dev
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
