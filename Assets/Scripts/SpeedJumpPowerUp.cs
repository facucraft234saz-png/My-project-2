using UnityEngine;
using System.Collections;

public class SpeedJumpPowerUp : MonoBehaviour
{
    [Header("Ajustes del Power-Up")]
    public float speedMultiplier = 3f;  // Aumento del 200% (3 veces el valor inicial)
    public float jumpMultiplier = 3f;   // Aumento del 200%
    public float duration = 10f;        // Duración del efecto en el jugador
    public float respawnTime = 20f;     // Tiempo para reaparecer la esfera

    private MeshRenderer meshRenderer;
    private Collider itemCollider;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        itemCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out PlayerMovement player))
            {
                // Aplicar el efecto al jugador
                player.ApplyPowerUp(speedMultiplier, jumpMultiplier, duration);

                // Ocultar y desactivar la esfera en la escena
                StartCoroutine(RespawnRoutine());
            }
        }
    }

    private IEnumerator RespawnRoutine()
    {
        // Ocultar la esfera y desactivar colisiones mientras reaparece
        meshRenderer.enabled = false;
        itemCollider.enabled = false;

        // Esperar 20 segundos
        yield return new WaitForSeconds(respawnTime);

        // Volver a hacer visible y colectable la esfera
        meshRenderer.enabled = true;
        itemCollider.enabled = true;
    }
}