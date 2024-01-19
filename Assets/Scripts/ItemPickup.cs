using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // Enumeración que define los diferentes tipos de objetos que se pueden recoger
    public enum ItemType
    {
        ExtraBomb,
        BlastRadius,
        SpeedIncrease
    }

    // Variable que almacena el tipo de objeto que se está recogiendo
    public ItemType type;

    private void OnItemPickup(GameObject player)
    {
        switch (type)
        {
            case ItemType.ExtraBomb:
                player.GetComponent<BombController>().AddBomb(); // Añade una bomba al jugador
                break;
            case ItemType.BlastRadius:
                player.GetComponent<BombController>().explosionRadius++; // Aumenta el radio de explosión en 1
                break;
            case ItemType.SpeedIncrease:
                player.GetComponent<MovementController>().AddSpeed(); // Añade la velocidad del personaje 0.10f
                break;
        }

        Destroy(gameObject); // Trás coger el item destruye el objeto

    }

    // Función que se llama cuando el objeto (jugador) colisiona con el item generado tras la destrucción del destructible
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnItemPickup(other.gameObject);

        }
    }
}
