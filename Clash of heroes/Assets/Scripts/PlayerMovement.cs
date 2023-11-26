using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;

    public float runSpeed = 120f;
    public float originalRunSpeed;

    [SerializeField] private float startCrouchSpeed;


    public float horizontalMove = 0f;

    bool jump = false;
    bool crouch = false;
    public bool moving = false;
    public bool in_air;
    private void Start()
    {
        originalRunSpeed = runSpeed;
        startCrouchSpeed = controller.m_CrouchSpeed;
    }

    public void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (!controller.IsGrounded())
        {
            controller.m_CrouchSpeed = 1;
        }
        else
        {
            controller.m_CrouchSpeed = startCrouchSpeed;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
        if (Input.GetButtonDown("Crouch") && controller.IsGrounded())
        {
            crouch = true;
        } else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }
        if (Mathf.Abs(horizontalMove) < 1f)
        {
            animator.SetBool("idle", true);
        }
        else
        {
            animator.SetBool("idle", false);
        }
        if (controller.IsGrounded() && crouch == true )
        {
            animator.SetBool("crouch", true);
        }
        else
        {
            animator.SetBool("crouch", false);
        }
        if (Mathf.Abs(horizontalMove) != 0)
        {
            animator.SetBool("moving", true);
            moving = true;
        }
        else
        {
            animator.SetBool("moving", false);
            moving = false;
        }
        if (controller.IsGrounded())
        {
            animator.SetBool("in_air", false);
            in_air = false;
        }
        else
        {
            animator.SetBool("in_air", true);
            in_air = true;
        }
    }
    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }
}
