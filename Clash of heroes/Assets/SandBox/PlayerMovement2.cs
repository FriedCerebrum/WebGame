using Photon.Pun;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviourPun
{
    public CharacterController22D controller;
    public Animator animator;

    public float runSpeed = 120f;
    public float originalRunSpeed;
    [SerializeField] private float startCrouchSpeed;

    public float horizontalMove = 0f;
    public bool jump = false;
    public bool crouch = false;
    public bool moving = false;
    public bool in_air;

    private void Start()
    {
        originalRunSpeed = runSpeed;
        startCrouchSpeed = controller.m_CrouchSpeed;
    }

    public void Update()
    {
        if (photonView.IsMine)
        {
            HandleInput();
        }

        UpdateAnimations();
    }

    private void HandleInput()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        jump = Input.GetButtonDown("Jump");
        if (Input.GetButtonDown("Crouch") && controller.IsGrounded())
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }

        moving = Mathf.Abs(horizontalMove) > 0;
    }

    private void UpdateAnimations()
    {
        bool isGrounded = controller.IsGrounded();

        animator.SetBool("idle", horizontalMove == 0);
        animator.SetBool("crouch", isGrounded && crouch);
        animator.SetBool("moving", moving);
        animator.SetBool("in_air", !isGrounded);
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
            jump = false;
        }
    }
}
