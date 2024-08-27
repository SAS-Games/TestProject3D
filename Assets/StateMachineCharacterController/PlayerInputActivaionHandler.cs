using SAS.StateMachineCharacterController;
using SAS.Utilities.TagSystem;
using UnityEngine;

public class PlayerInputActivaionHandler : MonoBehaviour
{
    [FieldRequiresSelf] private InputHandler _inputHandler;
    private EventBinding<DialogueStartEvent> _dialogueStartEventBinding;
    private EventBinding<DialogueEndEvent> _dialogueEndEventBinding;
    void Start()
    {
        this.Initialize();

        _dialogueStartEventBinding = new EventBinding<DialogueStartEvent>(_ =>
        {
            _inputHandler.enabled = false;
        });
        _dialogueEndEventBinding = new EventBinding<DialogueEndEvent>(_ => _inputHandler.enabled = true);

        EventBus<DialogueStartEvent>.Register(_dialogueStartEventBinding);
        EventBus<DialogueEndEvent>.Register(_dialogueEndEventBinding);
    }

    void OnDestroy()
    {
        EventBus<DialogueStartEvent>.Deregister(_dialogueStartEventBinding);
        EventBus<DialogueEndEvent>.Deregister(_dialogueEndEventBinding);
    }
}
