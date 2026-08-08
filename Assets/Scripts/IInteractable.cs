using UnityEngine;

public interface IInteractable
{
    public string ActionName { get; }
    public string InteractSound { get; }

    public void Interact();

    public bool CanInteract();
}
