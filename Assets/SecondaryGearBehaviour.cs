using UnityEngine;
using UnityEngine.Animations;

public class SecondaryGearBehaviour : GearBehaviour
{
    public GearBehaviour primaryGear; // Reference to the primary gear

    private void Update()
    {}
    public override void StartMoving(float speed, bool forceSpeed=false)
    {
        if (forceSpeed)
        {
            rotationSpeed = speed;
            isMoving = true;
            return;
        }
        
        // Start moving the primary gear instead of this gear
        if (primaryGear != null)
        {
            primaryGear.StartMoving(speed / teethCount, true); // Force speed to primary gear
        }

        base.StartMoving(speed);

    }
    
}
