using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject achievements;

    public VideoPlayer videoPlayer;
    
    private void Start()
    {
        optionsMenu.SetActive(false);
        achievements.SetActive(false);
        videoPlayer.gameObject.SetActive(false);
    }

    public void Init()
    {
        GameManager.Instance.StartGame();
    }

    public void Options()
    {
        // Mostrar menú de opciones
        optionsMenu.SetActive(true);
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

    public void Achievements()
    {
        achievements.SetActive(true);
    }

    public void ExitGame()
    {
        // Salir del juego
        GameManager.Instance.ExitGame();
    }

    public void Back()
    {
        // Cerrar menú de opciones
        optionsMenu.SetActive(false);
        achievements.SetActive(false);
    }

}
