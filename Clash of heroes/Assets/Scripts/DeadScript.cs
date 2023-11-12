using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DeadScript : MonoBehaviour
{
    public Animator animator;
    public PlayerMovement pm;
    public PlayerCombat pc;
    public Entity entity;
    public GameObject targetObject;
    public Rigidbody2D rb;
    public void Update()
    {
        if (entity.hp <= 0 && gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (pm != null)
            {
                pm.enabled = false;
                animator.speed = 0;
            }
            if (pc != null)
            {
                pc.enabled = false;
                animator.speed = 0;
            }
        }
    }
}
