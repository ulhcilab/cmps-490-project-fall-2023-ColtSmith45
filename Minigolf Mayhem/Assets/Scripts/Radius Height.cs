using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusHeight : MonoBehaviour
{
    void Start()
    {
        // Get the mesh collider component from the parent
        MeshCollider meshCollider = GetComponentInParent<MeshCollider>();

        if (meshCollider != null && meshCollider.sharedMesh != null)
        {
            // Get the mesh bounds
            Bounds bounds = meshCollider.sharedMesh.bounds;

            // Calculate radius and height based on the bounding box
            float radius = bounds.extents.magnitude/125;
            float height = bounds.size.y/125;

            // Print or use the values
            Debug.Log("Radius: " + radius);
            Debug.Log("Height: " + height);
        }
        else
        {
            Debug.LogError("The parent object does not have a MeshCollider component or a valid mesh.");
        }
    }
}

