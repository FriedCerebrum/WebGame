using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public float attackSpeed = 0.07f;
    private bool ableToAttack = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && ableToAttack)
        {
            StartCoroutine(TimeToAttack());
        }
    }

    IEnumerator TimeToAttack()
    {
        ableToAttack = false;
        yield return new WaitForSeconds(attackSpeed);
        Attack();
        ableToAttack = true;
    }

    void Attack()
    {
        Debug.Log("Attack!");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {   
            if (enemy.GetComponent<Entity>().hp > 0)
            {
                Debug.Log("Entity найдён");
                if (enemy.GetComponent<ColoredFlash>() != null)
                {
                    Debug.Log("ColoredFlash найдён");
                    Color flashColor = new Color(0.7f, 0f, 0f);
                    enemy.GetComponent<ColoredFlash>().Flash(flashColor);
                }
            }
            Debug.Log("We hit " + enemy.name);
            enemy.GetComponent<Entity>().TakeDamage(Random.Range(25, 35));
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
