using UnityEngine;
using UnityEngine.InputSystem;

public static class InputAdapter
{
    public static readonly InputAction move;
    public static readonly InputAction look;
    public static readonly InputAction interact;
    public static readonly InputAction sprint;
    public static readonly InputAction drop;
    public static readonly InputAction cancel;
    public static readonly InputAction checkTime;
    public static readonly InputAction toggleConsole;

    static InputAdapter()
    {
        move = InputSystem.actions.FindAction("Move");
        look = InputSystem.actions.FindAction("Look");
        interact = InputSystem.actions.FindAction("Interact");
        sprint = InputSystem.actions.FindAction("Sprint");
        drop = InputSystem.actions.FindAction("Drop");
        cancel = InputSystem.actions.FindAction("Cancel");
        checkTime = InputSystem.actions.FindAction("CheckTime");
        toggleConsole = InputSystem.actions.FindAction("ToggleConsole");
    }
}
