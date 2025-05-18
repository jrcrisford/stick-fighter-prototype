using UnityEditor.Rendering;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private MeleeWeapon leftWeapon;
    [SerializeField] private MeleeWeapon rightWeapon;

    [Header("Hand Transforms")]
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;

    private Animator animator;
    private bool isPlayer = false;
    private bool equipToLeftNext = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (gameObject.CompareTag("Player"))
        {
            isPlayer = true;
        }

        Transform[] allChildren = GetComponentsInChildren<Transform>(true);
        foreach (Transform t in allChildren)
        {
            if (t.name == "hand.L_end") leftHand = t;
            if (t.name == "hand.R_end") rightHand = t;
        }

        if (leftHand == null || rightHand == null) Debug.LogWarning("WeaponHandler: Could not find hand transforms by name.");
    }

    private void Update()
    {
        if (!isPlayer) return;

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
        MeleeWeapon weapon = attackSide == 0 ? leftWeapon : rightWeapon;

        if (weapon == null)
        {
            Debug.LogWarning($"{name} has no weapon equipped in {(attackSide == 0 ? "left" : "right")} hand.");
            return;
        }

        if (animator != null)
        {
            animator.SetTrigger(attackSide == 0 ? "LeftAttack" : "RightAttack");
        }

        weapon.TryAttack();
    }

    public void PickupWeapon(MeleeWeapon weapon)
    {
        if (equipToLeftNext)
        {
            if (leftWeapon != null) Destroy(leftWeapon.gameObject);
            EquipWeapon(weapon, leftHand, true);
        }
        else
        {
            if (rightWeapon != null) Destroy(rightWeapon.gameObject);
            EquipWeapon(weapon, rightHand, false);
        }

        equipToLeftNext = !equipToLeftNext;
    }

    private void EquipWeapon(MeleeWeapon weapon, Transform hand, bool isLeft)
    {
        weapon.transform.SetParent(hand);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        var rb = weapon.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;

        var col = weapon.GetComponent<Collider>();
        if (col) col.enabled = false;

        if (isLeft) leftWeapon = weapon;
        else rightWeapon = weapon;
    }
}
