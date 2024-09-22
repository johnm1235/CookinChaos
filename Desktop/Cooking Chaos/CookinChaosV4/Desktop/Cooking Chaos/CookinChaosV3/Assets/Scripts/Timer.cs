using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float tiempoJuego = 60f; // Duración del juego en segundos
    public TMP_Text temporizadorText; // Texto UI para mostrar el temporizador
    public TMP_Text mensajeFinText; // Texto UI para mostrar el mensaje de fin del juego
    public TMP_Text contadorPreparacionText; // Texto UI para mostrar el contador de preparación

    private float tiempoRestante;
    private bool juegoEnCurso = false;

    private void Start()
    {
        mensajeFinText.gameObject.SetActive(false);
        StartCoroutine(ContadorPreparacion());
    }

    private void Update()
    {
        if (juegoEnCurso)
        {
            tiempoRestante -= Time.deltaTime;
            if (tiempoRestante <= 0)
            {
                tiempoRestante = 0; // Asegurarse de que el tiempo no sea negativo
                FinDelJuego();
            }
            ActualizarTemporizadorUI();
        }
    }

    private IEnumerator ContadorPreparacion()
    {
        for (int i = 3; i > 0; i--)
        {
            contadorPreparacionText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        contadorPreparacionText.text = "¡ADELANTE!";
        yield return new WaitForSeconds(1);
        contadorPreparacionText.gameObject.SetActive(false);

        IniciarJuego();
    }

    private void IniciarJuego()
    {
        tiempoRestante = tiempoJuego + 1;
        juegoEnCurso = true;

        // Habilitar el movimiento del jugador
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.HabilitarMovimiento();
        }

        // Habilitar la generación de pedidos
        Orders orders = FindObjectOfType<Orders>();
        if (orders != null)
        {
            orders.HabilitarGeneracionPedidos();
        }
    }

    private void FinDelJuego()
    {
        juegoEnCurso = false;
        mensajeFinText.gameObject.SetActive(true);
        mensajeFinText.text = "¡TIEMPO!";

        // Deshabilitar el movimiento del jugador
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.DeshabilitarMovimiento();
        }

        // Deshabilitar la generación de pedidos
        Orders orders = FindObjectOfType<Orders>();
        if (orders != null)
        {
            orders.DeshabilitarGeneracionPedidos();
        }

        StartCoroutine(ReiniciarJuego());
    }

    private IEnumerator ReiniciarJuego()
    {
        yield return new WaitForSeconds(3); // Esperar 3 segundos antes de reiniciar el juego
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ActualizarTemporizadorUI()
    {
        if (temporizadorText != null)
        {
            int minutos = Mathf.FloorToInt(tiempoRestante / 60);
            int segundos = Mathf.FloorToInt(tiempoRestante % 60);
            temporizadorText.text = string.Format("{0:00}:{1:00}", minutos, segundos);
        }
    }
}
