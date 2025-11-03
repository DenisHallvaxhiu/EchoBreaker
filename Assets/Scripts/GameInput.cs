using UnityEngine;
using System;

public class GameInput : MonoBehaviour {

    public static GameInput Instance { get; private set; }
    public event EventHandler OnDashAction;

    public enum Binding {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Dash,
    }

    private PlayerInput playerInput;

    private void Awake() {
        Instance = this;
        playerInput = new PlayerInput();
        playerInput.Player.Enable();
        playerInput.Player.Dash.performed += Dash_performed;
    }

    private void Dash_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnDashAction?.Invoke(this,EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = playerInput.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }
}
