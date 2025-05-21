using UnityEngine;

[RequireComponent(typeof(MeleeWeapon))]
[RequireComponent(typeof(Light))]
public class WeaponPickupTrigger : MonoBehaviour
{
    private bool isPickedUp = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var handler = other.GetComponent<WeaponHandler>();
        if (handler != null && !isPickedUp)
        {
            isPickedUp = true;
            handler.PickupWeapon(GetComponent<MeleeWeapon>());
        }
    }
}
