using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Configuración de Ejes (Interruptores)")]
    [Tooltip("Activa el movimiento en el eje X")]
    public bool moveX = true;
    [Tooltip("Activa el movimiento en el eje Y")]
    public bool moveY = false;
    [Tooltip("Activa el movimiento en el eje Z")]
    public bool moveZ = false;

    [Header("Parámetros de Movimiento")]
    [Tooltip("Distancia total que se desplazará la plataforma desde el punto de origen")]
    public float distance = 6f;
    [Tooltip("Velocidad de desplazamiento")]
    public float speed = 2f;
    [Tooltip("Tiempo de espera en segundos al llegar a cada punto extremo")]
    public float waitTimeAtEnds = 0.5f;
    [Tooltip("Tiempo inicial de retraso antes de comenzar a moverse")]
    public float startDelay = 1f;

    [Header("Espacio de Movimiento")]
    [Tooltip("Si está activo, se moverá según la orientación local de la plataforma (rotación)")]
    public bool useLocalSpace = false;

    private Vector3 pointA;
    private Vector3 pointB;
    private Vector3 targetPoint;
    private bool movingToB = true;
    private bool movementEnabled = false;
    private bool isWaiting = false;

    void Start()
    {
        // Guardar la posición inicial como Punto A
        pointA = transform.position;

        // Calcular el vector de dirección según los interruptores activados
        Vector3 direction = new Vector3(
            moveX ? 1f : 0f,
            moveY ? 1f : 0f,
            moveZ ? 1f : 0f
        );

        // Normalizar la dirección si hay múltiples ejes activos para no multiplicar la distancia en diagonales
        if (direction.sqrMagnitude > 0)
        {
            direction.Normalize();
        }

        // Convertir la dirección a espacio local si la opción está activa
        if (useLocalSpace)
        {
            direction = transform.TransformDirection(direction);
        }

        // Calcular la posición final Punto B
        pointB = pointA + (direction * distance);

        targetPoint = pointB;

        // Iniciar retraso de movimiento
        Invoke(nameof(EnableMovement), startDelay);
    }

    void Update()
    {
        if (!movementEnabled || isWaiting) return;

        // Mover la plataforma hacia el objetivo actual
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPoint,
            speed * Time.deltaTime
        );

        // Comprobar si llegó al destino
        if (Vector3.Distance(transform.position, targetPoint) < 0.01f)
        {
            StartCoroutine(WaitAndChangeDirection());
        }
    }

    private IEnumerator WaitAndChangeDirection()
    {
        isWaiting = true;

        if (waitTimeAtEnds > 0f)
        {
            yield return new WaitForSeconds(waitTimeAtEnds);
        }

        // Alternar el punto de destino
        movingToB = !movingToB;
        targetPoint = movingToB ? pointB : pointA;

        isWaiting = false;
    }

    private void EnableMovement()
    {
        movementEnabled = true;
    }

    // Dibuja los puntos A y B en la vista de escena para facilitar el diseño de niveles
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            Vector3 start = transform.position;
            Vector3 dir = new Vector3(
                moveX ? 1f : 0f,
                moveY ? 1f : 0f,
                moveZ ? 1f : 0f
            );

            if (dir.sqrMagnitude > 0) dir.Normalize();
            if (useLocalSpace) dir = transform.TransformDirection(dir);

            Vector3 end = start + (dir * distance);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(start, 0.3f);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(end, 0.3f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(start, end);
        }
    }
}