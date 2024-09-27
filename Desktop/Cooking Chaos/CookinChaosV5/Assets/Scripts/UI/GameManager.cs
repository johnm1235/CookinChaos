using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Configuraciones globales del juego
    public float Volume { get; private set; } = 1.0f; // Volumen del sonido (0.0 a 1.0)

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Asegura que el GameManager persista entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        LoadLevel("Level1");
    }

    public void ExitGame()
    {
        Application.Quit();
       // EditorApplication.isPlaying = false;
    }

    public void LoadLevel(string levelName)
    {
        Debug.Log("Cargando nivel: " + levelName);
        SceneManager.LoadScene(levelName);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        PauseGame();
    }

    public void GameWin()
    {
        PauseGame();
    }
}
