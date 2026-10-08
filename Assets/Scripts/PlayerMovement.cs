using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Parámetros de Movimiento")]
    [Tooltip("Velocidad de desplazamiento del jugador")]
    public float speed = 5f;
    [Tooltip("Fuerza de impulso del salto")]
    public float jumpForce = 7f;
    [Tooltip("Velocidad de rotación del personaje hacia la dirección de movimiento")]
    public float rotationSpeed = 10f;

    // Componentes y referencias del sistema
    private Rigidbody rb;
    private Transform mainCamera;

    // Vectores para calcular la dirección
    private Vector3 movementInput;
    private Vector3 moveDirection;

    // Estado del salto
    private bool isGrounded = false;

    // Variables para sincronizar el movimiento con plataformas móviles
    private Transform currentPlatform;
    private Vector3 lastPlatformPosition;

    void Start()
    {
        // Obtener el Rigidbody adosado a este GameObject
        rb = GetComponent<Rigidbody>();

        // Guardar la referencia de la cámara principal para calcular direcciones
        if (Camera.main != null)
        {
            mainCamera = Camera.main.transform;
        }
    }

    void Update()
    {
        // 1. CAPTURA DE ENTRADA TECLADO (Únicamente teclas WASD)
        float x = 0f;
        float z = 0f;

        if (Input.GetKey(KeyCode.A)) x -= 1f; // Izquierda
        if (Input.GetKey(KeyCode.D)) x += 1f; // Derecha
        if (Input.GetKey(KeyCode.W)) z += 1f; // Adelante
        if (Input.GetKey(KeyCode.S)) z -= 1f; // Atrás

        // Normalizar para que el movimiento diagonal no sea más rápido
        movementInput = new Vector3(x, 0f, z).normalized;

        // 2. CONTROL DEL SALTO (Estricto: un solo salto si está tocando el suelo)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // Aplicar movimiento y rotación en el ciclo de física
        MoveAndRotate();
    }

    void MoveAndRotate()
    {
        // Transformar la entrada WASD según la orientación de la cámara
        if (movementInput.sqrMagnitude > 0.01f && mainCamera != null)
        {
            Vector3 camForward = mainCamera.forward;
            Vector3 camRight = mainCamera.right;

            // Anular la inclinación vertical de la cámara (mantener plano horizontal)
            camForward.y = 0f;
            camRight.y = 0f;

            camForward.Normalize();
            camRight.Normalize();

            // Calcular el vector de movimiento proyectado en el espacio de la cámara
            moveDirection = (camForward * movementInput.z) + (camRight * movementInput.x);
        }
        else
        {
            moveDirection = Vector3.zero;
        }

        // Calcular desplazamiento del jugador usando Time.fixedDeltaTime
        Vector3 playerDelta = moveDirection * speed * Time.fixedDeltaTime;

        // Calcular desplazamiento acumulado si el jugador está parado sobre una plataforma móvil
        Vector3 platformDelta = Vector3.zero;
        if (currentPlatform != null)
        {
            platformDelta = currentPlatform.position - lastPlatformPosition;
            lastPlatformPosition = currentPlatform.position;
        }

        // Mover el Rigidbody aplicando la suma del movimiento propio y el de la plataforma
        rb.MovePosition(rb.position + playerDelta + platformDelta);

        // Rotar suavemente el personaje mirando hacia donde se desplaza
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    void Jump()
    {
        // Apagar el estado de suelo inmediatamente para prevenir saltos dobles
        isGrounded = false;
        currentPlatform = null;

        // Limpiar la velocidad vertical previa para que la fuerza del salto sea siempre uniforme
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // Aplicar el impulso hacia arriba
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void OnCollisionStay(Collision collision)
    {
        // Recorrer los puntos de contacto de la colisión
        foreach (ContactPoint contact in collision.contacts)
        {
            // Comprobar si el impacto fue desde abajo (superficie horizontal sobre la que estamos parados)
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;

                // Comprobar si el objeto colisionado es una plataforma móvil
                if (collision.gameObject.TryGetComponent(out MovingPlatform platform))
                {
                    if (currentPlatform != collision.transform)
                    {
                        currentPlatform = collision.transform;
                        lastPlatformPosition = currentPlatform.position;
                    }
                }
                break;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Al dejar de tocar CUALQUIER superficie, deshabilitar el estado de suelo
        isGrounded = false;

        // Si dejamos de tocar la plataforma móvil en la que estábamos, desvincularla
        if (currentPlatform == collision.transform)
        {
            currentPlatform = null;
        }
    }
}