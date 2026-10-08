using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Plataforma Jugador")]
    [Tooltip("Transform de la plataforma a la que volverá el jugador")]
    public Transform initialPlatform;

    [Header("Plataforma Caja")]
    [Tooltip("Transform de la plataforma a la que volverá la caja si colisiona con el obstáculo")]
    public Transform boxRespawnPlatform;

    [Header("Ajustes generales")]
    [Tooltip("Altura adicional sobre la plataforma para evitar aparecer enterrado")]
    public float heightOffset = 1.5f;

    // Detectar colisión con el Jugador o con la Caja
    void OnCollisionEnter(Collision collision)
    {
        // CASO 1: Choca el Jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            // Si el jugador llevaba la caja agarrada, la suelta
            BoxPickup box = FindObjectOfType<BoxPickup>();
            if (box != null && box.IsPickedUp())
            {
                box.Drop();
            }

            // Teletransportar al jugador a su plataforma inicial
            TeleportObject(collision.gameObject, initialPlatform);
        }
        // CASO 2: Choca la Caja
        else if (collision.gameObject.TryGetComponent(out BoxPickup boxPickup))
        {
            // Si la caja estaba siendo transportada, forzar soltado antes del teletransporte
            if (boxPickup.IsPickedUp())
            {
                boxPickup.Drop();
            }

            // Teletransportar la caja a su plataforma asignada
            TeleportObject(boxPickup.gameObject, boxRespawnPlatform);
        }
    }

    // Método genérico para reubicar un objeto (Jugador o Caja) en su respectiva plataforma
    void TeleportObject(GameObject target, Transform destinationPlatform)
    {
        if (destinationPlatform == null)
        {
            Debug.LogWarning("Falta asignar la plataforma de respawn en el inspector del obstáculo.");
            return;
        }

        // Sacar de la jerarquía de cualquier padre por seguridad
        target.transform.SetParent(null);

        // Mover a la posición de destino más el offset de altura
        target.transform.position = destinationPlatform.position + Vector3.up * heightOffset;

        // Resetear la velocidad del Rigidbody para evitar inercias
        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log(target.name + " colisionó con un obstáculo y fue teletransportado.");
    }
}