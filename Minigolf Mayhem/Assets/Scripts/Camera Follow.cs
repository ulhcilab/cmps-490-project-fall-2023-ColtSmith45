using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float zoomSpeed = 2.5f;
    private GameObject golfBall;
    private Vector3 cameraOffset = new Vector3(0, 0.3f, -1f);
    // Start is called before the first frame update
    void Start()
    {
        golfBall = GameObject.Find("Golf Ball");
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the camera position relative to the ball's position and rotation
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float zoomAmount = scroll * zoomSpeed;
        if (cameraOffset.z + zoomAmount < -0.5) cameraOffset.z += zoomAmount;
        Vector3 desiredCameraPosition = golfBall.transform.position + golfBall.transform.TransformDirection(cameraOffset);
        transform.position = Vector3.Lerp(transform.position, desiredCameraPosition, Time.deltaTime * 20);
        transform.LookAt(golfBall.transform.position);
    }
}
