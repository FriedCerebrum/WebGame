using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;

    public float runSpeed = 40f;

    [SerializeField] private float startCrouchSpeed;

    private bool pressed = false;

    public float horizontalMove = 0f;

    bool jump = false;
    bool crouch = false;
    private void Start()
    {
        startCrouchSpeed = controller.m_CrouchSpeed;
    }

    private void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        
        if (Input.GetKeyDown(KeyCode.R) && !pressed)
        {
            pressed = true;
            animator.SetTrigger("hurt");
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            pressed = false;
        }

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
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }
    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }
}
