using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class CameraFollow : MonoBehaviour
{
    public float zoomSpeed = 2.5f;
    private GameObject golfBall;
    private Vector3 cameraOffset = new Vector3(0, 0.3f, -1f);
    // Start is called before the first frame update
    void Start()
    {
        string name = transform.name;
        golfBall = GameObject.Find("Golf Ball " + ExtractNumbers(name));
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the camera position relative to the ball's position and rotation
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float zoomAmount = scroll * zoomSpeed;
        if (cameraOffset.y - zoomAmount > 0.0001 && cameraOffset.y - zoomAmount < 3.6)
        {
            cameraOffset.z += zoomAmount;
            cameraOffset.y -= zoomAmount/2;
        }
        Vector3 desiredCameraPosition = golfBall.transform.position + golfBall.transform.TransformDirection(cameraOffset);
        transform.position = Vector3.Lerp(transform.position, desiredCameraPosition, Time.deltaTime * 20);
        transform.LookAt(golfBall.transform.position);
    }

    public static string ExtractNumbers(string input)
    {
        // Define a regular expression pattern that matches digits
        string pattern = @"\d+";

        // Use Regex.Match to find all matches of the pattern in the input string
        MatchCollection matches = Regex.Matches(input, pattern);

        // Concatenate the matched numbers into a single string
        string result = "";
        foreach (Match match in matches)
        {
            result += match.Value;
        }

        return result;
    }
}
