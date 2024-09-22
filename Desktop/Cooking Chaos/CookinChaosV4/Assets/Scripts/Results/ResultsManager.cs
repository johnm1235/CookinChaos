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

    private void Start()
    {
        // Mostrar el puntaje final en el texto UI
        puntajeFinalText.text = ScoreManager.puntajeFinal.ToString();

        // Mostrar los pedidos completados y fallidos en los textos UI
        pedidosCompletadosText.text = Orders.Instance.ObtenerPedidosCompletados().ToString();
        pedidosFallidosText.text = Orders.Instance.ObtenerPedidosFallidos().ToString();
    }

    // M�todo para reiniciar el nivel
    public void ReiniciarNivel()
    {
        // Reiniciar el puntaje
        ScoreManager.Instance.ReiniciarPuntaje();

        // Asume que el nombre de la escena del nivel es "Nivel"
        SceneManager.LoadScene("Level1");
    }

    // M�todo para regresar al men� principal
    public void IrAlMenuPrincipal()
    {
        // Asume que el nombre de la escena del men� principal es "MenuPrincipal"
        SceneManager.LoadScene("MainMenu");

        // Destruir el objeto ScoreManager para reiniciar el puntaje
        Destroy(ScoreManager.Instance.gameObject);
    }
}
