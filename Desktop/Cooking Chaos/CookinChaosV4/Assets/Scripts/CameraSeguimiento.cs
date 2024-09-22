using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraSeguimiento : MonoBehaviour
{
    public Transform jugador;           // El jugador que la cámara debe seguir
    public float suavizado = 0.05f;     // Valor de suavizado para el movimiento (ajústalo para mayor suavidad)
    public float limiteMovimiento = 0.5f; // Limita cuánto puede moverse la cámara desde su posición inicial
    public float sensibilidad = 0.1f;   // Controla la cantidad de movimiento respecto al jugador

    private Vector3 posicionInicial;    // La posición inicial de la cámara

    private void Start()
    {
        // Guardar la posición inicial de la cámara
        posicionInicial = transform.position;
    }

    private void LateUpdate()
    {
        // Solo mover la cámara si el jugador se desplaza en el eje X
        float diferenciaX = jugador.position.x - posicionInicial.x;

        // Limitar el movimiento de la cámara en el eje X con la sensibilidad y el límite de movimiento
        float desplazamientoX = Mathf.Clamp(diferenciaX * sensibilidad, -limiteMovimiento, limiteMovimiento);

        // Crear la nueva posición deseada de la cámara, solo moviendo en el eje X
        Vector3 posicionDeseada = new Vector3(posicionInicial.x + desplazamientoX, posicionInicial.y, posicionInicial.z);

        // Suavizar la transición entre la posición actual de la cámara y la posición deseada
        transform.position = Vector3.Lerp(transform.position, posicionDeseada, suavizado);
    }
}

