using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 12f;
    public float gravity = -9.81f;

    public CharacterController controller;
    public Transform ground_check;
    public float ground_distance = 0.4f;
    public LayerMask ground_mask;

    private InputAction move_action, jump_action;

    private Vector3 velocity;
    bool grounded;

    void Start()
    {
        move_action = InputSystem.actions.FindAction("Move");
        jump_action = InputSystem.actions.FindAction("Jump");
    }

    void Update()
    {
        grounded = Physics.CheckSphere(ground_check.position, ground_distance, ground_mask);
        print(grounded);

        if (grounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float move_x = move_action.ReadValue<Vector2>().x;
        float move_z = move_action.ReadValue<Vector2>().y;

        Vector3 move = transform.right * move_x + transform.forward * move_z;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}