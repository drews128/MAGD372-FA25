using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCamera : MonoBehaviour
{
    public float mouse_sens = 100f;
    private float x_rotation = 0f;

    public Transform player_body;

    private InputAction look_action;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        look_action = InputSystem.actions.FindAction("Look");
    }

    void Update()
    {
        float mouse_x = look_action.ReadValue<Vector2>().x * mouse_sens * Time.deltaTime;
        float mouse_y = look_action.ReadValue<Vector2>().y * mouse_sens * Time.deltaTime;

        x_rotation -= mouse_y;
        x_rotation = Mathf.Clamp(x_rotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(x_rotation, 0f, 0f);
        player_body.Rotate(Vector3.up * mouse_x);
    }
}
