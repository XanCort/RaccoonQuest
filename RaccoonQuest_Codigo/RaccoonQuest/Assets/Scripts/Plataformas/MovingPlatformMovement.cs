using UnityEngine;
//Clase que utilizamos para manejar el movimiento de las plataformas, tiene varios estados se pueden asignar a botones y podemos visualizar el recorrido que va a hacer
public class PlataformaMovil : MonoBehaviour
{
    [SerializeField] private float velocidad = 2f;

    [Header("Recorrido por coordenadas")]
    //Coordenadas para el inicio y el fin del moviemiento
    [SerializeField] private Vector3 puntoInicio;
    [SerializeField] private Vector3 puntoFin;

    [Header("Activación")]
    [SerializeField] private bool activo = true;
    private Vector3 ultimaPos;
    private int direccion = 1; // 1 = hacia puntoFin, -1 = hacia puntoInicio
    //Método que se utiliza al cargar el objeto lo coloca en su posición inicial
    void Start()
    {
        transform.position = puntoInicio;
        ultimaPos = transform.position;
    }
    //Método que se utiliza en períodos de tiempo concretos
    void FixedUpdate()
    {
        //Si no está activa no hace naada
        if (!activo) return;

        // Avanzar en la dirección actual
        Vector3 destino = (direccion == 1) ? puntoFin : puntoInicio;
        Vector3 nuevaPos = Vector3.MoveTowards(transform.position, destino, velocidad * Time.fixedDeltaTime);

        // Calcula delta
        Vector3 delta = nuevaPos - transform.position;

        transform.position = nuevaPos;

        // Empuja jugadores encima
        Collider2D[] jugadores = Physics2D.OverlapBoxAll(
            transform.position,
            GetComponent<BoxCollider2D>().size,
            0f
        );
        //Comprueba si hay jugadores o cajas en contacto con ella para transmitirles el movimiento
        foreach (var col in jugadores)
        {
            if (col.CompareTag("Player") || col.CompareTag("Caja"))
            {
                Rigidbody2D rbJugador = col.GetComponent<Rigidbody2D>();
                if (rbJugador != null)
                    rbJugador.position += new Vector2(delta.x, delta.y);
            }
        }

        ultimaPos = transform.position;

        // Si ha llegado al destino, cambiar dirección
        if (Vector3.Distance(transform.position, destino) < 0.01f)
        {
            direccion *= -1;
        }
    }

    public void ActivarPlataforma()
    {
        activo = true;
        // No reseteamos dirección seguirá desde donde iba
    }

    public void DesactivarPlataforma()
    {
        activo = false;
    }
    //Método para visualizar el recorrido que hace la plataforma
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(puntoInicio, puntoFin);
        Gizmos.DrawSphere(puntoInicio, 0.1f);
        Gizmos.DrawSphere(puntoFin, 0.1f);
    }
}
