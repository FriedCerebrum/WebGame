using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Entity : MonoBehaviour
{
    public int maxHp = 100;
    public Animator animator;
    public Rigidbody2D rb;
    public Collider2D col;
    public ColoredFlash cf;
    public bool HasFPressed = false;

    public int hp;
    void Start()
    {
        hp = maxHp;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !HasFPressed && gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            HurtMe(25);
            HasFPressed = true;
        }
        if (!Input.GetKeyDown(KeyCode.F)) // dev
        {
            HasFPressed = false;
        }
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
        hp -= damage;
    }
    public void HurtMe(int damage) // dev
    {
        hp -= damage;
        Color flashColor = new Color(0.7f, 0f, 0f);
        cf.Flash(flashColor);
    }
}
