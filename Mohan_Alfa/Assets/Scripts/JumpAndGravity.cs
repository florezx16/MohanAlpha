using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class JumpAndGravity : MonoBehaviour
{
    public float gravity;//La fuerza de gravedad del personaje
    public float fallMultiplier;//Aumenta la velocidad de caida
    public float jumpForce;//Fuerza de salto
    public float jumpDelay;//Retraso de salto

    private CharacterController controller;
    private Vector3 velocity;

    private Animator animator;
    private bool isGrounded;
    private bool isLanding;
    private bool isJumping;

    public LayerMask groundLayer; //Layer de suelo
    public float groundDistance = 0.2f; //Proximidad del jugador hacia el suelo
    public Transform groundCheck; //GameObject de verificacion de suelo

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer); //IsReallyGrounded();

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -3f;
        }

        //Activacion del salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsIdle", false);
            isJumping = true;
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        if (isJumping && isGrounded)
        {
            StartCoroutine(EndJump());
        }

        if (!isJumping)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        if (!isGrounded)
        {
            velocity.y += gravity * (fallMultiplier - 1) * Time.deltaTime;
            isLanding = true;
            isJumping = false;
            //Debug.Log("EN AIRE");
        }
        else
        {
            isLanding = false;
            //Debug.Log("EN SUELO");
        }

        controller.Move(velocity * Time.deltaTime);

        //Actualizar las animaciones
        animator.SetBool("IsFalling", isLanding);
        animator.SetBool("IsGrounded", isGrounded);
    }

    IEnumerator EndJump()
    {
        float animationDuration = 0.5f; //Ajustable de animacion del salto
        yield return new WaitForSeconds(animationDuration);

        isJumping = false;
        animator.SetBool("IsJumping", false);
    }

}
