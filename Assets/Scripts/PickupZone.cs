using UnityEngine;

public class PickupZone : MonoBehaviour
{
    private BoxPickup boxPickup;


    void Start()
    {
        boxPickup = GetComponentInParent<BoxPickup>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boxPickup.SetPlayerNearby(true);
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boxPickup.SetPlayerNearby(false);
        }
    }
}