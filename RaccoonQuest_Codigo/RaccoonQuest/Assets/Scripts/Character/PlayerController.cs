using UnityEngine;
using UnityEngine.SceneManagement;
//Clase que controla el movimiento y las animaciones del personaje principal
public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D playerRb;

    public float speed = 20.0f;
    public bool isGrounded = false;
    public bool isPushing = false;

    private float forwardInput;
    private Vector2 plataformaDelta = Vector2.zero;

    //Utilizamos un objeto para comprobar si el jugador está en tierra, utilizar el onExitCollision y onEnterCollision daba problemas con paredes verticales
    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;
    private bool wasGroundedLastFrame = true;

    [Header("Jump Settings")]
    public float jumpForce = 3f;
    public float coyoteTime = 0.1f;
    private float coyoteTimeCounter;

    private bool isJumpingAnim = false; // Controla animación de salto
    //Método que se activa al cargar el objeto carga el animador y el rigbody
    void Start()
    {
        anim = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody2D>();
    }
    //Método que se activa en intervalos de tiempo fijos
    void FixedUpdate()
    {
        // Actualiza estado de suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Coyote time: reinicia si está en el suelo
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.fixedDeltaTime;
        }

        // Detecta aterrizaje (solo cuando venimos de estar en el aire y habíamos saltado)
        if (isGrounded && !wasGroundedLastFrame && isJumpingAnim)
        {
            anim.SetTrigger("StopWalking"); // Termina animación de salto
            isJumpingAnim = false;
        }

        wasGroundedLastFrame = isGrounded;

        // Movimiento horizontal
        forwardInput = Input.GetAxis("Horizontal");
        Vector2 inputVelocity = new Vector2(forwardInput * speed, playerRb.linearVelocity.y);
        playerRb.linearVelocity = inputVelocity;

        // Herencia de movimiento por delta
        if (plataformaDelta != Vector2.zero)
        {
            playerRb.position += plataformaDelta;
        }

        // Flip horizontal
        if (forwardInput > 0.1f)
            transform.localScale = new Vector3(1, transform.localScale.y, 1);
        else if (forwardInput < -0.1f)
            transform.localScale = new Vector3(-1, transform.localScale.y, 1);

        // Flip vertical según gravedad
        float yScale = (playerRb.gravityScale > 0) ? Mathf.Abs(transform.localScale.y) : -Mathf.Abs(transform.localScale.y);
        transform.localScale = new Vector3(transform.localScale.x, yScale, transform.localScale.z);

        // Animación de caminar
        bool walking = Mathf.Abs(forwardInput) > 0.1f && isGrounded && !isPushing;
        anim.SetBool("Walking", walking);
    }
    //Método que se activa en cada Frame, lo utilizamos para realizar saltos y reinicio del nivel
    void Update()
    {
        // Salto con coyote time, comprobamos si ha estado fuera de una plataforma demasiado tiempo, en caso de que no permitimos saltar
        if (Input.GetKeyDown(KeyCode.Space) && coyoteTimeCounter > 0f)
        {
            Vector2 jumpDirection = (playerRb.gravityScale > 0) ? Vector2.up : Vector2.down;
            playerRb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
            coyoteTimeCounter = 0f; // Evita doble salto
            isJumpingAnim = true;
            anim.SetTrigger("Jump"); // Activamos animación de salto
        }

        // Reiniciar nivel
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    //Método que comprueba si permanecemos en colision con distintos objetos
    void OnCollisionStay2D(Collision2D collision)
    {
        //Establecemos las variables con el valor inicial para hacer las comrpobaciones de forma correcta
        isPushing = false;
        plataformaDelta = Vector2.zero;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            Vector2 normal = contact.normal;

            // Detecta si está empujando una caja para activar la animacion de push
            if (Mathf.Abs(normal.x) > 0.5f && collision.collider.CompareTag("Caja"))
            {
                isPushing = true;
                Debug.Log("Empujando caja");
            }

            // Herencia de movimiento desde caja
            if (collision.collider.CompareTag("Caja") && normal.y > 0.5f)
            {
                MovableBox mb = collision.collider.GetComponent<MovableBox>();
                if (mb != null)
                {
                    plataformaDelta = mb.GetDeltaVelocity() * Time.fixedDeltaTime;
                }
            }
            // Herencia de movimiento desde plataforma
            else if (collision.collider.CompareTag("Plataforma") && normal.y > 0.5f)
            {
                VelocityCalculator vc = collision.collider.GetComponent<VelocityCalculator>();
                if (vc != null)
                {
                    plataformaDelta = vc.GetVelocity() * Time.fixedDeltaTime;
                }
            }
        }

        anim.SetBool("Pushing", isPushing);
    }
    //Método que detecta cuando dejamos de estar en colision con otro objeto
    void OnCollisionExit2D(Collision2D collision)
    {
        //Comprobamos si dejamos de estar en contacto con una caja para cancelar la animación de empujar
        if (collision.collider.CompareTag("Caja")){
            isPushing=false;
            anim.SetBool("Pushing", false);
            Debug.Log("Dejamos de empujar caja");
        }
        
        //Comprobamos si dejamos de estar en contacto con una plataforma o caja para cancelar la herencia de velocidad
        if (collision.collider.CompareTag("Plataforma") || collision.collider.CompareTag("Caja"))
        {
            plataformaDelta = Vector2.zero;
            Debug.Log("Salimos de plataforma");
        }

        Debug.Log("Contacto perdido");
    }

    //Método auxiliar de depuracion para activar de forma visual comprobaciones
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            //Nos permite observar cuanto radio de colision tiene el comprobador del suelo
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
