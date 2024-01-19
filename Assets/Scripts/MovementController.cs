using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Rigidbody2D Rigidbody2D { get; private set; }
    private Vector2 direction = Vector2.down; // Dirección de movimiento 
    public float speed = 4f; // Velocidad de movimiento inicial
    public KeyCode inputUp = KeyCode.W; // Tecla Arriba
    public KeyCode inputDown = KeyCode.S; // Tecla Abajo
    public KeyCode inputRight = KeyCode.D; // Tecla Derecha
    public KeyCode inputLeft = KeyCode.A; // Tecla Izquierda

    public AudioSource src;
    public AudioSource src2;
    public AudioClip deathClip; // Audio que sonará cuando un jugador muera
    public AudioClip stepClip; // Audio que sonará cuando un jugador este caminando

    public AnimatedSpriteRenderer spriteRendererUp; // Animación que se usa cuando el jugador camina hacia arriba
    public AnimatedSpriteRenderer spriteRendererDown; // Animación que se usa cuando el jugador camina hacia abajo
    public AnimatedSpriteRenderer spriteRendererLeft; // Animación que se usa cuando el jugador camina hacia la izquierda
    public AnimatedSpriteRenderer spriteRendererRight; // Animación que se usa cuando el jugador camina hacia la derecha
    public AnimatedSpriteRenderer spriteRendererDeath; // Animación que se usa cuando el jugador muere
    private AnimatedSpriteRenderer activeSpriteRenderer; // Animación que esta activa


    private void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>(); // Obtiene el objeto Rigidbody2D del jugador
        activeSpriteRenderer = spriteRendererDown; // Posición inicial
    }

    private void Update()
    {
        if (Input.GetKey(inputUp)) // Cuando el jugador camina hacia arriba
        {
            SetDirection(Vector2.up, spriteRendererUp);
            PlaySound();
        }
        else if (Input.GetKey(inputDown)) // Cuando el jugador camina hacia abajo
        {
            SetDirection(Vector2.down, spriteRendererDown);
            PlaySound();
        }
        else if (Input.GetKey(inputRight)) // Cuando el jugador camina hacia la derecha
        {
            SetDirection(Vector2.right, spriteRendererRight);
            PlaySound();
        }
        else if (Input.GetKey(inputLeft)) // Cuando el jugador camina hacia la izquierda
        {
            SetDirection(Vector2.left, spriteRendererLeft);
            PlaySound();
        }
        else // Cuando el jugador esta quieto
        {
            SetDirection(Vector2.zero, activeSpriteRenderer);
            StopSound();
        }
    }
    // Función que reproduce el sonido de los pasos cuando el jugador esta caminando
    private void PlaySound()
    {
        if (!src2.isPlaying)
        {
            src2.clip = stepClip;
            src2.loop = true;
            src2.Play();
        }
    }

    // Función que para el sonido de los pasos cuando el jugador se detiene
    private void StopSound()
    {
        if (src2.isPlaying)
        {
            src2.Stop();
        }
    }

    private void FixedUpdate()
    {
        // Obtén la posición actual del jugador
        Vector2 position = Rigidbody2D.position;
        // Calcula el desplazamiento basado en la velocidad, el tiempo fijo y la dirección
        Vector2 translation = speed * Time.fixedDeltaTime * direction;
        // Mueve al jugador a la nueva posición sumando el desplazamiento calculado
        Rigidbody2D.MovePosition(position + translation);
    }



    private void SetDirection(Vector2 newDirection, AnimatedSpriteRenderer spriteRenderer)
    {
        // Establece la nueva dirección de movimiento
        direction = newDirection;

        // Establece la visibilidad de la animación según la dirección en la que se mueva el jugador
        spriteRendererUp.enabled = spriteRenderer == spriteRendererUp;
        spriteRendererDown.enabled = spriteRenderer == spriteRendererDown;
        spriteRendererLeft.enabled = spriteRenderer == spriteRendererLeft;
        spriteRendererRight.enabled = spriteRenderer == spriteRendererRight;

        // Asigna la animación activa al proporcionado
        activeSpriteRenderer = spriteRenderer;

        // Configura el estado "idle" (cuando el jugador esta parado existe un renderer 
        // donde se queda parado) del sprite renderer activo según la dirección.
        activeSpriteRenderer.idle = direction == Vector2.zero;
    }

    // Función que carga la animación de la muerte del jugador en caso de estar en contacto con la explosión
    private void OnTriggerEnter2D(Collider2D other)

    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            DeathSequence();
        }
    }

    // Animación de la muerte del jugador
    private void DeathSequence()
    {
        enabled = false;
        GetComponent<BombController>().enabled = false; // Desactiva que el jugador no pueda tirar bombas al morir

        // Desactiva todas las animaciones de movimiento
        spriteRendererUp.enabled = false;
        spriteRendererDown.enabled = false;
        spriteRendererLeft.enabled = false;
        spriteRendererRight.enabled = false;
        spriteRendererDeath.enabled = true;

        // Activa el sonido de muerte
        src.clip = deathClip;
        src.Play();

        // Invoca el método OnDeathSequenceEnded después de un retraso de 1.25 segundos.
        Invoke(nameof(OnDeathSequenceEnded), 1.25f);
    }

    // Función que desactiva el objeto jugador
    private void OnDeathSequenceEnded()
    {
        gameObject.SetActive(false);
        FindObjectOfType<GameManager>().CheckWinState(); // Comprueba los jugadores restantes
    }

    // Función que le añade velocidad de movimiento al jugador
    public void AddSpeed()
    {
        speed += 0.10f;
    }

}
