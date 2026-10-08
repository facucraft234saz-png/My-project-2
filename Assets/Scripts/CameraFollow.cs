using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    // Distancia base respecto al jugador
    public Vector3 offset = new Vector3(0f, 6f, -8f);

    public float rotationSpeed = 100f;
    public float smoothSpeed = 10f;

    private float currentYaw = 0f;

    void LateUpdate()
    {
        if (target == null) return;

        // Leer flechas de izquierda/derecha para rotar la cámara
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            currentYaw -= rotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            currentYaw += rotationSpeed * Time.deltaTime;
        }

        // Calcular la rotación actual alrededor del eje Y
        Quaternion rotation = Quaternion.Euler(0f, currentYaw, 0f);

        // Rotar el vector offset según el ángulo actual
        Vector3 rotatedOffset = rotation * offset;

        // Posición deseada respetando la rotación
        Vector3 desiredPosition = target.position + rotatedOffset;

        // Interpolación suave de movimiento
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        // Apuntar la cámara hacia el jugador
        transform.LookAt(target.position + Vector3.up * 1.5f); // Un poco elevado hacia la cabeza/torso
    }
}