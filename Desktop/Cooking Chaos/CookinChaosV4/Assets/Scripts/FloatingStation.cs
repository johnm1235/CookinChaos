using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingStation : MonoBehaviour
{
    public Transform[] positions;  // Array de posiciones predefinidas
    public float liftHeight = 3f;   // Altura a la que se elevará la estación
    public float moveSpeed = 2f;    // Velocidad de movimiento horizontal
    public float verticalSpeed = 2f;  // Velocidad de movimiento vertical (subir/bajar)
    public float waitTime = 2f;     // Tiempo de espera antes de moverse a la siguiente posición
    public float waitAfterLowering = 2f; // Tiempo de espera después de bajar a la posición
    public float rotationSpeed = 2f;  // Velocidad de rotación hacia la nueva orientación

    private enum MovementState { Idle, Lifting, Moving, Lowering, Waiting }
    private MovementState currentState;

    private Vector3 targetPosition;  // La posición objetivo (horizontalmente)
    private Quaternion targetRotation; // La rotación objetivo
    private float initialYPosition;  // Guarda la posición Y original
    private float timer;

    public bool isCooking;  // Bandera para indicar si se está cocinando

    void Start()
    {
        // Inicia en modo idle
        currentState = MovementState.Idle;

        // Guarda la posición inicial en Y (altura original de la estación)
        initialYPosition = transform.position.y;

        // Selecciona una posición aleatoria para moverse
        SelectNewTarget();

        // Inicia el temporizador
        timer = waitTime;
    }

    void Update()
    {
        if (isCooking)
        {
            return;  // No hacer nada si se está cocinando
        }

        switch (currentState)
        {
            case MovementState.Idle:
                HandleIdleState();
                break;

            case MovementState.Lifting:
                HandleLiftingState();
                break;

            case MovementState.Moving:
                HandleMovingState();
                break;

            case MovementState.Lowering:
                HandleLoweringState();
                break;

            case MovementState.Waiting:
                HandleWaitingState();
                break;
        }
    }

    // Selecciona una nueva posición y rotación aleatoria de las estaciones de cocina
    private void SelectNewTarget()
    {
        Transform newTarget = positions[Random.Range(0, positions.Length)];
        targetPosition = newTarget.position;
        targetRotation = newTarget.rotation; // Nueva rotación objetivo
    }

    // Estado de espera antes de que se mueva la estación
    private void HandleIdleState()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            currentState = MovementState.Lifting;  // Cambia al estado de elevación
        }
    }

    // Estado de elevación de la estación hacia arriba
    private void HandleLiftingState()
    {
        Vector3 liftTarget = new Vector3(transform.position.x, initialYPosition + liftHeight, transform.position.z);

        // Mueve hacia arriba
        transform.position = Vector3.MoveTowards(transform.position, liftTarget, verticalSpeed * Time.deltaTime);

        // Si ha alcanzado la altura objetivo
        if (Vector3.Distance(transform.position, liftTarget) < 0.1f)
        {
            currentState = MovementState.Moving;  // Cambia al estado de movimiento horizontal
        }
    }

    // Estado de movimiento horizontal hacia la nueva posición y rotación
    private void HandleMovingState()
    {
        // Mueve hacia la nueva posición objetivo, manteniendo la altura elevada
        Vector3 horizontalTarget = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        transform.position = Vector3.MoveTowards(transform.position, horizontalTarget, moveSpeed * Time.deltaTime);

        // Rota hacia la nueva rotación objetivo
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Si ha alcanzado la posición y rotación horizontal
        if (Vector3.Distance(transform.position, horizontalTarget) < 0.1f && Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            currentState = MovementState.Lowering;  // Cambia al estado de descenso
        }
    }

    // Estado de descenso hacia el suelo
    private void HandleLoweringState()
    {
        Vector3 lowerTarget = new Vector3(transform.position.x, initialYPosition, transform.position.z);

        // Mueve hacia abajo
        transform.position = Vector3.MoveTowards(transform.position, lowerTarget, verticalSpeed * Time.deltaTime);

        // Si ha alcanzado el suelo
        if (Vector3.Distance(transform.position, lowerTarget) < 0.1f)
        {
            timer = waitAfterLowering;  // Inicia la espera después de bajar
            currentState = MovementState.Waiting;  // Cambia al estado de espera
        }
    }

    // Estado de espera después de llegar a la nueva posición
    private void HandleWaitingState()
    {
        timer -= Time.deltaTime;

        // Si el temporizador ha terminado, selecciona una nueva posición y vuelve al estado idle
        if (timer <= 0)
        {
            SelectNewTarget();  // Selecciona una nueva posición y rotación
            timer = waitTime;  // Reinicia el temporizador para el estado Idle
            currentState = MovementState.Idle;  // Vuelve al estado idle
        }
    }
}
