using UnityEngine;

//Clase de botón que se activa una sola vez, aunque los objetos se quiten de el seguirá activado
public class ButtonSwitch : MonoBehaviour
{
    [SerializeField] private GravityZone zonaGravedad;
    [SerializeField] private Sprite botonNormal;       // button_0
    [SerializeField] private Sprite botonPresionado;   // button_1
    [SerializeField] private float pixelesBajada = 3f; // cuántos píxeles baja
    [SerializeField] private float pixelsPerUnit = 100f; // igual que tu sprite importado

    private SpriteRenderer sr;
    private BoxCollider2D box;
    private bool yaActivado = false;
    private Vector2 colliderOffsetOriginal;
    //Método que se activa al cargar eel objeto, establece los colliders y el sprite controller
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();
        sr.sprite = botonNormal;
        colliderOffsetOriginal = box.offset;
    }
    //Método que se activa cuando un collider entra en contacto con el objeto
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //El botón solo tiene que activarse una vez, si ya lo está ignoramos esto
        if (yaActivado) return;
        //Si el jugador o una caja entran en contacto con el botón este se activa
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Caja"))
        {
            // Activa la zona de gravedad
            zonaGravedad.ActivarZona(true);

            // Cambia sprite
            sr.sprite = botonPresionado;

            // Calcula bajada en Unity Units
            float bajada = pixelesBajada / pixelsPerUnit;

            // Mueve el collider hacia abajo
            box.offset = new Vector2(colliderOffsetOriginal.x, colliderOffsetOriginal.y - bajada);

            yaActivado = true;
            Debug.Log("Botón presionado → Zona activada, sprite cambiado y collider bajado");
        }
    }
}
