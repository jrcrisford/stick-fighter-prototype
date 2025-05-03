using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private MeleeWeapon equippedWeapon;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AttemptAttack();
        }
    }

    public void AttemptAttack()
    {
        if (equippedWeapon == null)
        {
            Debug.LogWarning($"{name} has no weapon equipped.");
            return;
        }
        
        equippedWeapon.TryAttack();
    }
}
