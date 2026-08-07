using UnityEngine;

public interface IInteractable
{
    public string ActionName { get; }

    public void Interact();

    public bool CanInteract();
}
