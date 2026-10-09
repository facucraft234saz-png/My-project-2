using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 5f;

    void Start()
    {
        // Se destruye solo al pasar el tiempo asignado
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Movimiento constante hacia adelante en tiempo real
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}