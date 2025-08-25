using UnityEngine;
using UnityEngine.UI;
//Clase que sirve para abrir y cerrar el cartel
public class InteraccionCartel : MonoBehaviour
{
    [SerializeField] private GameObject panelCartel; // Panel UI que se activa al interactuar
    [SerializeField] private Image imagenCartel;     // UI Image que mostrará la ilustración

    private CartelController cartelActual;
    //En cada frame comprueba si se ha interactuado con el
    void Update()
    {
        // Si estamos cerca de un cartel y pulsamos "E" mostramos el contenido
        if (cartelActual != null && Input.GetKeyDown(KeyCode.E))
        {
            panelCartel.SetActive(true);
            imagenCartel.sprite = cartelActual.GetSprite(); 
        }

        // Para cerrar el cartel pulsamos Esc
        if (panelCartel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            panelCartel.SetActive(false);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Cartel"))
        {
            cartelActual = other.GetComponent<CartelController>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Cartel"))
        {
            cartelActual = null;
        }
    }
}