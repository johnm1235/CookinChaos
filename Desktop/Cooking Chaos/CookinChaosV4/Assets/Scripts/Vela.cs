using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Vela : MonoBehaviour
{
    [SerializeField] private Light velaLuz;          // La luz de la vela
    [SerializeField] private float tiempoEncendida;  // Tiempo en segundos que la vela estará encendida
    [HideInInspector] public bool estaApagada;  // Indica si la vela está apagada o encendida
    private bool enRango;          // Indica si el jugador está dentro del trigger

    private List<Color> originalColors = new List<Color>();
    private List<Renderer> childRenderers = new List<Renderer>();
    [SerializeField] private Color SelectedColor;  // Color de la vela encendida

    [SerializeField] private TMP_Text interactText;  // Referencia al objeto Text en la UI

    private void Start()
    {
        velaLuz.enabled = true;
        estaApagada = false;
        enRango = false;

        // Guardar los colores originales de todos los hijos
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            originalColors.Add(renderer.material.color);
            childRenderers.Add(renderer);
        }
    }

    private void Update()
    {
        // Si la vela está apagada y el jugador está en rango, permitir encenderla
        if (estaApagada && enRango && Input.GetKeyDown(KeyCode.E))
        {
            EncenderVela();
        }
    }

    // Método para apagar la vela (público para ser llamado desde el VelaManager)
    public void ApagarVela()
    {
        if (!estaApagada)
        {
            velaLuz.enabled = false;
            estaApagada = true;
            Debug.Log("La vela se ha apagado.");
        }
    }

    // Método para encender la vela
    private void EncenderVela()
    {
        velaLuz.enabled = true;
        estaApagada = false;
        Debug.Log("La vela se ha encendido.");
    }

    // Detectar cuando el jugador entra en el rango de la vela (trigger)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Verifica si el objeto es el jugador
        {
            enRango = true;
            CanvasKeyOn();
            Debug.Log("El jugador está en rango para encender la vela.");
        }
    }

    // Detectar cuando el jugador sale del rango de la vela (trigger)
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))  // Verifica si el objeto es el jugador
        {
            enRango = false;
            CanvasKeyOff();
            Debug.Log("El jugador ha salido del rango de la vela.");
        }
    }

    private void CanvasKeyOn()
    {
        // Mostrar la tecla "E" en la pantalla
        interactText.gameObject.SetActive(true);
        interactText.text = "[ E ]";
        for (int i = 0; i < childRenderers.Count; i++)
        {
            childRenderers[i].material.color = SelectedColor;
        }
    }

    private void CanvasKeyOff()
    {
        // Ocultar la tecla "E" en la pantalla
        interactText.gameObject.SetActive(false);
        for (int i = 0; i < childRenderers.Count; i++)
        {
            childRenderers[i].material.color = originalColors[i];
        }
    }
}
