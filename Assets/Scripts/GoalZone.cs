using UnityEngine;
using TMPro;

public class GoalZone : MonoBehaviour
{
    [Header("UI de Pantalla (Canvas)")]
    [Tooltip("Texto UI de TextMeshPro (TextMeshProUGUI) en el Canvas del jugador")]
    public TextMeshProUGUI victoryText;

    [Header("Textos Personalizados")]
    public string messageVictory = "VICTORIA";
    public string messageNeedBox = "Necesitas la caja para ganar";

    private void Start()
    {
        // Ocultar el texto de victoria/mensaje al iniciar
        if (victoryText != null)
        {
            victoryText.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto que entra es el Jugador
        if (other.CompareTag("Player"))
        {
            // Buscar la caja en la escena para comprobar su estado
            BoxPickup box = FindObjectOfType<BoxPickup>();

            if (box != null && box.IsPickedUp())
            {
                ShowMessage(messageVictory);
                Debug.Log("¡El jugador ha ganado el juego!");
            }
            else
            {
                ShowMessage(messageNeedBox);
                Debug.Log("El jugador no lleva la caja.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Ocultar el texto al salir de la zona de la meta
        if (other.CompareTag("Player") && victoryText != null)
        {
            victoryText.gameObject.SetActive(false);
        }
    }

    private void ShowMessage(string message)
    {
        if (victoryText != null)
        {
            victoryText.text = message;
            victoryText.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Falta asignar el TextMeshProUGUI en el inspector de GoalZone.");
        }
    }
}