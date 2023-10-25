using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float zoomSpeed = 1f; // Adjust this value to control zoom speed
    public float maxZoom = 1f; // Adjust this value to control maximum zoom in
    public float minZoom = 0.151f; // Adjust this value to control maximum zoom out
    public float yOffset = 0.2f; // Adjust this value to control the amount of change in y
    public float zOffset = -0.6f; // Adjust this value to control the amount of change in z

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 currentPosition = transform.localPosition;

        if (scroll != 0.0f)
        {
            // Adjust the direction vector using yOffset and zOffset
            Vector3 direction = new Vector3(0, scroll * zoomSpeed * yOffset, scroll * zoomSpeed * zOffset);
            Vector3 newPosition = currentPosition + direction;

            // Limiting the zoom range
            if (newPosition.y < maxZoom && newPosition.y > minZoom)
            {
                transform.localPosition = newPosition;
            }
        }
    }
}
