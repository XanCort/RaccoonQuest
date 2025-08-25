using UnityEngine;
//Clase usada para las fisicas de las cajas
[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class MovableBox : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 previousPosition;
    private Vector2 inheritedVelocity = Vector2.zero;
    //Método que se activa al cargar el objeto, establece los collider
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        previousPosition = rb.position;
    }
    //Método que se ejecuta en periodos de tiempo concretos, lo utilizamos para heredar la velocidad de las plataformas sobre las que este la caja y transmitirsela al jugador
    void FixedUpdate()
    {
        // Aplica la velocidad heredada de la plataforma
        if (inheritedVelocity != Vector2.zero)
        {
            rb.MovePosition(rb.position + inheritedVelocity * Time.fixedDeltaTime);
        }

        // Calcula delta de movimiento de esta caja
        Vector2 delta = rb.position - previousPosition;
        previousPosition = rb.position;

        // Propaga delta a jugadores o cajas encima
        Collider2D[] encima = Physics2D.OverlapBoxAll(
            rb.position,
            GetComponent<BoxCollider2D>().size,
            0f
        );

        foreach (var col in encima)
        {
            //Comprobamos si el jugador está encima para heredarle velocidad
            if (col.CompareTag("Player"))
            {
                Rigidbody2D rbJugador = col.GetComponent<Rigidbody2D>();
                if (rbJugador != null)
                    rbJugador.position += new Vector2(delta.x, delta.y);
            }
        }

        // Reset de velocidad heredada
        inheritedVelocity = Vector2.zero;
    }
    //Método que comprueba si el collider sigue en contacto con otro, en este caso lo usamos para recibir velocidad de la plataforma y transmitirsela a lo que tengamos encima
    void OnCollisionStay2D(Collision2D collision)
    {
        // Hereda velocidad si está sobre una plataforma o caja
        if (collision.collider.CompareTag("Plataforma"))
        {
            VelocityCalculator vc = collision.collider.GetComponent<VelocityCalculator>();
            if (vc != null)
            {
                Vector3 v = vc.GetVelocity();
                inheritedVelocity = new Vector2(v.x, v.y);
            }
        }
        else if (collision.collider.CompareTag("Caja"))
        {
            MovableBox mb = collision.collider.GetComponent<MovableBox>();
            if (mb != null)
            {
                inheritedVelocity = mb.GetDeltaVelocity();
            }
        }
    }

    // Permite aplicar delta desde abajo (otra caja encima)
    public void ApplyDelta(Vector2 delta)
    {
        rb.MovePosition(rb.position + delta);
    }

    // Método para que otras cajas hereden movimiento
    public Vector2 GetDeltaVelocity()
    {
        return (rb.position - previousPosition) / Time.fixedDeltaTime;
    }
}
