using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField]
    private float interactionRange;
    [SerializeField]
    private Transform camera;

    public bool CanInteract => interactable != null;
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
            interactable?.Interact(gameObject);
        }
    }

    private void Update()
    {
        if (Physics.Raycast(camera.position, camera.forward, out RaycastHit hitInfo, interactionRange) &&
            hitInfo.rigidbody != null &&
            hitInfo.rigidbody.TryGetComponent(out IInteractable newInteractable))
        {
            interactable = newInteractable;
        }
        else
        {
            interactable = null;
        }
    }
}
