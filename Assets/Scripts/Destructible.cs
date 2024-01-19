using UnityEngine;

public class Destructible : MonoBehaviour
{
    public float destructionTime = 1f; //  Tiempo en segundos después del cual el objeto gameObject se destruirá

    // Probabilidad de que un objeto se genere al destruir el objeto gameObject
    // El valor debe estar entre 0 y 1
    [Range(0f, 1f)]
    public float itemSpawnChance = 0.35f;

    public GameObject[] spawnableItems; // Array con los objetos que podrán ser instanciados


    private void Start()
    {
        Destroy(gameObject, destructionTime); // Destruye el objeto trás un tiempo determinado
    }

    private void OnDestroy()
    {
        // Comprueba que haya objetos que se puedan generar y si el número aleatorio es menor que la probalidad asignada
        if (spawnableItems.Length > 0 && Random.value < itemSpawnChance)
        {
            int randomIndex = Random.Range(0, spawnableItems.Length); // Selecciona un objeto del array de forma aleatoria
            Instantiate(spawnableItems[randomIndex], transform.position, Quaternion.identity); // Se genera (instancia) el item en la posición del objeto gameObject (Destructible) que se destruyó.
        }
    }

}
