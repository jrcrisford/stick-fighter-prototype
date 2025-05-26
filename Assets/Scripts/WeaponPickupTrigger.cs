using UnityEngine;

[RequireComponent(typeof(MeleeWeapon))]
public class WeaponPickupTrigger : MonoBehaviour
{
    private bool playerInRange = false;
    private WeaponHandler playerHandler;
    private bool isPickedUp = false;

    [SerializeField] private GameObject pickupUIPrompt;

    private void Start()
    {
        pickupUIPrompt = GetComponentInChildren<Canvas>(true)?.gameObject;

        if (pickupUIPrompt != null)
            pickupUIPrompt.SetActive(false);
        else
            Debug.LogWarning("WeaponPickupTrigger: Pickup UI prompt not found as child!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || isPickedUp) return;

        playerHandler = other.GetComponent<WeaponHandler>();
        if (playerHandler != null)
        {
            playerInRange = true;
            if (pickupUIPrompt != null)
                pickupUIPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (pickupUIPrompt != null)
            pickupUIPrompt.SetActive(false);

        playerInRange = false;
        playerHandler = null;
    }

    private void Update()
    {
        if (playerInRange && !isPickedUp && Input.GetKeyDown(KeyCode.E))
        {
            playerHandler.PickupWeapon(GetComponent<MeleeWeapon>());
            isPickedUp = true;

            if (pickupUIPrompt != null)
                pickupUIPrompt.SetActive(false);
        }
    }
}
