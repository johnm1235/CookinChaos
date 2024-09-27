using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject player;
    public float speed = 5.0f;
    private CharacterController controller;
    private bool puedeMoverse = false; // Variable para controlar si el jugador puede moverse

    private bool isGrounded;
    private float gravity = 9.81f;

    private float rotationSpeed;

    public Animator anim;

    public void Start()
    {
        player = GameObject.Find("Player");
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    public void Update()
    {
        if (puedeMoverse)
        {
            Move();
        }
    }

    private void Move()
    {
        float move = Input.GetAxisRaw("Vertical");
        float strafe = Input.GetAxisRaw("Horizontal");

        Vector3 direction = new Vector3(strafe, 0f, move).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Rotaci�n del personaje en funci�n de la c�mara
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Movimiento hacia adelante seg�n la direcci�n
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDirection *= speed;

            // A�adir la gravedad al movimiento
            if (isGrounded)
            {
                moveDirection.y = -gravity * Time.deltaTime;
            }

            // Mover el personaje
            controller.Move(moveDirection * Time.deltaTime);
        }

        // Actualizar el par�metro de velocidad en el Animator
        anim.SetFloat("Speed", direction.magnitude * speed);
    }

    // M�todo para habilitar el movimiento del jugador
    public void HabilitarMovimiento()
    {
        puedeMoverse = true;
    }

    public void DeshabilitarMovimiento()
    {
        puedeMoverse = false;
    }
}
