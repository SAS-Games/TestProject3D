using UnityEngine;
using UnityEngine.InputSystem;

public class AdvancedCharacterPushObject : CharacterPushObject
{
    [SerializeField] private InputActionReference pushAction; // Reference to the Input Action for pushing

    private void OnEnable()
    {
        pushAction.action.Enable();
    }

    private void OnDisable()
    {
        pushAction.action.Disable();
    }

    protected override bool ShouldPushObject(ControllerColliderHit hit)
    {
        return IsTriggerPressed() && IsMovingTowardObject(hit);
    }

    private bool IsTriggerPressed()
    {
        return pushAction.action.IsPressed();
    }

    private bool IsMovingTowardObject(ControllerColliderHit hit)
    {
        return true;
        var playerMovementDirection = _characterController.velocity.normalized;
        var objectDirection = (hit.transform.position - transform.position).normalized;

        return Vector3.Dot(playerMovementDirection, objectDirection) > 0.5f; // Adjust threshold as needed
    }
}
