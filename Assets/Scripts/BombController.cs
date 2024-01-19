using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombController : MonoBehaviour
{
    [Header("Bomb")]
    public KeyCode inputKey = KeyCode.Space; // Tecla que se asigna para colocar la bomba
    public GameObject bombPrefab; // Prefab de la bomba
    public float bombFuseTime = 3f; // Tiempo que tarda la bomba en explotar
    public int bombAmount = 1; // Cantidad de bombas inicial
    private int bombsRemaining; // Bombas restantes (se utiliza para verificar)

    [Header("Explosion")]
    public Explosion explosionPrefab;
    public LayerMask explosionLayerMask;
    public float explosionDuration = 1f; // Duración de la explosión
    public int explosionRadius = 1; // Radio de la explosión
    public AudioSource src;
    public AudioClip explosionClip; // Sonido de la explosión

    [Header("Destructible")]
    public Tilemap destructiblesTiles; // Tilemap de objetos destructibles
    public Destructible destructiblePrefab; // Prefab Destructible


    private void OnEnable()
    {
        bombsRemaining = bombAmount; // Las bombas restantes iniciales seran las cantidad determinada de bombas asignada inicialmente
    }

    // Este método se ejecuta en cada frame del juego.
    private void Update()
    {
        // Verifica si hay bombas restantes y si se ha presionado la tecla de entrada.
        if (bombsRemaining > 0 && Input.GetKeyDown(inputKey))
        {
            // Inicia una corrutina para colocar una bomba.
            //Las corrutinas en Unity permiten ejecutar código de manera asíncrona y se utilizan
            // comúnmente para realizar acciones a lo largo del tiempo, como la colocación de una bomba en este caso.
            StartCoroutine(PlaceBomb());
        }
    }

    // Corrutina para colocar una bomba y gestionar su explosión
    private IEnumerator PlaceBomb()
    {

        // Obtiene la posición actual del jugador que pone la bomba y redondea las coordenadas.
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        // Instancia una bomba en la posición redondeada
        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        // Reduce el número de bombas restantes
        bombsRemaining--;

        // Espera el tiempo de explosión de la bomba
        yield return new WaitForSeconds(bombFuseTime);

        // Obtiene la posición de la bomba y redondea las coordenadas.
        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        // Instancia una explosión en la posición de la bomba.
        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        // Activa el renderizador correspondiente al inicio de la explosión
        explosion.SetActiveRenderer(explosion.start);
        // Destruye la explosión después de una determinada duración
        explosion.DestroyAfter(explosionDuration);

        // Realiza explosiones en todas las direcciones desde la posición de la bomba
        Explode(position, Vector2.up, explosionRadius);
        Explode(position, Vector2.down, explosionRadius);
        Explode(position, Vector2.left, explosionRadius);
        Explode(position, Vector2.right, explosionRadius);

        // Destruye la bomba después de la explosión
        Destroy(bomb);

        // Incrementa el número de bombas restantes
        bombsRemaining++;
    }

    // Método para realizar una explosión en una dirección desde una posición dada
    private void Explode(Vector2 position, Vector2 direction, int length)
    {
        // Verifica si la longitud de la explosión es igual o menor a cero.
        if (length <= 0)
        {
            return;
        }

        // Obtiene la nueva posición en la explotará la bomba
        position += direction;

        // Verifica si hay objetos destructibles en la posición actual, y los destruye
        // Además de ello, impide que se instancien explosiones si tiene un muro delante, ya sea destructible o no, en caso de ser destructible
        // lo elimina
        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask))
        {
            ClearDestructible(position);
            return;
        }

        // Instancia una explosión en la posición actual
        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        // Activa el renderizador correspondiente según la longitud de la explosión
        explosion.SetActiveRenderer(length > 1 ? explosion.middle : explosion.end);
        // Establece la dirección de la explosión
        explosion.SetDirection(direction);
        // Destruye la explosión después de cierta duración
        explosion.DestroyAfter(explosionDuration);
        
        // Realiza recursivamente explosiones en la misma dirección con longitud reducida
        Explode(position, direction, length - 1);

        // Reproduce el sonido de la explosión
        src.clip = explosionClip;
        src.Play();

    }

    // Quita el Tile del TileMap
    private void ClearDestructible(Vector2 position)
    {
        // Convierte la posición del vector en una celda del TileMap de Destructibles.
        Vector3Int cell = destructiblesTiles.WorldToCell(position);
        // Obtiene el Tile de esa celda
        TileBase tile = destructiblesTiles.GetTile(cell);

        if (tile != null)
        {
            // Instancia la animación de la destrucción del Tile
            Instantiate(destructiblePrefab, position, Quaternion.identity);

            // Quita el Tile
            destructiblesTiles.SetTile(cell, null);
        }
    }

    // Añade una bomba al jugador
    public void AddBomb()
    {
        bombAmount++;
        bombsRemaining++;
    }

    // Desactiva que el objeto bomba sea un Trigger (que puedas pasar por encima)
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            other.isTrigger = false;
        }
    }
}
