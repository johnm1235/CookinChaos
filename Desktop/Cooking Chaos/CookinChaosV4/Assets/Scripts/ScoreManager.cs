using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public TMP_Text puntajeText; // Texto UI para mostrar el puntaje
    public TMP_Text puntosPedidoText; // Texto UI para mostrar los puntos ganados o perdidos en cada pedido
    public int puntajeActual = 0;
    public static int puntajeFinal; // Variable est�tica para almacenar el puntaje final
    private string prefijoPuntaje; // Prefijo del puntaje tomado del texto inicial

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Guardar el texto inicial como prefijo
        if (puntajeText != null)
        {
            prefijoPuntaje = puntajeText.text;
        }
        ActualizarPuntajeUI();
    }

    public void AgregarPuntos(int puntos)
    {
        puntajeActual += puntos;
        MostrarPuntosPedido(puntos);
        ActualizarPuntajeUI();
    }

    public void RestarPuntos(int puntos)
    {
        puntajeActual -= puntos;
        MostrarPuntosPedido(-puntos);
        ActualizarPuntajeUI();
    }

    private void ActualizarPuntajeUI()
    {
        if (puntajeText != null)
        {
            puntajeText.text = prefijoPuntaje + puntajeActual;
        }
    }

    private void MostrarPuntosPedido(int puntos)
    {
        if (puntosPedidoText != null)
        {
            puntosPedidoText.text = (puntos > 0 ? "+" : "") + puntos.ToString();
            // Aqu� puedes agregar l�gica adicional para animar o mostrar temporalmente el texto
            StartCoroutine(DesaparecerPuntosPedido());
        }
    }

    private IEnumerator DesaparecerPuntosPedido()
    {
        yield return new WaitForSeconds(2); // Esperar 2 segundos antes de ocultar el texto
        if (puntosPedidoText != null)
        {
            puntosPedidoText.text = "";
        }
    }

    public int ObtenerPuntajeFinal()
    {
        return puntajeActual;
    }

    public void GuardarPuntajeFinal()
    {
        puntajeFinal = puntajeActual;
    }

    // M�todo para reiniciar el puntaje
    public void ReiniciarPuntaje()
    {
        puntajeActual = 0;
        puntajeFinal = 0;
        ActualizarPuntajeUI();
    }
}
