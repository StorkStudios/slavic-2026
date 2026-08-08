using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeatGrinding : Minigame
{
    [Header("Settings")]
    [SerializeField]
    private float grindAngle;

    public override string Name => "Grind";

    private float rotatedAngle = 0f;
    private bool pressed = false;
    private float lastAngle;
    private Vector2 screenHalf;

    public override void StartMinigame()
    {
        base.StartMinigame();
        screenHalf = new Vector2(Screen.width / 2f, Screen.height / 2f);

        InputAdapter.look.performed += OnMouseMove;
        InputAdapter.interact.started += OnMousePress;
        InputAdapter.interact.canceled += OnMouseRelease;
    }

    public override void EndMinigame(bool win)
    {
        base.EndMinigame(win);

        InputAdapter.look.performed -= OnMouseMove;
        InputAdapter.interact.started -= OnMousePress;
        InputAdapter.interact.canceled -= OnMouseRelease;
        rotatedAngle = 0f;
    }

    private void OnMouseMove(InputAction.CallbackContext context)
    {
        float angle = GetMouseAngle();
        float diff = GetAngleDelta(angle, lastAngle);
        if (pressed)
        {
            rotatedAngle += diff;
            Debug.Log(rotatedAngle);
            if (rotatedAngle >= grindAngle)
            {
                EndMinigame(true);
            }
        }
        lastAngle = angle;
    }

    private float GetAngleDelta(float currentAngle, float lastAngle)
    {
        if (lastAngle - currentAngle > 0f && lastAngle - currentAngle < 180f)
        {
            return lastAngle - currentAngle;
        }
        if (currentAngle > 270 && lastAngle < 90)
        {
            return currentAngle - 360 - lastAngle;
        }
        return 0f;
    }

    private void OnMouseRelease(InputAction.CallbackContext context)
    {
        pressed = false;
    }

    private void OnMousePress(InputAction.CallbackContext context)
    {
        pressed = true;
        lastAngle = GetMouseAngle();
    }

    private float GetMouseAngle()
    {
        return Vector2.SignedAngle(Vector2.up, Mouse.current.position.ReadValue() - screenHalf) + 180f;
    }
}
