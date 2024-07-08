using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    public event Action<Vector2> OnCharacterMovement = delegate { };
    public event Action OnActionSelected = delegate { };

    public void OnMovementInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnCharacterMovement?.Invoke(context.ReadValue<Vector2>());
        }
    }

    public void OnActionInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnActionSelected?.Invoke();
        }
    }
}
