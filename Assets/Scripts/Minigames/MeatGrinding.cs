using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeatGrinding : Minigame
{
    [Header("Settings")]
    [SerializeField]
    private float grindAngle;

    public override string Name => "Grind";

    private float currentAngle = 0f;

    public override void StartMinigame()
    {
        base.StartMinigame();

        InputAdapter.look.performed += OnMouseMove;
    }

    public override void EndMinigame(bool win)
    {
        base.EndMinigame(win);

        InputAdapter.look.performed -= OnMouseMove;
        currentAngle = 0f;
    }

    private void OnMouseMove(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<Vector2>());
    }
}
