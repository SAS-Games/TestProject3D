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
        return pushAction.action.IsPressed();
    }
}
