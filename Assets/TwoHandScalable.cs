using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TwoHandScalable : XRGrabInteractable
{
    private IXRSelectInteractor firstInteractor;
    private IXRSelectInteractor secondInteractor;

    private float initialDistance;
    private Vector3 initialScale;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        UpdateInteractors();

        // When exactly two interactors are selecting, set initial values
        if (interactorsSelecting.Count == 2)
        {
            initialDistance = GetInteractorDistance();
            initialScale = transform.localScale;
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        UpdateInteractors();
    }

    void Update()
    {
        if (firstInteractor != null && secondInteractor != null)
        {
            float currentDistance = GetInteractorDistance();
            if (initialDistance > 0.001f)
            {
                float scaleRatio = currentDistance / initialDistance;
                transform.localScale = initialScale * scaleRatio;
            }
        }
    }

    private void UpdateInteractors()
    {
        // Always keep first and second up to date
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
}
