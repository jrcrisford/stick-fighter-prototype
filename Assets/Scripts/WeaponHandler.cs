//using UnityEditor.Rendering;
using TMPro;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private MeleeWeapon leftWeapon;
    [SerializeField] private MeleeWeapon rightWeapon;

    [Header("Hand Transforms")]
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;

    [Header("Item UI")]
    [SerializeField] private TextMeshProUGUI leftDam;
    [SerializeField] private TextMeshProUGUI rightDam;
    [SerializeField] private TextMeshProUGUI leftRange;
    [SerializeField] private TextMeshProUGUI rightRange;
    [SerializeField] private TextMeshProUGUI leftKnock;
    [SerializeField] private TextMeshProUGUI rightKnock;
    [SerializeField] private TextMeshProUGUI leftStick;
    [SerializeField] private TextMeshProUGUI rightStick;


    private Animator animator;
    private bool isPlayer = false;
    private bool equipToLeftNext = true;

    void Awake()
    {
        animator = GetComponent<Animator>();

        leftDam.text = string.Empty;
        rightDam.text = string.Empty;
        leftRange.text = string.Empty;
        rightRange.text = string.Empty;
        leftKnock.text = string.Empty;
        rightKnock.text = string.Empty;
        leftStick.text = string.Empty;
        rightStick.text = string.Empty;

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
        if (isPlayer)
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
            leftStick.text = weapon.name;
            leftDam.text = weapon.damage.ToString();
            leftRange.text = weapon.attackRange.ToString();
            leftKnock.text = weapon.knockbackForce.ToString();
            EquipWeapon(weapon, leftHand, true);
        }
        else
        {
            if (rightWeapon != null) Destroy(rightWeapon.gameObject);
            rightStick.text = weapon.name;
            rightDam.text = weapon.damage.ToString();
            rightRange.text = weapon.attackRange.ToString();
            rightKnock.text = weapon.knockbackForce.ToString();
            EquipWeapon(weapon, rightHand, false);
        }

        equipToLeftNext = !equipToLeftNext;
    }

    private void EquipWeapon(MeleeWeapon weapon, Transform hand, bool isLeft)
    {
        Transform grip = weapon.transform.Find("GripPoint");
        if (grip != null)
        {
            weapon.transform.SetParent(hand);

            // Position the weapon so its GripPoint aligns with the hand
            weapon.transform.position = hand.position;
            //weapon.transform.rotation = hand.rotation * Quaternion.Euler(90, -55, 85);
            weapon.transform.rotation = hand.rotation * grip.rotation;

            // Offset the weapon to match the grip's relative transform
            Vector3 gripOffset = weapon.transform.position - grip.position;
            weapon.transform.position += gripOffset;
        }
        else
        {
            Debug.LogWarning($"Weapon '{weapon.name}' has no GripPoint transform.");
            weapon.transform.SetParent(hand);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
        }

        var rb = weapon.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;

        var col = weapon.GetComponent<Collider>();
        if (col) col.enabled = false;

        if (isLeft) leftWeapon = weapon;
        else rightWeapon = weapon;
    }
}
