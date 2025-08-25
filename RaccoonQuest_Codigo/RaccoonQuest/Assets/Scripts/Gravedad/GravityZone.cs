using UnityEngine;
using System.Collections;

public class GravityZone : MonoBehaviour
{
    [Header("Gravedad")]
    [SerializeField] private float invertedGravityScale = -1f;
    [SerializeField] private float normalGravityScale = 1f;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string cajaTag = "Caja";

    [Header("Visual")]
    [SerializeField] private SpriteRenderer spriteRenderer; 
    [SerializeField] private Color colorDesactivado = Color.gray;
    [SerializeField] private Color colorActivado = Color.red;

    [Header("Temporizador")]
    [SerializeField] public bool permanente = false;  
    [SerializeField] private float duracion = 5f;      
    [SerializeField] private float tiempoParpadeo = 3f;

    private Coroutine temporizador;
    private float tiempoRestante;
    public bool activo = false;

    private void Start()
    {
        spriteRenderer.color = activo ? colorActivado : colorDesactivado;
    }

    public void ActivarZona(bool estado)
    {
        activo = estado;
        spriteRenderer.color = activo ? colorActivado : colorDesactivado;

        // Aplica la gravedad de inmediato a todos los objetos dentro
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, GetComponent<Collider2D>().bounds.size, 0f);
        foreach (var col in colliders)
        {
            if (col.CompareTag(playerTag) || col.CompareTag(cajaTag))
            {
                Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
                if (rb != null)
                    rb.gravityScale = activo ? invertedGravityScale : normalGravityScale;
            }
        }

        if (activo && !permanente)
        {
            if (temporizador != null) StopCoroutine(temporizador);
            temporizador = StartCoroutine(DesactivarTrasTiempo());
        }
        else if (!activo && temporizador != null)
        {
            StopCoroutine(temporizador);
            temporizador = null;
        }
    }

    private IEnumerator DesactivarTrasTiempo()
    {
        tiempoRestante = duracion;
        while (tiempoRestante > 0)
        {
            if (tiempoRestante <= tiempoParpadeo)
            {
                float t = Mathf.PingPong(Time.time * 5f, 1f);
                spriteRenderer.color = Color.Lerp(colorActivado, colorDesactivado, t);
            }
            else
            {
                spriteRenderer.color = colorActivado;
            }

            tiempoRestante -= Time.deltaTime;
            yield return null;
        }

        ActivarZona(false);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (activo && (other.CompareTag(playerTag) || other.CompareTag(cajaTag)))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.gravityScale = invertedGravityScale;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag) || other.CompareTag(cajaTag))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.gravityScale = normalGravityScale;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = activo ? Color.red : Color.gray;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
    }
}
