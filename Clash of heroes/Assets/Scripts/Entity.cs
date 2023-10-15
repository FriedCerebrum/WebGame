using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int maxHp = 100;
    private int damage;

    public int hp;
    void Start()
    {
        hp = maxHp;
    }

    void Update()
    {
        if (hp <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }
    public void TakeDamage(int damage)
    {
        hp -= damage;
    }
}
