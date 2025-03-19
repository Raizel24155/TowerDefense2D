using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;

    private Vector3 lastPosition;

    public Vector2 GetMousePositionVector2()
    {
        Vector3 mousePosition = sceneCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        lastPosition = mousePosition;
        return lastPosition;
    }
}
