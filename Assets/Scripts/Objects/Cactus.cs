using StorkStudios.CoreNest;
using UnityEngine;

public class Cactus : MonoBehaviour, IInteractable
{
    [SerializeField]
    private ParticleSystem particles;
    [SerializeField]
    private float petCooldown;
    [SerializeField]
    private float hotinReduceAmount;
    [SerializeField]
    private string actionName = "Pet";
    [SerializeField]
    private string interactSound;

    private bool canPet = true;

    public string InteractSound => interactSound;
    public string ActionName => actionName;

    public bool CanInteract()
    {
        return canPet;
    }

    public void Interact()
    {
        Hotin.Instance.Value -= hotinReduceAmount;
        particles.Play();
        canPet = false;
        this.CallDelayed(petCooldown, () => canPet = true);
    }
}
