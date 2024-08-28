using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Movement _movement;

    private void Awake()
    {
        _movement = GetComponent<Movement>();
    }

    private void OnEnable()
    {
        RegisterInputs();
    }

    private void OnDisable()
    {
        UnregisterInputs();
    }

    private void RegisterInputs()
    {
        InputAction moveAction = InputManager.Instance.GameControls.FindAction(name);
        moveAction.Enable();
        moveAction.performed += OnMovePerformed;
        moveAction.canceled += OnMoveCanceled;
    }

    private void UnregisterInputs()
    {
        InputAction moveAction = InputManager.Instance.GameControls.FindAction(name);
        moveAction.Disable();
        moveAction.performed -= OnMovePerformed;
        moveAction.canceled -= OnMoveCanceled;
    }

    private void SetMovement(Vector2 direction)
    {
        _movement.SetDirection(new Vector3(direction.x + direction.y, 0f, direction.y - direction.x).normalized);
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        SetMovement(context.ReadValue<Vector2>());
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        SetMovement(Vector2.zero);
    }
}