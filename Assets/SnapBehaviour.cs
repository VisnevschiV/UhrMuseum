using UnityEngine;

public class SnapBehaviour : MonoBehaviour
{
    [Tooltip("Tag to identify Gear objects.")]
    public string gearTag = "Gear"; // Set this in the Inspector

    [Tooltip("If true, the object will rotate randomly.")]
    public bool rotate = false;

    [Tooltip("The transform to snap to when the object is dropped.")]
    public Transform snapDestination;

    [Tooltip("The object to rotate randomly.")]
    public Transform gear; // Assign the child or specific object to rotate
    void Start()
    {
        // Initialization logic if needed
    }

    void Update()
    {
        // Rotate the specified object randomly if the rotate flag is true
        if ( gear != null)
        {
            gear.position = transform.position;
            if (rotate)
            {
                gear.Rotate(Vector3.up, 20 * Time.deltaTime); // Rotate around the Y-axis
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has the specified gear tag
        if (other.CompareTag(gearTag))
        {
            Debug.Log("Gear object entered the trigger zone.");
            // Snap the position to the center of the snap target
            if (snapDestination != null)
            {
                gear = other.transform; // Assign the gear object to the one that entered the trigger
                gear.position = snapDestination.position;
                gear.rotation = snapDestination.rotation;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the colliding object has the specified gear tag
        if (other.CompareTag(gearTag))
        {
            if(other.transform == gear)
            {
                Debug.Log("Gear object exited the trigger zone.");
                // Optionally, you can reset the gear position or perform other actions here
                gear = null; // Clear the reference to the gear object
            }
        }
    }
}
