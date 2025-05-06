using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    
    public Vector2 MoveInput { get; private set; }                                          // Publicly accessible movement and aim input properties        
    public Vector2 AimInput { get; private set; }                                           // <<
    public bool JumpInput { get; private set; }                                           // <<

    private PlayerControls controls;                                                        

    private void Awake()
    {
        controls = new PlayerControls();                                                    // Initalise the PlayerControls class containing binds            

        controls.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();      // Bind move action to update MoveInput
        controls.Player.Move.canceled += ctx => MoveInput = Vector2.zero;                   // <<

        controls.Player.Aim.performed += ctx => AimInput = ctx.ReadValue<Vector2>();        // Bind aim action to update AimInput
        controls.Player.Aim.canceled += ctx => AimInput = Vector2.zero;                     // <<

        controls.Player.Jump.performed += ctx => JumpInput = true;                          // Bind jump action to update JumpPressed
        controls.Player.Jump.canceled += ctx => JumpInput = false;                          // <<
    }

    // Enable input system when script becomes active
    private void OnEnable()
    {
        controls.Enable();
    }

    // Disable input system to avoid unwanted input
    private void OnDisable()
    {
        controls.Disable();
    }
}
