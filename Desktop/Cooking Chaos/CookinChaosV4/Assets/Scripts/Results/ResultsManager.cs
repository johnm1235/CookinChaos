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

    // Método para reiniciar el nivel
    public void ReiniciarNivel()
    {
        // Reiniciar el puntaje
        ScoreManager.Instance.ReiniciarPuntaje();

        // Asume que el nombre de la escena del nivel es "Nivel"
        SceneManager.LoadScene("Level1");
    }

    // Método para regresar al menú principal
    public void IrAlMenuPrincipal()
    {
        // Asume que el nombre de la escena del menú principal es "MenuPrincipal"
        SceneManager.LoadScene("MainMenu");

        // Destruir el objeto ScoreManager para reiniciar el puntaje
        Destroy(ScoreManager.Instance.gameObject);
    }
}
