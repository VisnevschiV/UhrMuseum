using System.Collections.Generic;
using UnityEngine;

public class GearBehaviour : MonoBehaviour
{
    public float rotationSpeed = 100f; // Speed of rotation
    public bool isMoving = false; // Whether this gear is currently moving
    public List<GearBehaviour> connectedGears = new List<GearBehaviour>(); // List of connected gears

    private void Start()
    {
        // Optionally, initialize the gear as moving
        if (isMoving)
        {
            RotateGear();
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            RotateGear();
        }
    }

    private void RotateGear()
    {
        // Check for conflicting rotation speeds
        foreach (GearBehaviour gear in connectedGears)
        {
            if (gear.isMoving && Mathf.Sign(gear.rotationSpeed) == Mathf.Sign(rotationSpeed))
            {
                Debug.LogWarning("Conflicting rotation detected! Stopping all gears.");
                StopAllGears();
                return;
            }
        }

        // Rotate this gear around its Z-axis
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // Trigger connected gears to move if they are not already moving
        foreach (GearBehaviour gear in connectedGears)
        {
            if (!gear.isMoving)
            {
                gear.StartMoving(-rotationSpeed); // Reverse rotation for connected gears
            }
        }
    }

    public void StartMoving(float speed)
    {
        if (!isMoving)
        {
            isMoving = true;
            rotationSpeed = speed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detect if another gear is in contact
        GearBehaviour otherGear = other.GetComponent<GearBehaviour>();
        if (otherGear != null && !connectedGears.Contains(otherGear))
        {
            connectedGears.Add(otherGear);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Remove the gear from the connected list when it is no longer in contact
        GearBehaviour otherGear = other.GetComponent<GearBehaviour>();
        if (otherGear != null && connectedGears.Contains(otherGear))
        {
            connectedGears.Remove(otherGear);
        }
        isMoving = false; // Stop this gear when it exits contact with another gear
    }

    private void StopAllGears()
    {
        // Stop this gear and all connected gears
        isMoving = false;
        foreach (GearBehaviour gear in connectedGears)
        {
            gear.isMoving = false;
        }
    }
}
