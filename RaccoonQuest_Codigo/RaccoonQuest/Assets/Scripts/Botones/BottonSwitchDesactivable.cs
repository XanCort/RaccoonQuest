using UnityEngine;

/***
    Clase para botónes que necesitan de una cantidad de objetos encima para activarse
    Controla el movimiento de plataformas
    Si los objetos salen de encima del botón el funcionamiento se detiene
**/
public class ButtonSwitchDesactivable : MonoBehaviour
{
    [SerializeField] private PlataformaMovil plataforma;
    [SerializeField] private Sprite botonNormal;
    [SerializeField] private Sprite botonPresionado;
    [SerializeField] private float pixelesBajada = 3f;
    [SerializeField] private float pixelsPerUnit = 100f;

    private SpriteRenderer sr;
    private PolygonCollider2D polyCollider;
    private Vector2 colliderOffsetOriginal;

    //Número de objetos encima del botón
    public int objetosEncima = 0;
    //Número de objetos necesarios para la activación del botón
    public int objetosNecesarios = 1;

    //Método que se ejecuta cuando carga el objeto, establece los controladores de sprite y los collider
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        polyCollider = GetComponent<PolygonCollider2D>();
        if (polyCollider == null)
        {
            polyCollider = gameObject.AddComponent<PolygonCollider2D>();
        }

        sr.sprite = botonNormal;
        colliderOffsetOriginal = polyCollider.offset;
    }
    //Método que comprueba si algo ha entrado en contacto con el botón
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Actualizamos el estado del botón en caso de que el jugador o una caja entren en contacto con el
        if (other.CompareTag("Player") || other.CompareTag("Caja"))
        {
            objetosEncima++;
            ActualizarEstado();
        }
    }
    //Método que se activa cuando un collider deja de estar en contacto con el objeto
    private void OnTriggerExit2D(Collider2D other)
    {
        //Si el jugador o una caja dejan de estar encima del botón actualizamos el estado
        if (other.CompareTag("Player") || other.CompareTag("Caja"))
        {
            objetosEncima--;
            ActualizarEstado();
        }
    }
    //Método que comprueba si hay suficientes objetos para activar la funcion del botón
    private void ActualizarEstado()
    {
        //En caso de que los objetos sean sufientes activamos en este caso la plataforma
        if (objetosEncima >= objetosNecesarios)
        {
            if (plataforma != null)
                plataforma.ActivarPlataforma();
            //Cambiamos el sprite del botón a botón pulsado
            sr.sprite = botonPresionado;
            float bajada = pixelesBajada / pixelsPerUnit;
            polyCollider.offset = new Vector2(colliderOffsetOriginal.x, colliderOffsetOriginal.y - bajada);

            Debug.Log("Botón presionado → Plataforma activada");
        }
        //Si los objetos no son suficientes o alguno deja de estar en contacto con el botón la desactivamos (Si hacen falta 4 y una caja se cae el botón se desactiva)
        else
        {
            if (plataforma != null)
                plataforma.DesactivarPlataforma();

            sr.sprite = botonNormal;
            polyCollider.offset = colliderOffsetOriginal;

            Debug.Log("Botón liberado → Plataforma detenida");
        }
    }
}
