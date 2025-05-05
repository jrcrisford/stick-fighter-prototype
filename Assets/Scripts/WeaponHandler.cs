using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private MeleeWeapon equippedWeapon;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AttemptAttack(0);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            AttemptAttack(1);
        }
    }

    public void AttemptAttack(int attackSide)
    {
        if (equippedWeapon == null)
        {
            Debug.LogWarning($"{name} has no weapon equipped.");
            return;
        }

        if (animator != null)
        {
            if (attackSide == 0)
                animator.SetTrigger("LeftAttack");
            else
                animator.SetTrigger("RightAttack");
        }

        equippedWeapon.TryAttack();
    }
}
