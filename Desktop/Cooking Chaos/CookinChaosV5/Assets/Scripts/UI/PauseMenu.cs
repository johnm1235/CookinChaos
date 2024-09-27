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
        // Asegurarse de que los elementos hijos del menú de pausa estén desactivados al inicio del juego
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

    // Método para ser llamado desde el botón de menú principal
    public void OnMainMenuButtonPressed()
    {
        Time.timeScale = 1f; // Asegurarse de que el tiempo del juego esté reanudado antes de cambiar de escena
        SceneManager.LoadScene("MainMenu"); // Cambiar "MainMenu" por el nombre de tu escena del menú principal
        Destroy(ScoreManager.Instance.gameObject); // Destruir el objeto ScoreManager para reiniciar el puntaje
    }

}
