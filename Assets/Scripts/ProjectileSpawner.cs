using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float spawnTime = 2f;

    void Start()
    {
        // Llama a SpawnProjectile repetidamente cada 'spawnTime' segundos
        InvokeRepeating(nameof(SpawnProjectile), 0f, spawnTime);
    }

    void SpawnProjectile()
    {
        if (projectilePrefab != null)
        {
            Instantiate(projectilePrefab, transform.position, transform.rotation);
            Debug.Log("Proyectil generado");
        }
    }
}