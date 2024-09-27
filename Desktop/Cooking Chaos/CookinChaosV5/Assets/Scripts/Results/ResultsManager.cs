using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsManager : MonoBehaviour
{
    public TMP_Text puntajeFinalText;
    public TMP_Text pedidosCompletadosText;
    public TMP_Text pedidosFallidosText;

    public AudioSource audioSource;

    private void Start()
    {
        // Mostrar el puntaje final en el texto UI
        puntajeFinalText.text = ScoreManager.puntajeFinal.ToString();

        // Mostrar los pedidos completados y fallidos en los textos UI
        pedidosCompletadosText.text = Orders.Instance.ObtenerPedidosCompletados().ToString();
        pedidosFallidosText.text = Orders.Instance.ObtenerPedidosFallidos().ToString();

        // Reproducir sonido
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    // Método para reiniciar el nivel
    public void ReiniciarNivel()
    {
        // Reiniciar el puntaje
        ScoreManager.Instance.ReiniciarPuntaje();

        // Recargar la escena anterior
        string nombreNivel = Timer.nombreEscenaActual;
        if (!string.IsNullOrEmpty(nombreNivel))
        {
            SceneManager.LoadScene(nombreNivel);
        }
        else
        {
            Debug.LogWarning("Nombre de la escena anterior no establecido.");
        }
    }

    // Método para regresar al menú principal
    public void IrAlMenuPrincipal()
    {
        // Asume que el nombre de la escena del menú principal es "MainMenu"
        SceneManager.LoadScene("MainMenu");

        // Destruir el objeto ScoreManager para reiniciar el puntaje
        Destroy(ScoreManager.Instance.gameObject);
    }

    public void NextLevel()
    {
        // Destruir el objeto ScoreManager para reiniciar el puntaje
        Destroy(ScoreManager.Instance.gameObject);

        // Asume que el nombre de la escena del siguiente nivel es "Level2"
        SceneManager.LoadScene("Level2");
    }
}
