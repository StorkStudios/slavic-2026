using UnityEngine;
using UnityEngine.InputSystem;

public static class InputAdapter
{
    public static readonly InputAction move;
    public static readonly InputAction look;
    public static readonly InputAction interact;
    public static readonly InputAction sprint;

    static InputAdapter()
    {
        move = InputSystem.actions.FindAction("Move");
        look = InputSystem.actions.FindAction("Look");
        interact = InputSystem.actions.FindAction("Interact");
        sprint = InputSystem.actions.FindAction("Sprint");
    }
}
