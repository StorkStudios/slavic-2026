using StorkStudios.CoreNest;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : Singleton<PlayerInteractor>
{
    [SerializeField]
    private float interactionRange;
    [SerializeField]
    private Transform camera;
    [SerializeField]
    private LayerMask interactableLayerMask;

    public bool CanInteract => interactable != null && interactable.CanInteract();
    public IInteractable Interactable => interactable;

    private IInteractable interactable;

    private void Start()
    {
        InputAdapter.interact.performed += OnInteract;
    }

    protected override void OnDestroy()
    {
        InputAdapter.interact.performed -= OnInteract;
        base.OnDestroy();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (CanInteract)
        {
            interactable?.Interact();
            if (!string.IsNullOrEmpty(interactable?.InteractSound))
            {
                CommonSoundManager.Instance.PlaySound(interactable.InteractSound);
            }
        }
        else if (PlayerObjectHolder.Instance.CurrentObject is PickupableItem item && item.CanUse())
        {
            item.Use();
        }
    }

    private void Update()
    {
        if (Physics.Raycast(camera.position, camera.forward, out RaycastHit hitInfo, interactionRange, interactableLayerMask) &&
            (hitInfo.transform.TryGetComponent(out IInteractable newInteractable) ||
            (hitInfo.rigidbody != null && hitInfo.rigidbody.TryGetComponent(out newInteractable))))
        {
            interactable = newInteractable;
        }
        else
        {
            interactable = null;
        }
    }
}
