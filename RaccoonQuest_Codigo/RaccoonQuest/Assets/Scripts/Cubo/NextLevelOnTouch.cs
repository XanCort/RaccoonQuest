using UnityEngine;
using UnityEngine.SceneManagement;
//Clase que permite pasar al siguiente nivel al tocar un objeto concreto, cubos de basura en este caso
public class NextLevelOnTouch : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string nextSceneName = ""; // Si se deja vacio irá por el índice de niveles, se puede añadri el nombre la siguiente escena si se quiere otro orden
    [SerializeField] private float delay = 0f;         // Retraso opcional en segundos, de momento no lo uso por no tener animación
    private bool used = false;
    //Comprueba si el jugador ha entrado en contacto con el objeto
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Nos aseguramos de que no ocurra varias veces
        if (used) return;
        if (!other.CompareTag(playerTag)) return;

        used = true;
        if (delay <= 0f) LoadNext();
        else Invoke(nameof(LoadNext), delay);
    }
    //Cargamos el siguiente nivel
    private void LoadNext()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            // Guardamos progreso por nombre
            PlayerPrefs.SetString("SavedLevelName", nextSceneName);
            PlayerPrefs.Save();

            SceneManager.LoadScene(nextSceneName);
        }
    }
}
