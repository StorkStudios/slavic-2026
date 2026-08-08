using DG.Tweening;
using StorkStudios.CoreNest;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : Singleton<PlayerController>
{
    [SerializeField]
    private Transform camera;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float sprintSpeed;
    [SerializeField]
    private Vector2 sensitivity;
    [SerializeField]
    private float cameraShakeStrength;
    [SerializeField]
    private int cameraShakeVibrato;
    [SerializeField]
    private float cameraShakeRandomness;

    [HideInInspector]
    public bool active = true;

    private CharacterController characterController;

    private Vector2 input;

    private Vector3 velocity;

    private float cameraXAngle;

    bool isSprinting = false;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        cameraXAngle = camera.transform.rotation.eulerAngles.x;

        InputAdapter.move.performed += OnMove;
        InputAdapter.move.canceled += OnMove;

        InputAdapter.look.performed += OnLook;

        InputAdapter.sprint.performed += OnSprintStart;
        InputAdapter.sprint.canceled += OnSprintStop;

        isSprinting = false;
    }

    private void OnSprintStart(InputAction.CallbackContext obj)
    {
        isSprinting = true;
    }

    private void OnSprintStop(InputAction.CallbackContext obj)
    {
        isSprinting = false;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        InputAdapter.move.performed -= OnMove;
        InputAdapter.move.canceled -= OnMove;

        InputAdapter.look.performed -= OnLook;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        if (!active)
        {
            return;
        }

        Vector2 mouse = context.ReadValue<Vector2>();
        Vector3 euler = transform.rotation.eulerAngles;
        euler.y += mouse.x * sensitivity.x;
        transform.rotation = Quaternion.Euler(euler);

        cameraXAngle -= mouse.y * sensitivity.y;
        cameraXAngle = Mathf.Clamp(cameraXAngle, -90, 90);
        camera.transform.localRotation = Quaternion.Euler(cameraXAngle, 0, 0);
    }

    private void Update()
    {
        if (!active)
        {
            return;
        }

        float ySpeed = velocity.y;

        if (characterController.isGrounded && ySpeed <= 0)
        {
            ySpeed = -1;
        }
        else
        {
            ySpeed += Physics.gravity.y * Time.deltaTime;
        }

        velocity = (transform.forward * input.y + transform.right * input.x) * (isSprinting ? sprintSpeed : speed);
        velocity.y += ySpeed;

        characterController.Move(Time.deltaTime * velocity);
    }

    public void ShakeCamera(float duration)
    {
        camera.GetChild(0).DOShakeRotation(duration, cameraShakeStrength, cameraShakeVibrato, cameraShakeRandomness);
    }
}
