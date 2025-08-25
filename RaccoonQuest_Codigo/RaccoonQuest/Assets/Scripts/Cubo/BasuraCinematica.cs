using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class BasuraCinematica : MonoBehaviour
{
    public PlayableDirector director;
    public CameraFollow cameraFollow; // referencia a tu script CameraFollow
    public string nivelSiguiente = "Level1"; // nombre de la escena a cargar

    //Método que se activa al entrar en contacto con otro collider, comprobamos si es el player y lanzamos la cinematica
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (cameraFollow != null)
                cameraFollow.enabled = false;

            director.stopped += OnCinematicaTerminada;
            director.Play();
        }
    }
    //Método que se ejecuta al acabar la cinematica, devuelve el control de la camara y lanza el siguiente nivel
    private void OnCinematicaTerminada(PlayableDirector obj)
    {
        // Reactivar la cámara
        if (cameraFollow != null)
            cameraFollow.enabled = true;

        // Desuscribimos el evento
        director.stopped -= OnCinematicaTerminada;

        // Cargar el siguiente nivel
        SceneManager.LoadScene(nivelSiguiente);
    }
}
