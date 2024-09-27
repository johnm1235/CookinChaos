using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float tiempoJuego = 60f; // Duración del juego en segundos
    public TMP_Text temporizadorText; // Texto UI para mostrar el temporizador
    public TMP_Text mensajeFinText; // Texto UI para mostrar el mensaje de fin del juego
    public TMP_Text contadorPreparacionText; // Texto UI para mostrar el contador de preparación
    public string escenaResultados = "Resultados"; // Nombre de la escena de resultados
    private float tiempoEsperaResultados = 5f; // Tiempo de espera antes de mostrar los resultados

    // Referencias a los textos UI para mostrar los resultados
    public TMP_Text puntajeFinalText;

    private float tiempoRestante;
    private bool juegoEnCurso = false;

    public AudioSource audioSource;
    public AudioClip audioClipEndGame;
    public AudioClip audioClipStartGame;
    public AudioClip audioClip11Segundos; // Nuevo AudioClip para los 11 segundos restantes

    // Variable estática para almacenar el nombre de la escena actual
    public static string nombreEscenaActual;

    private bool audio11SegundosReproducido = false; // Para asegurarse de que el audio se reproduzca solo una vez

    private void Start()
    {
        mensajeFinText.gameObject.SetActive(false);
        StartCoroutine(ContadorPreparacion());

        // Inicializar el audio source
        audioSource = GetComponent<AudioSource>();
        // Reproducir el audio de inicio del juego
        audioSource.PlayOneShot(audioClipStartGame);

        GameObject otroAudioGameObject = GameObject.Find("Main Camera");
        if (otroAudioGameObject != null)
        {
            AudioSource otroAudioSource = otroAudioGameObject.GetComponent<AudioSource>();
            if (otroAudioSource != null)
            {
                otroAudioSource.Stop();
            }
        }
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
            else if (tiempoRestante <= 11 && !audio11SegundosReproducido)
            {
                // Reproducir el audio cuando falten 11 segundos
                audioSource.PlayOneShot(audioClip11Segundos);
                audio11SegundosReproducido = true;
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

        GameObject otroAudioGameObject = GameObject.Find("Main Camera");
        if (otroAudioGameObject != null)
        {
            AudioSource otroAudioSource = otroAudioGameObject.GetComponent<AudioSource>();
            if (otroAudioSource != null)
            {
                otroAudioSource.Play();
            }
        }

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

        // Reproducir el audio de fin del juego
        audioSource.PlayOneShot(audioClipEndGame);

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

        // Desactivar el otro audio
        GameObject otroAudioGameObject = GameObject.Find("Main Camera");
        if (otroAudioGameObject != null)
        {
            AudioSource otroAudioSource = otroAudioGameObject.GetComponent<AudioSource>();
            if (otroAudioSource != null)
            {
                otroAudioSource.Stop();
            }
        }

        // Guardar el puntaje final
        ScoreManager.Instance.GuardarPuntajeFinal();

        // Guardar el nombre de la escena actual
        nombreEscenaActual = SceneManager.GetActiveScene().name;

        // Iniciar la corrutina para mostrar los resultados después de unos segundos
        StartCoroutine(MostrarResultados());
    }

    private IEnumerator MostrarResultados()
    {
        // Esperar antes de cargar la escena de resultados
        yield return new WaitForSeconds(tiempoEsperaResultados);

        // Cargar la escena de resultados
        SceneManager.LoadScene(escenaResultados);
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
