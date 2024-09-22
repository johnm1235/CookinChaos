using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject achievements;
    private void Start()
    {
        optionsMenu.SetActive(false);
        achievements.SetActive(false);
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
