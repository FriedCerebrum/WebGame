using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPunCallbacks, IPunObservable // Реализуем интерфейс IPunObservable
{
    public CharacterController2D controller;
    public Animator animator;

    public float runSpeed = 120f;
    public float originalRunSpeed;

    [SerializeField] public float startCrouchSpeed;

    public float horizontalMove = 0f;
    public bool jump = false;
    public bool crouch = false;
    public bool moving = false;
    public bool in_air;

    public void Start()
    {
        originalRunSpeed = runSpeed;
        startCrouchSpeed = controller.m_CrouchSpeed;

        // Если объект, к которому прикреплен этот скрипт, не принадлежит локальному игроку, выключаем управление
        if (!photonView.IsMine)
        {
            enabled = false;
        }
    }

    public void Update()
    {
        if (photonView.IsMine) // Убедимся, что управление активно только для локального игрока
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
            }
            else if (Input.GetButtonUp("Crouch"))
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
            if (controller.IsGrounded() && crouch == true)
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
    }

    public void FixedUpdate()
    {
        if (photonView.IsMine) // Убедимся, что управление активно только для локального игрока
        {
            controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
            jump = false;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // В этом методе мы определяем, какие данные передавать по сети для синхронизации
        if (stream.IsWriting) // Отправляем данные
        {
            stream.SendNext(horizontalMove);
            stream.SendNext(jump);
            stream.SendNext(crouch);
            stream.SendNext(moving);
            stream.SendNext(in_air);
        }
        else // Получаем данные
        {
            horizontalMove = (float)stream.ReceiveNext();
            jump = (bool)stream.ReceiveNext();
            crouch = (bool)stream.ReceiveNext();
            moving = (bool)stream.ReceiveNext();
            in_air = (bool)stream.ReceiveNext();
        }
    }
}