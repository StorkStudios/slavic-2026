using StorkStudios.CoreNest;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : Singleton<PlayerInteractor>
{
    [SerializeField]
    private float interactionRange;
    [SerializeField]
    private Transform camera;

    public bool CanInteract => interactable != null && interactable.CanInteract();
    public IInteractable Interactable => interactable;

    private IInteractable interactable;

    private void Start()
    {
        InputAdapter.interact.performed += OnInteract;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (CanInteract)
        {
            interactable?.Interact();
        }
    }

    private void Update()
    {
        if (Physics.Raycast(camera.position, camera.forward, out RaycastHit hitInfo, interactionRange) &&
            hitInfo.transform.TryGetComponent(out IInteractable newInteractable))
        {
            interactable = newInteractable;
        }
        else
        {
            interactable = null;
        }
    }
}
