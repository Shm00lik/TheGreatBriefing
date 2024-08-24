using UnityEngine;

public class WallPositioner : MonoBehaviour
{
    public Camera mainCamera;
    public Side side; // Set this in the Inspector for each wall

    void Update()
    {
        // Get the position of the wall
        Vector3 wallPosition = transform.position;

        switch (side)
        {
            case Side.Right:
                wallPosition.x = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
                break;
            case Side.Left:
                wallPosition.x = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
                break;
        }

        // Apply the new position
        transform.position = wallPosition;
    }
}

public enum Side
{
    Right,
    Left
}
