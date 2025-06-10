using System.Collections.Generic;
using UnityEngine;

public class GearBehaviour : MonoBehaviour
{
    public int teethCount;
    public float rotationSpeed = 10f; // Speed of rotation
    public bool isMoving = false; // Whether this gear is currently moving
    public bool isEngine = false; // Whether this gear is an engine (primary gear)
    public List<GearBehaviour> connectedGears = new List<GearBehaviour>(); // List of connected gears

    [Tooltip("The transform to rotate instead of this object. If null, rotates this.transform.")]
    public Transform targetToRotate;
    [SerializeField]
    private SecondaryGearBehaviour secondaryGearBehaviour;

    private void Update()
    {
        if (isMoving)
        {
            RotateGear();
        }
    }

    private void RotateGear()
    {
        // Rotate the target transform (or this if not set) around its Z-axis
        Transform rotateTarget = targetToRotate != null ? targetToRotate : transform;
        rotateTarget.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // Trigger connected gears to move if they are not already moving
        foreach (GearBehaviour gear in connectedGears)
        {

            gear.StartMoving(-rotationSpeed * teethCount); // Reverse rotation for connected gears
        }
    }

    public virtual void StartMoving(float speed, bool forceSpeed = false)
    {
        if (!isMoving)
        {
            isMoving = true;
            if (forceSpeed)
            {
                rotationSpeed = speed;
            }
            else
            {
                rotationSpeed = speed / teethCount;
                secondaryGearBehaviour?.StartMoving(rotationSpeed, true); // Start moving the secondary gear
            }
        }
        
    }

    public void OnTriggerEnter(Collider other)
    {
        // Detect if another gear is in contact
        GearBehaviour otherGear = other.GetComponent<GearBehaviour>();
        if (otherGear != null && !connectedGears.Contains(otherGear) && otherGear != secondaryGearBehaviour)
        {
            connectedGears.Add(otherGear);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        // Remove the gear from the connected list when it is no longer in contact
        GearBehaviour otherGear = other.GetComponent<GearBehaviour>();
        if (otherGear != null && connectedGears.Contains(otherGear))
        {
            connectedGears.Remove(otherGear);
        }
        if (!isEngine)
        {
            isMoving = false; // Stop moving if this is not an engine gear
            rotationSpeed = 0f; // Reset rotation speed
        }
    }
}
