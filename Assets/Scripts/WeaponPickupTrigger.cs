using UnityEngine;

[RequireComponent(typeof(MeleeWeapon))]
[RequireComponent(typeof(Light))]
public class WeaponPickupTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var handler = other.GetComponent<WeaponHandler>();
        if (handler != null)
        {
            handler.PickupWeapon(GetComponent<MeleeWeapon>());
        }
    }
}
