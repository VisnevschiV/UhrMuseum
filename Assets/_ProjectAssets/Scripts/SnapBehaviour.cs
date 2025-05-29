using System;
using UnityEngine;

public class SnapBehaviour : MonoBehaviour
{
    [Tooltip("Tag to identify Gear objects.")]
    public Transform objToSnap;

    [Tooltip("The transform to snap to when the object is dropped.")]
    public Transform snapDestination;

    [SerializeField]
    private float forcedRotationSpeed;

    private Transform gear; // Assign the child or specific object to rotate

    private bool isSnapping = false;
    [SerializeField]
    public Action onSnapComplete;



    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == objToSnap)
        {
            Debug.Log("Gear object entered the trigger zone.");
            if (snapDestination != null)
            {
                onSnapComplete?.Invoke();
                gear = other.transform;
                //gear.GetComponent<GearBehaviour>().enabled = false; // Disable GearBehaviour to prevent rotation

                Rigidbody rb = gear.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.isKinematic = true;

                isSnapping = true;

                // Deactivate this object's mesh
                MeshRenderer mesh = GetComponent<MeshRenderer>();
                if (mesh != null)
                    mesh.enabled = false;

                // Disable all children's MeshRenderers
                foreach (var childMesh in GetComponentsInChildren<MeshRenderer>())
                {
                    if (childMesh != mesh) // Avoid double-disabling the parent
                        childMesh.enabled = false;
                }
                if (mesh != null)
                    mesh.enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == objToSnap)
        {
            if (gear != null && other.transform == gear)
            {
                Debug.Log("Gear object exited the trigger zone.");
                gear.GetComponent<GearBehaviour>().enabled = true;
                Rigidbody rb = gear.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.isKinematic = false;

                isSnapping = false;
                gear = null;

                // Reactivate this object's mesh
                MeshRenderer mesh = GetComponent<MeshRenderer>();
                foreach (var childMesh in GetComponentsInChildren<MeshRenderer>())
                {
                    if (childMesh != mesh) // Avoid double-disabling the parent
                        childMesh.enabled = true;
                }
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

            // Combine parent's rotation (X, Y) with gear's own local Z rotation, and rotate Z with forcedRotationSpeed
            Vector3 parentEuler = transform.rotation.eulerAngles;
            Vector3 gearEuler = gear.rotation.eulerAngles;
            float gearZ = gearEuler.z + forcedRotationSpeed * Time.deltaTime;

            // Set rotation: parent's X and Y, gear's Z (rotated)
            gear.rotation = Quaternion.Euler(parentEuler.x, parentEuler.y, gearZ);
        }
    }
}
