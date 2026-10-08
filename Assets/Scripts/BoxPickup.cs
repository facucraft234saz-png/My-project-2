using UnityEngine;
using TMPro;

public class BoxPickup : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Punto o posición donde se ubicará la caja al ser recogida")]
    public Transform carryPoint;

    [Tooltip("Texto flotante de interfaz para indicar qué tecla presionar")]
    public TextMeshPro interactionText;

    [Header("Animación Básica")]
    public float bobHeight = 0.15f;
    public float bobSpeed = 3f;

    // Estados internos de la caja
    private bool playerNearby = false;
    private bool isPickedUp = false;
    private GameObject player;
    private Vector3 carryOffset = new Vector3(0f, 1.2f, 0f);

    void Start()
    {
        // Ocultar el texto al iniciar
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Recoger con la tecla E si el jugador está cerca
        if (playerNearby && !isPickedUp && Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }
        // Soltar manualmente con la tecla Q
        else if (isPickedUp && Input.GetKeyDown(KeyCode.Q))
        {
            Drop(true);
        }

        // Animación de flotación mientras la carga
        if (isPickedUp)
        {
            AnimateCarriedBox();
        }
    }

    // Función para agarrar la caja
    public void PickUp()
    {
        if (carryPoint == null)
        {
            Debug.LogWarning("Falta asignar el CarryPoint en el inspector.");
            return;
        }

        isPickedUp = true;
        player = carryPoint.root.gameObject;

        // Desactivar física para que no choque con el jugador al llevarla
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        // Emparentar al jugador
        transform.SetParent(carryPoint);
        transform.localPosition = carryOffset;
        transform.localRotation = Quaternion.identity;

        // Cambiar texto
        if (interactionText != null)
        {
            interactionText.text = "Presiona Q para soltar";
            interactionText.gameObject.SetActive(true);
        }
    }

    // Animación básica usando el tiempo global
    void AnimateCarriedBox()
    {
        float bob = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.localPosition = carryOffset + new Vector3(0f, bob, 0f);
        transform.localRotation = Quaternion.identity;
    }

    // Función para soltar la caja. Si setPositionInFront es true, la deja frente al jugador
    public void Drop(bool setPositionInFront = false)
    {
        isPickedUp = false;

        // Desvincular de la jerarquía
        transform.SetParent(null);

        // Reactivar físicas de la caja
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
        }

        // Posicionar al frente del jugador únicamente si es un soltado manual
        if (setPositionInFront && player != null)
        {
            transform.position = player.transform.position + player.transform.forward * 1.5f + Vector3.up * 0.5f;
        }

        // Actualizar texto informativo
        if (interactionText != null)
        {
            interactionText.text = "Presiona E para agarrar";
            interactionText.gameObject.SetActive(playerNearby);
        }
    }

    // Devuelve si la caja está actualmente agarrada
    public bool IsPickedUp()
    {
        return isPickedUp;
    }

    // Cambia el estado de proximidad con el jugador
    public void SetPlayerNearby(bool nearby)
    {
        playerNearby = nearby;

        if (interactionText == null) return;

        if (isPickedUp)
        {
            interactionText.text = "Presiona Q para soltar";
            interactionText.gameObject.SetActive(true);
        }
        else if (playerNearby)
        {
            interactionText.text = "Presiona E para agarrar";
            interactionText.gameObject.SetActive(true);
        }
        else
        {
            interactionText.gameObject.SetActive(false);
        }
    }
}