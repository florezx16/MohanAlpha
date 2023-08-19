using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterController : MonoBehaviour
{
    Animator animator; //Accedemos a las animaciones del personaje
    CharacterController controller; //Accedemos a los controles del personaje
    Transform cam; //Accedemos a la camara

    //Variables para definir las caracteristicas de los controles
    public float frontward; //Velocidad de movimiento hacia adelante
    public float backwardSpeed; //Velocidad de movimiento hacia atras
    public float turnSmoothTime; //Tiempo de suavizado para la rotacion(como rota pues)
    private float turnSmoothVelocity; //Velocidad adicional para la rotacion
    private Vector3 initialPosition; //Guardamos posicion de inicio
    private Quaternion initialRotation; //Gaurdamos la rotacion

    void Start()
    {
        //Inicializamos componentes
        animator = this.GetComponent<Animator>();
        controller = this.GetComponent<CharacterController>();
        cam = this.GetComponent<Transform>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        //Obtenemos los controles(PC o control)
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //Definimos vector de movimiento
        Vector3 direction = new Vector3(horizontal, 0f, Mathf.Abs(vertical)).normalized;

        //Personaje en movimiento
        if (direction.magnitude >= 0.1f)
        {
            //Calcular el angulo segun la direccion de entrada
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            //Si el movimiento es hacia atras, invertimos el angulo
            if (vertical < 0)
            {
                targetAngle += horizontal * 90; //Ajustamos el valor para que se pueda mover en diagonal asi vaya para atras
            }

            //Hacemos la rotacion mas suabe
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            //Aplicamos la rotacion
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Calculamos la direccion del objeto
            float moveDirection = (vertical < 0) ? -1 : 1;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * moveDirection;

            //Comprobar si el movimiento del personaje es lineal(adelante o atras)
            if (Mathf.Abs(vertical) > 0.1f)
            {
                //Personaje llendo hacia atras
                if (vertical < 0)
                {
                    //Establecer variables de animacion y mover el personaje a la velocidad hacia atras
                    animator.SetBool("IsWalkingBackwards", true);
                    animator.SetBool("RightTurn", false);
                    animator.SetBool("LeftTurn", false);
                    animator.SetBool("IsIdle", false);
                    controller.Move(moveDir.normalized * backwardSpeed * Time.deltaTime);
                }

                //Personaje hacia adelante
                else
                {
                    //Establecer variables de animacion y mover el personaje a la velocidad hacia adelante
                    animator.SetBool("IsWalkingBackwards", false);
                    animator.SetBool("RightTurn", false);
                    animator.SetBool("LeftTurn", false);
                    animator.SetBool("IsIdle", false);
                    animator.SetBool("IsWalking", true);
                    controller.Move(moveDir.normalized * frontward * Time.deltaTime);
                }
            }

            //Comprobar si el personaje se esta moviendose horizontalmente (en giro)
            else if (Mathf.Abs(horizontal) > 0.1f)
            {
                //Definimos variable IsIdle a false
                animator.SetBool("IsIdle", false);
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsWalkingBackwards", false);


                //Giro a la derecha
                if (horizontal > 0)
                {
                    animator.SetBool("RightTurn", true);
                    animator.SetBool("LeftTurn", false);
                }
                //Giro a la izquierda
                else if (horizontal < 0)
                {
                    animator.SetBool("RightTurn", false);
                    animator.SetBool("LeftTurn", true);
                }
            }
            //Personaje inmovil
            else
            {
                //Reestablecer variables de animaciones por defecto
                ResetAnimations();
            }
        }
        //Personaje inmovil
        else
        {
            //Reestablecer variables de animaciones por defecto
            ResetAnimations();
        }
    }

    public void ResetAnimations()
    {
        animator.SetBool("RightTurn", false);
        animator.SetBool("LeftTurn", false);
        animator.SetBool("IsWalkingBackwards", false);
        animator.SetBool("IsWalking", false);
        if (animator.GetBool("IsJumping"))
        {
            animator.SetBool("IsIdle", false);
        }
        else
        {
            animator.SetBool("IsIdle", true);
        }
    }
    public void PlayerResetPosition()
    {
        controller.enabled = false;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        controller.enabled = true;
    }

}
