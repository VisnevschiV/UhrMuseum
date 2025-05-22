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

    private bool isSnapping = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(gearTag))
        {
            Debug.Log("Gear object entered the trigger zone.");
            if (snapDestination != null)
            {
                gear = other.transform;

                Rigidbody rb = gear.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.isKinematic = true;

                isSnapping = true;

                // Deactivate this object's mesh
                MeshRenderer mesh = GetComponent<MeshRenderer>();
                if (mesh != null)
                    mesh.enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(gearTag))
        {
            if (gear != null && other.transform == gear)
            {
                Debug.Log("Gear object exited the trigger zone.");

                Rigidbody rb = gear.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.isKinematic = false;

                isSnapping = false;
                gear = null;

                // Reactivate this object's mesh
                MeshRenderer mesh = GetComponent<MeshRenderer>();
                if (mesh != null)
                    mesh.enabled = true;
            }
        }
    }

    private void Update()
    {
        if (isSnapping && gear != null)
        {
            // Match position and scale
            gear.position = transform.position;
            gear.localScale = transform.lossyScale;

            // Combine parent's rotation (X, Y) with gear's own local Z rotation
            Vector3 parentEuler = transform.rotation.eulerAngles;
            Vector3 gearEuler = gear.rotation.eulerAngles;
            float gearZ = gearEuler.z;

            // Set rotation: parent's X and Y, gear's Z
            gear.rotation = Quaternion.Euler(parentEuler.x, parentEuler.y, gearZ);
        }
    }
}
