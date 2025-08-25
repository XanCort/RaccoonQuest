using UnityEngine;
using UnityEngine.SceneManagement;
//Clase para el menú principal del juego cuenta con 3 botones
public class MainMenu : MonoBehaviour
{
    [SerializeField] private string firstLevelName = "Nivel1";
    //Si se pulsa el botón Start el juego empezará por el primer nivel
    public void StartGame()
    {
        // Empieza desde el primer nivel
        PlayerPrefs.SetInt("SavedLevel", SceneManager.GetSceneByName(firstLevelName).buildIndex);
        PlayerPrefs.Save();

        SceneManager.LoadScene(firstLevelName);
    }
    //Si se pulsa el botón Load comprobaremos si el usuario ha jugado antes y lo mandaremos al nivel en el que se quedó
    public void LoadGame()
    {
        //Comprobamos si hay algun nivel guardado y lo cargamos
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            string lastLevel = PlayerPrefs.GetString("LastLevelName", "Level1"); // Level1 por defecto
            SceneManager.LoadScene(lastLevel);
        }
        else
        {
            Debug.Log("No hay partida guardada. Empezando desde el principio.");
            StartGame();
        }
    }
    //Cerramos la aplicacion
    public void QuitGame()
    {
        Debug.Log("Salir del juego");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
