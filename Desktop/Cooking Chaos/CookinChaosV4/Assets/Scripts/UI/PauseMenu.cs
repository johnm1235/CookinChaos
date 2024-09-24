using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;

    public VideoPlayer videoPlayer;
    public void BackToMenu()
    {
        // Cargar la escena del menú principal
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    private void Start()
    {
        pauseMenu.SetActive(false);
        // Ocultar el menú de opciones
        optionsMenu.SetActive(false);
        videoPlayer.gameObject.SetActive(false);
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

    public void Tutorial()
    {
        // Mostrar tutorial
        videoPlayer.gameObject.SetActive(true);
        videoPlayer.Play();
    }

    public void CloseTutorial()
    {
        // Cerrar tutorial
        videoPlayer.Stop();
        videoPlayer.gameObject.SetActive(false);
    }

    public void PauseGame()
    {
        // Pausar el juego
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(true);

    }

    public void ResumeGame()
    {
        // Reanudar el juego
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }
}
