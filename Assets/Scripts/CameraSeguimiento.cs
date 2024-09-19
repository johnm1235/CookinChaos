using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraSeguimiento : MonoBehaviour
{
    public Transform jugador;           // El jugador que la c�mara debe seguir
    public float suavizado = 0.05f;     // Valor de suavizado para el movimiento (aj�stalo para mayor suavidad)
    public float limiteMovimiento = 0.5f; // Limita cu�nto puede moverse la c�mara desde su posici�n inicial
    public float sensibilidad = 0.1f;   // Controla la cantidad de movimiento respecto al jugador

    private Vector3 posicionInicial;    // La posici�n inicial de la c�mara

    private void Start()
    {
        // Guardar la posici�n inicial de la c�mara
        posicionInicial = transform.position;
    }

    private void LateUpdate()
    {
        // Solo mover la c�mara si el jugador se desplaza en el eje X
        float diferenciaX = jugador.position.x - posicionInicial.x;

        // Limitar el movimiento de la c�mara en el eje X con la sensibilidad y el l�mite de movimiento
        float desplazamientoX = Mathf.Clamp(diferenciaX * sensibilidad, -limiteMovimiento, limiteMovimiento);

        // Crear la nueva posici�n deseada de la c�mara, solo moviendo en el eje X
        Vector3 posicionDeseada = new Vector3(posicionInicial.x + desplazamientoX, posicionInicial.y, posicionInicial.z);

        // Suavizar la transici�n entre la posici�n actual de la c�mara y la posici�n deseada
        transform.position = Vector3.Lerp(transform.position, posicionDeseada, suavizado);
    }
}

