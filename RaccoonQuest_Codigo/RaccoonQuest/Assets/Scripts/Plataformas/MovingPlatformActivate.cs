using UnityEngine;

public class MovingPlatformActivate : MonoBehaviour
{
    [SerializeField] private float velocidad = 2f;
    [SerializeField] private float distancia = 3f;

    private Vector3 puntoInicialOriginal; // posición donde comenzó la plataforma
    public bool activo = false;
    private float tiempoInicio;

    void Start()
    {
        puntoInicialOriginal = transform.position;
        tiempoInicio = Time.time;
    }

    void FixedUpdate()
    {
        if (!activo) return;

        // PingPong siempre relativo al punto inicial original
        float desplazamiento = Mathf.PingPong((Time.time - tiempoInicio) * velocidad, distancia);
        Vector3 nuevaPos = puntoInicialOriginal + new Vector3(0f, desplazamiento, 0f);

        // Calcula delta para empujar jugadores
        Vector3 delta = nuevaPos - transform.position;
        transform.position = nuevaPos;

        Collider2D[] jugadores = Physics2D.OverlapBoxAll(
            transform.position,
            GetComponent<BoxCollider2D>().size,
            0f
        );

        foreach (var col in jugadores)
        {
            if (col.CompareTag("Player"))
            {
                Rigidbody2D rbJugador = col.GetComponent<Rigidbody2D>();
                if (rbJugador != null)
                    rbJugador.position += new Vector2(delta.x, delta.y);
            }
        }
    }

    public void ActivarPlataforma()
    {
        activo = true;
        // Reinicia el tiempo para que PingPong empiece desde la posición actual
        tiempoInicio = Time.time - (transform.position.y - puntoInicialOriginal.y) / velocidad;
    }

    public void DesactivarPlataforma()
    {
        activo = false;
    }
}
