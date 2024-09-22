using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    public void BackToMenu()
    {
        // Cargar la escena del men� principal
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    private void Start()
    {
        pauseMenu.SetActive(false);
        // Ocultar el men� de opciones
        optionsMenu.SetActive(false);
    }

    public void Update()
    {
        // Si se presiona la tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Pausar o reanudar el juego
            if (Time.timeScale == 0)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        // Pausar el juego
        Time.timeScale = 0;
        pauseMenu.SetActive(true);

    }

    public void ResumeGame()
    {
        // Reanudar el juego
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

}
