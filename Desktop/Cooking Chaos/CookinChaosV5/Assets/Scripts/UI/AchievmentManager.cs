using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievmentManager : MonoBehaviour
{
    public RawImage logroImage; // Imagen que representa el logro 1
    public RawImage secondLogro; // Imagen que representa el logro 2
    public RawImage thirdLogro;  // Imagen que representa el logro 3

    public RawImage logroImage2;
    public RawImage secondLogro2;
    public RawImage thirdLogro2;

    private static bool logrosReiniciados = false; // Variable para verificar si los logros han sido reiniciados

    void Start()
    {
        if (!logrosReiniciados)
        {
            ReiniciarLogros(); // Reiniciar los logros al iniciar el juego
            logrosReiniciados = true; // Marcar que los logros han sido reiniciados
        }

        // Verificar si el logro ha sido obtenido
        if (PlayerPrefs.GetInt("LogroObtenido", 0) == 1)
        {
            logroImage.gameObject.SetActive(true); // Mostrar la imagen del logro
        }
        else
        {
            logroImage.gameObject.SetActive(false); // Ocultar la imagen del logro
        }

        if (PlayerPrefs.GetInt("LogroObtenido2", 0) == 1)
        {
            logroImage2.gameObject.SetActive(true); // Mostrar la imagen del logro
        }
        else
        {
            logroImage2.gameObject.SetActive(false); // Ocultar la imagen del logro
        }

        // Verificar si el segundo logro ha sido obtenido
        if (PlayerPrefs.GetInt("LogroSinFallar", 0) == 1)
        {
            secondLogro.gameObject.SetActive(true); // Mostrar la imagen del segundo logro
        }
        else
        {
            secondLogro.gameObject.SetActive(false); // Ocultar la imagen del segundo logro
        }

        if (PlayerPrefs.GetInt("LogroSinFallar2", 0) == 1)
        {
            secondLogro2.gameObject.SetActive(true); // Mostrar la imagen del segundo logro
        }
        else
        {
            secondLogro2.gameObject.SetActive(false); // Ocultar la imagen del segundo logro
        }

        // Verificar si el tercer logro ha sido obtenido
        if (PlayerPrefs.GetInt("LogroPuntaje", 0) == 1)
        {
            thirdLogro.gameObject.SetActive(true); // Mostrar la imagen del tercer logro
        }
        else
        {
            thirdLogro.gameObject.SetActive(false); // Ocultar la imagen del tercer logro
        }

        if (PlayerPrefs.GetInt("LogroPuntaje2", 0) == 1)
        {
            thirdLogro2.gameObject.SetActive(true); // Mostrar la imagen del tercer logro
        }
        else
        {
            thirdLogro2.gameObject.SetActive(false); // Ocultar la imagen del tercer logro
        }
    }

    // Método para reiniciar los logros
    public void ReiniciarLogros()
    {
        PlayerPrefs.SetInt("LogroObtenido", 0); // Reiniciar el estado del primer logro
        PlayerPrefs.SetInt("LogroSinFallar", 0); // Reiniciar el estado del segundo logro
        PlayerPrefs.SetInt("LogroPuntaje", 0); // Reiniciar el estado del tercer logro

        PlayerPrefs.SetInt("LogroObtenido2", 0); // Reiniciar el estado del primer logro
        PlayerPrefs.SetInt("LogroSinFallar2", 0); // Reiniciar el estado del segundo logro
        PlayerPrefs.SetInt("LogroPuntaje2", 0); // Reiniciar el estado del tercer logro
        PlayerPrefs.Save(); // Asegurarse de que se guarde inmediatamente

        // Ocultar las imágenes de los logros
        if (logroImage != null)
        {
            logroImage.gameObject.SetActive(false);
        }
        if (secondLogro != null)
        {
            secondLogro.gameObject.SetActive(false);
        }
        if (thirdLogro != null)
        {
            thirdLogro.gameObject.SetActive(false);
        }
        if (logroImage2 != null)
        {
            logroImage2.gameObject.SetActive(false);
        }
        if (secondLogro2 != null)
        {
            secondLogro2.gameObject.SetActive(false);
        }
        if (thirdLogro2 != null)
        {
            thirdLogro2.gameObject.SetActive(false);
        }
    }
}
