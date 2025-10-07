using UnityEngine;
using UnityEngine.InputSystem;

public class EventManager : MonoBehaviour
{
    // Defining events
    public delegate void AttackPressed();
    public static event AttackPressed OnAttack;
    public delegate void CrouchPressed();
    public static event CrouchPressed OnCrouch;

    private InputAction attack;
    private InputAction crouch;

    void Start()
    {
        // Trigger Interact() when the player presses E
        attack = InputSystem.actions.FindAction("Attack");
        attack.performed += Attack;

        crouch = InputSystem.actions.FindAction("Crouch");
        crouch.performed += Crouch;
    }

    void Attack(InputAction.CallbackContext context)
    {
        // Call the event
        OnAttack();
    }

    void Crouch(InputAction.CallbackContext context)
    {
        OnCrouch();
    }
}
