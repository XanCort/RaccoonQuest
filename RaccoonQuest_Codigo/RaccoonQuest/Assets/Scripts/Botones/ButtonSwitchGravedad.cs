using UnityEngine;

public class ButtonSwitchGravedad : MonoBehaviour
{
    [SerializeField] private GravityZone[] zonasGravedad; // Permite asignar varias zonas de gravedad
    [SerializeField] private Sprite botonNormal;          // button_0
    [SerializeField] private Sprite botonPresionado;      // button_1
    [SerializeField] private float pixelesBajada = 3f;    // cuántos píxeles baja el botón
    [SerializeField] private float pixelsPerUnit = 100f;  // igual que tu sprite importado

    private SpriteRenderer sr;
    private PolygonCollider2D polyCollider;
    private Vector2 colliderOffsetOriginal;

    private int objetosEncima = 0; // contador de objetos sobre el botón

    //Método que se activa al cargar eel objeto, establece los colliders y el sprite controller
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

    //Método que se activa cuando un collider entra en contacto con el objeto
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Caja"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // Solo contar si la colisión viene desde arriba
                if (contact.normal.y < -0.5f)
                {
                    objetosEncima++;
                    ActivarBoton();
                    break;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Caja"))
        {
            objetosEncima--;
            if (objetosEncima <= 0)
            {
                // Volver al sprite normal al no haber nada encima
                sr.sprite = botonNormal;
                polyCollider.offset = colliderOffsetOriginal;

                // Para zonas temporales → la zona seguirá su temporizador
                foreach (var zona in zonasGravedad)
                {
                    if (zona != null && !zona.permanente)
                        zona.ActivarZona(true); // reinicia temporizador
                }
            }
        }
    }
    //método que se activa cuando el botón tiene objetos encima
    private void ActivarBoton()
    {
        bool hayPermanente = false;
        foreach (var zona in zonasGravedad)
        {
            if (zona != null && zona.permanente)
            {
                hayPermanente = true;
                break;
            }
        }
        //Si la zona tiene de estado permanente la activa de forma permanente
        if (hayPermanente)
        {
            // Toggle para zonas permanentes
            foreach (var zona in zonasGravedad)
            {
                if (zona != null && zona.permanente)
                    zona.ActivarZona(!zona.activo); // cambia el estado activo
            }
        }
        //Si la zona no es permanente activa la zona y su temporizador
        else
        {
            // Zonas temporales activar y reiniciar temporizador
            foreach (var zona in zonasGravedad)
            {
                if (zona != null && !zona.permanente)
                    zona.ActivarZona(true);
            }
        }

        // Siempre que se pulse, cambia visualmente el botón
        sr.sprite = botonPresionado;
        float bajada = pixelesBajada / pixelsPerUnit;
        polyCollider.offset = new Vector2(colliderOffsetOriginal.x, colliderOffsetOriginal.y - bajada);

        Debug.Log("Botón presionado Zonas de gravedad actualizadas");
    }
}
