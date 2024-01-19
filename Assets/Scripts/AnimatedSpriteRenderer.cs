using UnityEngine;

public class AnimatedSpriteRenderer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; // Referencia al componente SpriteRenderer
    public Sprite idleSprite; // Sprite que se muestra cuando el objeto está inactivo
    public Sprite[] animationSprites; // Array de sprites que se muestran cuando el objeto está activo
    public float animationTime = 0.25f; // Tiempo entre cada cambio de sprite
    private int animationFrame; // Índice del sprite actual
    public bool loop = true; // Indica si la animación debe repetirse
    public bool idle = true; // Indica si el objeto está inactivo

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obtiene el componente SpriteRenderer del objeto
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true; // Habilita el componente SpriteRenderer
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false; // Deshabilita el componente SpriteRenderer
    }

    private void Start()
    {
        InvokeRepeating(nameof(NextFrame), animationTime, animationTime); // Invoca el método NextFrame() cada animationTime (0.25f) segundos
    }

    private void NextFrame()
    {
        animationFrame++; // Incrementa el índice del sprite actual 
        if (loop && animationFrame >= animationSprites.Length) // Si loop es verdadero y se llegó al final del array animationSprites el indice se pone a 0
        {
            animationFrame = 0;
        }

        if (idle)
        {
            spriteRenderer.sprite = idleSprite; // Muestra el sprite idleSprite (Cuando no esta en movimiento)

        }
        else if (animationFrame >= 0 && animationFrame < animationSprites.Length) // Si idle es falso y el índice del sprite actual está dentro del rango del arreglo animationSprites
        {
            spriteRenderer.sprite = animationSprites[animationFrame]; // Muestra el sprite en la posición animationFrame del array animationSprites
        }

    }
}
