using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel; // referencia al panel del menú
    private bool isPaused = false;

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
        pausePanel.SetActive(false);
        Time.timeScale = 1f; // reanudar juego
        isPaused = false;
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // pausa el juego
        isPaused = true;
    }

    public void ExitGame()
    {
        // Si estás en el editor, no funciona el quit
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); 
#endif
    }
}
