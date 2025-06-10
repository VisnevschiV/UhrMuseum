using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TwoHandDisassemble : XRGrabInteractable
{
    [SerializeField]
    private List<Transform> elementsToDisassemble;

    [SerializeField]
    private float disassembleDistanceMultiplier = 2f; // How much to move along local Z

    [SerializeField]
    private float returnDuration = 1f; // Time in seconds to return to initial positions

    [SerializeField]
    private List<float> directionMultipliers = new List<float>(); // -1, 0, 0.5, 1, etc.

    private IXRSelectInteractor firstInteractor;
    private IXRSelectInteractor secondInteractor;

    private float initialDistance;
    private Vector3[] initialPositions;
    private Vector3 centerPoint;

    private Coroutine returnCoroutine;

    protected override void Awake()
    {
        base.Awake();
        if (elementsToDisassemble == null || elementsToDisassemble.Count == 0)
        {
            Debug.LogError("No elements to disassemble assigned.");
            enabled = false; // Disable the script if no elements are set
            return;
        }

        CacheInitialPositions();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        UpdateInteractors();

        if (interactorsSelecting.Count == 2)
        {
            initialDistance = GetInteractorDistance();
            centerPoint = transform.position;
        }
        GetComponent<NextStageOnGrab>()?.OnGrab();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        UpdateInteractors();

        // Start coroutine to return elements to initial positions
        if (returnCoroutine != null)
            StopCoroutine(returnCoroutine);
        returnCoroutine = StartCoroutine(ReturnElementsToInitialPositions());
        GetComponent<NextStageDrop>()?.OnDrop();
    }

    void Update()
    {
        if (firstInteractor != null && secondInteractor != null && elementsToDisassemble != null && elementsToDisassemble.Count > 0)
        {
            float currentDistance = GetInteractorDistance();
            if (initialDistance > 0.001f)
            {
                float ratio = Mathf.Clamp01((currentDistance - initialDistance) / initialDistance);

                for (int i = 0; i < elementsToDisassemble.Count; i++)
                {
                    if (elementsToDisassemble[i] == null) continue;
                    Vector3 initialLocalPos = initialPositions[i];
                    float dir = (directionMultipliers != null && i < directionMultipliers.Count) ? directionMultipliers[i] : 1f;
                    // Move by a factor of disassembleDistanceMultiplier * dir
                    Vector3 targetLocalPos = new Vector3(
                        initialLocalPos.x,
                        initialLocalPos.y + (disassembleDistanceMultiplier * dir),
                        initialLocalPos.z
                    );
                    elementsToDisassemble[i].localPosition = Vector3.Lerp(initialLocalPos, targetLocalPos, ratio);
                }
            }
        }
    }

    private void UpdateInteractors()
    {
        firstInteractor = interactorsSelecting.Count > 0 ? interactorsSelecting[0] : null;
        secondInteractor = interactorsSelecting.Count > 1 ? interactorsSelecting[1] : null;
    }

    private float GetInteractorDistance()
    {
        if (firstInteractor == null || secondInteractor == null)
            return 0f;

        var posA = firstInteractor.GetAttachTransform(this).position;
        var posB = secondInteractor.GetAttachTransform(this).position;
        return Vector3.Distance(posA, posB);
    }

    private void CacheInitialPositions()
    {
        initialPositions = new Vector3[elementsToDisassemble.Count];
        for (int i = 0; i < elementsToDisassemble.Count; i++)
        {
            initialPositions[i] = elementsToDisassemble[i].localPosition;
        }
    }

    private System.Collections.IEnumerator ReturnElementsToInitialPositions()
    {
        float elapsed = 0f;
        // Store current positions
        Vector3[] startPositions = new Vector3[elementsToDisassemble.Count];
        for (int i = 0; i < elementsToDisassemble.Count; i++)
        {
            startPositions[i] = elementsToDisassemble[i] != null ? elementsToDisassemble[i].localPosition : Vector3.zero;
        }

        while (elapsed < returnDuration)
        {
            float t = elapsed / returnDuration;
            for (int i = 0; i < elementsToDisassemble.Count; i++)
            {
                if (elementsToDisassemble[i] == null) continue;
                elementsToDisassemble[i].localPosition = Vector3.Lerp(startPositions[i], initialPositions[i], t);
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
        // Ensure final position is set
        for (int i = 0; i < elementsToDisassemble.Count; i++)
        {
            if (elementsToDisassemble[i] == null) continue;
            elementsToDisassemble[i].localPosition = initialPositions[i];
        }
        returnCoroutine = null;
    }
}