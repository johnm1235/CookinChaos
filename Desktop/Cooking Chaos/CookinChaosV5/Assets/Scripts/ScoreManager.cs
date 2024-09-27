using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public TMP_Text puntajeText; // Texto UI para mostrar el puntaje
    public TMP_Text puntosPedidoText; // Texto UI para mostrar los puntos ganados o perdidos en cada pedido
    public int puntajeActual = 0;
    public static int puntajeFinal; // Variable estática para almacenar el puntaje final
    private string prefijoPuntaje; // Prefijo del puntaje tomado del texto inicial
    private int puntajeImpuesto = 100; // Puntaje que el jugador debe superar para obtener el tercer logro
    public TMP_Text puntajeImpuestoText; // Texto UI para mostrar el puntaje impuesto
    public TMP_Text logroText3;
    public int puntajeImpuestoMin = 50; // Puntaje mínimo que el jugador debe superar
    public int puntajeImpuestoMax = 150; // Puntaje máximo que el jugador debe superar
    private Orders achieve;

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
        // Generar el puntaje impuesto al azar dentro del rango establecido
        puntajeImpuesto = Random.Range(puntajeImpuestoMin, puntajeImpuestoMax + 1);

        achieve = GetComponent<Orders>();

        // Guardar el texto inicial como prefijo
        if (puntajeText != null)
        {
            prefijoPuntaje = puntajeText.text;
        }
        ActualizarPuntajeUI();

        // Mostrar el puntaje impuesto
        if (puntajeImpuestoText != null)
        {
            puntajeImpuestoText.text = "RECORD: " + puntajeImpuesto;
        }

        if (logroText3 != null)
        {
            logroText3.gameObject.SetActive(false);
        }
    }

    public void AgregarPuntos(int puntos)
    {
        puntajeActual += puntos;
        MostrarPuntosPedido(puntos);
        ActualizarPuntajeUI();

        // Verificar si el jugador ha superado el puntaje impuesto
        if (puntajeActual >= puntajeImpuesto)
        {
            StartCoroutine(MostrarTercerLogro());

        }
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
            // Aquí puedes agregar lógica adicional para animar o mostrar temporalmente el texto
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

    // Método para reiniciar el puntaje
    public void ReiniciarPuntaje()
    {
        puntajeActual = 0;
        puntajeFinal = 0;
        puntajeImpuesto = Random.Range(puntajeImpuestoMin, puntajeImpuestoMax + 1);
        ActualizarPuntajeUI();
    }

    public IEnumerator MostrarTercerLogro()
    {
        string logro = SceneManager.GetActiveScene().name;
        // Verificar si el tercer logro ya ha sido obtenido
        if (PlayerPrefs.GetInt("LogroPuntaje", 0) == 1 && logro == "Level1")
        {
            yield break; // Salir de la corrutina si el tercer logro ya ha sido obtenido
        }
        else if(PlayerPrefs.GetInt("LogroPuntaje2", 0) == 1 && logro == "Level2")
        {
            yield break; // Salir de la corrutina si el tercer logro ya ha sido obtenido
        }

        if (logroText3 != null && logro == "Level1")
        {
            logroText3.gameObject.SetActive(true); // Mostrar el texto del tercer logro
            PlayerPrefs.SetInt("LogroPuntaje", 1); // Guardar el estado del tercer logro
            PlayerPrefs.Save(); // Asegurarse de que se guarde inmediatamente
            yield return new WaitForSeconds(2f); // Mostrar el logro durante 2 segundos
            logroText3.gameObject.SetActive(false); // Ocultar el texto del tercer logro
        }
        else if (logroText3 != null && logro == "Level2")
        {
            logroText3.gameObject.SetActive(true); // Mostrar el texto del tercer logro
            PlayerPrefs.SetInt("LogroPuntaje2", 1); // Guardar el estado del tercer logro
            PlayerPrefs.Save(); // Asegurarse de que se guarde inmediatamente
            yield return new WaitForSeconds(2f); // Mostrar el logro durante 2 segundos
            logroText3.gameObject.SetActive(false); // Ocultar el texto del tercer logro
        }
    }
}
