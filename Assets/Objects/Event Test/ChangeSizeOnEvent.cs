using UnityEngine;

public class ChangeSizeOnEvent : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.OnCrouch += ChangeSize;
    }

    private void OnDisable()
    {
        EventManager.OnCrouch -= ChangeSize;
    }

    void ChangeSize()
    {
        Vector3 newSize = transform.localScale;
        newSize.x += Random.Range(-1.0f, 1.0f);
        newSize.z += Random.Range(-1.0f, 1.0f);
        transform.localScale = newSize;
    }
}

