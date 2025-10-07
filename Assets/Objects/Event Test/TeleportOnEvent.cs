using UnityEngine;

public class TeleportOnEvent : MonoBehaviour
{
    private void OnEnable()
    {
        // Add the teleport function to the event when the object is enabled
        EventManager.OnAttack += Teleport;
    }

    private void OnDisable()
    {
        // Remove the teleport function from the event when the object is disabled
        // (The purpose of this is to prevent issues such as memory leaks)
        // Whenever you subscribe a method to an event, you should ALWAYS have a corrosponding unsubscribe
        EventManager.OnAttack -= Teleport;
    }

    void Teleport()
    {
        Vector3 newPosition = transform.position;
        newPosition.x += Random.Range(-2.0f, 2.0f);
        newPosition.z += Random.Range(-2.0f, 2.0f);
        transform.position = newPosition;
    }
}
