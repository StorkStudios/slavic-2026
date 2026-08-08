using StorkStudios.CoreNest;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractiveTrigger : SceneTrigger, IInteractable
{
    [SerializeField]
    private string actionName;
    [SerializeField]
    private string interactSound;

    public string ActionName => actionName;
    public string InteractSound => interactSound;

    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        Set();
    }
}
