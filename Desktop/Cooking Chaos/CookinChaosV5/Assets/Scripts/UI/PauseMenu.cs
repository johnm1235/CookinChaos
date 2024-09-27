using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused = false;

    void Start()
    {
        // Asegurarse de que los elementos hijos del men� de pausa est�n desactivados al inicio del juego
        SetChildrenActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        SetChildrenActive(false);
        Time.timeScale = 1f; // Reanudar el tiempo del juego
        isPaused = false;
    }

    void Pause()
    {
        SetChildrenActive(true);
        Time.timeScale = 0f; // Detener el tiempo del juego
        isPaused = true;
    }

    private void SetChildrenActive(bool isActive)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(isActive);
        }
    }

    // M�todo para ser llamado desde el bot�n de men� principal
    public void OnMainMenuButtonPressed()
    {
        Time.timeScale = 1f; // Asegurarse de que el tiempo del juego est� reanudado antes de cambiar de escena
        SceneManager.LoadScene("MainMenu"); // Cambiar "MainMenu" por el nombre de tu escena del men� principal
        Destroy(ScoreManager.Instance.gameObject); // Destruir el objeto ScoreManager para reiniciar el puntaje
    }

}
