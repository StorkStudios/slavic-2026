using StorkStudios.CoreNest;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractiveTrigger : SceneTrigger, IInteractable
{
    [SerializeField]
    private string actionName;

    public string ActionName => actionName;

    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        Set();
    }
}
