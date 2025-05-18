using UnityEngine;

[RequireComponent(typeof(MeleeWeapon))]
[RequireComponent(typeof(Light))]
public class WeaponPickupTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var handler = other.GetComponent<WeaponHandler>();
        var light = GetComponent<Light>();
        if (handler != null)
        {
            light.enabled = false;
            handler.PickupWeapon(GetComponent<MeleeWeapon>());
        }
    }
}
