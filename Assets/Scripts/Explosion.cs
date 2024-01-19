using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Variables para almacenar las animaciones de la explosión
    public AnimatedSpriteRenderer start;
    public AnimatedSpriteRenderer middle;
    public AnimatedSpriteRenderer end;

    // Función para activar la animación correspondiente según el parámetro renderer que se le pase
    public void SetActiveRenderer(AnimatedSpriteRenderer renderer)
    {
        start.enabled = renderer == start;
        middle.enabled = renderer == middle;
        end.enabled = renderer == end;
    }

    // Función para establecer la dirección de la explosión según el vector direction que se le pase
    public void SetDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    // Función para destruir el objeto después de un número determinado de segundos que se le pase como parámetro
    public void DestroyAfter(float seconds)
    {
        Destroy(gameObject, seconds);
    }


}
