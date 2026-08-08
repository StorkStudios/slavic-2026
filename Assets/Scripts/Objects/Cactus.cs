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

    private bool canPet = true;

    public string ActionName => "Pet";

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
